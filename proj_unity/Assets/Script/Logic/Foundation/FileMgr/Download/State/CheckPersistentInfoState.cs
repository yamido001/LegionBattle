using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

namespace NewFileSystem
{
	public class CheckPersistentInfoState : IDownloadState {

		private FileDownloadData mSynchronizeData;
		private Action<FileDownloadStateId> mHdlNextState;
		private Action<FileErrorCode, string> mHdlError;

		public FileDownloadStateId StateId
		{
			get{ return FileDownloadStateId.CheckPersistentInfo; }
		}

		public void Init (FileDownloadData synchronizerData, Action<FileDownloadStateId> hdlNextState, Action hdlFinish, Action<FileErrorCode, string> hdlError)
		{
			mSynchronizeData = synchronizerData;
			mHdlNextState = hdlNextState;
		}

		public void Update()
		{

		}

		public void Enter ()
		{
			ShowTipInfo ();
			LoadInfo ();
			InfoCheck ();
			SyncInfoToHardDisk ();
		}

		public void Exit()
		{

		}

		private void ShowTipInfo()
		{
			Lancher.Instance.SetTips ("请求服务器资源信息");
		}

		/// <summary>
		/// 加载文件列表和Version信息，并做内容合法检查
		/// </summary>
		private void LoadInfo()
		{
			string localVersionFilePath = FileSystemUtils.GetFileRelativePath(FileDownloadData.VersionFileName, string.Empty);
			if (GameMain.Instance.FileOperateMgr.IsFileExist(localVersionFilePath)) {
				string localVersionContent = GameMain.Instance.FileOperateMgr.ReadAsText(localVersionFilePath);
				mSynchronizeData.localVersionInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<LocalVersionInfo> (localVersionContent);

			}
			if (null == mSynchronizeData.localVersionInfo) {
				mSynchronizeData.localVersionInfo = new LocalVersionInfo ();
				mSynchronizeData.localVersionInfo.Clear ();
			}

			string localFileListFilePath = FileSystemUtils.GetFileRelativePath(FileDownloadData.FileListFileName, string.Empty);
			Dictionary<string, FileDetailInfo> fileListDic = null;
			if (GameMain.Instance.FileOperateMgr.IsFileExist (localFileListFilePath)) {
				string fileListContent = GameMain.Instance.FileOperateMgr.ReadAsText (localFileListFilePath);
				FileListUtils.FileListParseError error = FileListUtils.FileListParseError.Null;
				fileListDic = FileListUtils.StringToFileList (fileListContent, out error);
				if (FileListUtils.FileListParseError.Null != error) {
					//文件列表已经被破坏，所以直接删除文件
					GameMain.Instance.FileOperateMgr.DeleteIfExist (localVersionFilePath);
					GameMain.Instance.FileOperateMgr.DeleteIfExist (localFileListFilePath);
				} 
				else {
					string localFileListMd5 = Utils.MD5 (fileListContent);
					if (localFileListMd5 != mSynchronizeData.localVersionInfo.persistentMd5) {
						//文件列表被修改
						mSynchronizeData.localVersionInfo.Clear();
					}
				}
			} 
			if (null == fileListDic) {
				//如果文件列表不存在，那么有可能是被其他系统删除，也有可能是文件解析失败，所以清空localVersionInfo，保证一定能够进入CompareFileListState状态
				mSynchronizeData.localVersionInfo.Clear();
				mSynchronizeData.persistentFileListDic = new Dictionary<string, FileDetailInfo> ();
			} 
			else {
				List<string> removeList = new List<string> ();
				Dictionary<string, FileDetailInfo>.Enumerator enumerator = fileListDic.GetEnumerator ();
				while (enumerator.MoveNext ()) {
					FileDetailInfo detailInfo = enumerator.Current.Value;
					if (detailInfo.IsInvalid()) {
						removeList.Add (enumerator.Current.Key);
					}
				}
				if (removeList.Count > 0) {
					//有部分信息是不合法的，那么一定需要进入CompareFileListState状态
					mSynchronizeData.localVersionInfo.Clear();
					for (int i = 0; i < removeList.Count; ++i) {
						fileListDic.Remove (removeList [i]);
					}
				}
				mSynchronizeData.persistentFileListDic = fileListDic;
			}
		}

		/// <summary>
		/// 保证内存中的文件列表和硬盘中的所有的真实文件一致，但是可能会和硬盘中的文件列表不一致
		/// </summary>
		private void InfoCheck()
		{
			List<string> removeList = new List<string> ();
			Dictionary<string, FileDetailInfo>.Enumerator enumerator = mSynchronizeData.persistentFileListDic.GetEnumerator ();
			while (enumerator.MoveNext ()) {
				FileDetailInfo detailInfo = enumerator.Current.Value;
				string localFilePath = FileSystemUtils.GetFileRelativePath (detailInfo.fileName, detailInfo.filePath, true);
				if(!GameMain.Instance.FileOperateMgr.IsFileExist(localFilePath)){
					//也有可能只是文件在其他目录，为了逻辑简单不做文件目录的恢复功能，如果文件目录不一致，就会直接删除掉，在后面的阶段重新下载
					removeList.Add (detailInfo.fileName);
					continue;
				}

				long fileLength = GameMain.Instance.FileOperateMgr.GetFileLength(localFilePath);
				if (fileLength != detailInfo.fileLength) {
					removeList.Add (detailInfo.fileName);
					GameMain.Instance.FileOperateMgr.DeleteIfExist (localFilePath);
					continue;
				}
			}

			if (removeList.Count > 0) {
				for (int i = 0; i < removeList.Count; ++i) {
					mSynchronizeData.persistentFileListDic.Remove (removeList [i]);
				}
				mSynchronizeData.localVersionInfo.Clear();
			}

			string relativePath = FileSystemUtils.GetFolderRelativePath (string.Empty);
			DirectoryInfo directoryInfo = GameMain.Instance.FileOperateMgr.GetDirectoryInfo (relativePath);
			CheckDiskFile (directoryInfo, string.Empty);
		}

		private void CheckDiskFile(DirectoryInfo directoryInfo, string relativePath)
		{
			FileInfo[] fileInfos = directoryInfo.GetFiles ();
			for (int i = 0; i < fileInfos.Length; ++i) {
				FileInfo fileInfo = fileInfos [i];
				string fileName = fileInfo.Name;
				bool needRemove = false;
				FileDetailInfo info = null;
				if (mSynchronizeData.persistentFileListDic.TryGetValue (fileName, out info)) {
					//文件列表中存在，需要判断路径是否一致
					if (relativePath != info.filePath) {
						//路径不一致，需要删除文件
						needRemove = true;
					} else {
						//路径一致时，在文件列表到硬盘的检查中已经覆盖，不需要处理
					}
				} 
				else {
					//文件列表中不存在
					needRemove = true;
				}
				if (needRemove) {
					string fileRelativePath = FileSystemUtils.GetFileRelativePath (fileName, relativePath, true);
					GameMain.Instance.FileOperateMgr.DeleteIfExist (fileRelativePath);
					mSynchronizeData.persistentFileListDic.Remove (fileName);
					mSynchronizeData.localVersionInfo.Clear ();
				}
			}

			DirectoryInfo[] childDirectories = directoryInfo.GetDirectories ();
			for (int i = 0; i < childDirectories.Length; ++i) {
				DirectoryInfo childDirectory = childDirectories [i];
				CheckDiskFile(childDirectory, Path.Combine(relativePath, childDirectory.Name));
			}
			if (directoryInfo.GetFiles ().Length == 0 && directoryInfo.GetDirectories().Length == 0) {
				directoryInfo.Delete ();
			}
		}

		/// <summary>
		/// 把文件列表和Version信息同步到硬盘
		/// </summary>
		private void SyncInfoToHardDisk()
		{
			string fileOpeError = string.Empty;
			string fileListMd5 = string.Empty;
			//同步Version信息
			FileErrorCode result = FileOperateUtils.TryFileWrite (delegate() {
				string fileContent = FileListUtils.FileListToString (mSynchronizeData.persistentFileListDic);
				fileListMd5 = Utils.MD5(fileContent);
				string relativePath = FileSystemUtils.GetFileRelativePath (FileDownloadData.FileListFileName, string.Empty);
				GameMain.Instance.FileOperateMgr.WriteTextFile (relativePath, fileContent, false);
			}, out fileOpeError);

			//同步文件列表信息
			if (FileErrorCode.Null == result) {
				mSynchronizeData.localVersionInfo.persistentMd5 = fileListMd5;
				result = FileOperateUtils.TryFileWrite (delegate() {
					string fileContent = Newtonsoft.Json.JsonConvert.SerializeObject (mSynchronizeData.localVersionInfo);
					string relativePath = FileSystemUtils.GetFileRelativePath (FileDownloadData.VersionFileName, string.Empty);
					GameMain.Instance.FileOperateMgr.WriteTextFile (relativePath, fileContent, false);
				}, out fileOpeError);

				if (FileErrorCode.Null == result) {
					mHdlNextState.Invoke (FileDownloadStateId.LoadStreamingFileList);
				} 
				else {
					mHdlError.Invoke (result, "Version info");
				}
			} 
			else {
				mHdlError.Invoke (result, "FileList info");
			}
		}
	}
}