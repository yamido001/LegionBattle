using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace NewFileSystem
{
	public class CompareFileListState : IDownloadState {

		private FileDownloadData mSynchronizeData;
		private Action<FileDownloadStateId> mNextHandler;
		private Action<FileErrorCode, string> mErrorHandler;
		private bool mLocalFileListHasChange = false;

		public FileDownloadStateId StateId
		{
			get{ return FileDownloadStateId.CompareFileList; }
		}

		public void Init (FileDownloadData synchronizerData, Action<FileDownloadStateId> hdlNextState, Action hdlFinish, Action<FileErrorCode, string> hdlError)
		{
			mSynchronizeData = synchronizerData;
			mNextHandler = hdlNextState;
			mErrorHandler = hdlError;
		}

		public void Update()
		{

		}

		public void Enter ()
		{
			RemoveUnusedFileList ();
			GetUpdatedFile ();
			EnterNextState ();
		}

		public void Exit()
		{

		}

		private void RemoveUnusedFileList()
		{
			//找到本地存在，但是服务器中不存在的文件，在硬盘中删除
			var enumerator = mSynchronizeData.persistentFileListDic.GetEnumerator ();
			List<FileDetailInfo> removeList = new List<FileDetailInfo> ();
			while (enumerator.MoveNext ()) {
				FileDetailInfo fileInfo = enumerator.Current.Value;
				if (mSynchronizeData.serverFileListDic.ContainsKey (enumerator.Current.Key)) {
					continue;
				}
				removeList.Add (enumerator.Current.Value);
			}
			for (int i = 0; i < removeList.Count; ++i) {
				FileDetailInfo removeFileInfo = removeList [i];
				string filePath = FileSystemUtils.GetFileRelativePath (removeFileInfo.fileName, removeFileInfo.filePath, true);
				GameMain.Instance.FileOperateMgr.DeleteIfExist (filePath);
				mSynchronizeData.persistentFileListDic.Remove (removeFileInfo.fileName);
				mLocalFileListHasChange = true;
			}
		}

		private void GetUpdatedFile()
		{
			mSynchronizeData.needDownloadSet = new List<FileDetailInfo> ();
			var enumerator = mSynchronizeData.serverFileListDic.GetEnumerator ();
			while (enumerator.MoveNext ()) {
				FileDetailInfo serverDetailInfo = enumerator.Current.Value;
				FileDetailInfo localDetailInfo = null;
				if (!mSynchronizeData.persistentFileListDic.TryGetValue (serverDetailInfo.fileName, out localDetailInfo)) {
					FileDetailInfo streamingDetailInfo;
					if (null != mSynchronizeData.streamingFileListDic && mSynchronizeData.streamingFileListDic.TryGetValue (serverDetailInfo.fileName, out streamingDetailInfo)) {
						if (streamingDetailInfo.fileMd5 != serverDetailInfo.fileMd5 || streamingDetailInfo.filePath != serverDetailInfo.filePath) {
							mSynchronizeData.needDownloadSet.Add (serverDetailInfo);
						} 
						else {
							//在StreamingAssets目录下存在相同的文件，不需要下载
						}
					} 
					else {
						mSynchronizeData.needDownloadSet.Add (serverDetailInfo);
					}
					continue;
				}
				if (localDetailInfo.fileMd5 != serverDetailInfo.fileMd5) {
					mSynchronizeData.needDownloadSet.Add (serverDetailInfo);
					string localFilePath = FileSystemUtils.GetFileRelativePath (localDetailInfo.fileName, localDetailInfo.filePath, true);
					GameMain.Instance.FileOperateMgr.DeleteIfExist (localFilePath);
					mSynchronizeData.persistentFileListDic.Remove (localDetailInfo.fileName);
					mLocalFileListHasChange = true;
					continue;
				}
				if (localDetailInfo.filePath != serverDetailInfo.filePath) {
					string fromLocalFilePath = FileSystemUtils.GetFileRelativePath (localDetailInfo.fileName, localDetailInfo.filePath, true);
					string toLocalFilePath = FileSystemUtils.GetFileRelativePath (serverDetailInfo.fileName, serverDetailInfo.filePath, true);
					GameMain.Instance.FileOperateMgr.MoveFile (fromLocalFilePath, toLocalFilePath);
					localDetailInfo.filePath = serverDetailInfo.filePath;
					mLocalFileListHasChange = true;
				}
			}
		}

		private void EnterNextState()
		{
			if (mLocalFileListHasChange) {
				string fileOpeError = string.Empty;
				FileErrorCode result = FileOperateUtils.TryFileWrite (delegate() {
					string fileContent = FileListUtils.FileListToString(mSynchronizeData.persistentFileListDic);
					string relativePath = FileSystemUtils.GetFileRelativePath (FileDownloadData.FileListFileName, string.Empty);
					GameMain.Instance.FileOperateMgr.WriteTextFile (relativePath, fileContent, false);
				}, out fileOpeError);
				if (FileErrorCode.Null != result) {
					mErrorHandler.Invoke (result, "Write filelist");
					return;
				}
			}
			if (mSynchronizeData.needDownloadSet.Count > 0) {
				mNextHandler.Invoke (FileDownloadStateId.DownLoadFile);
			}
			else {
				mNextHandler.Invoke (FileDownloadStateId.WritePersistentVersionFile);
			}
		}
	}
}