using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace NewFileSystem
{
	public class AndroidStreamingCopy : MonoBehaviour{

		private System.Action mHdlOnFinish;
		private Dictionary<string, FileDetailInfo> mStreamingFileListDic;
		private Dictionary<string, FileDetailInfo> mPersistentFileListDic;
		private List<string> mCopyList = new List<string>();
		private string mPersistentFileListRelativePath;
		private int mCopyFileMaxCount;

		public void Init (System.Action hdlOnFinish)
		{
			mHdlOnFinish = hdlOnFinish;
		}

		public void BeginCopy()
		{
			Lancher.Instance.SetTips ("资源检查中");
			LoadStreamingFileList ();
		}

		private void LoadStreamingFileList()
		{
			string relativePath = FileSystemUtils.GetFileRelativePath (FileDownloadData.FileListFileName, string.Empty);
			this.StartCoroutine (LoadFile (relativePath, delegate(byte[] fileBytes) {
				//保存的文件是无BOM形式的，所以直接用Encoding.UTF8.GetString不会有问题
				string fileContent = Encoding.UTF8.GetString(fileBytes);
				FileListUtils.FileListParseError error;
				mStreamingFileListDic = FileListUtils.StringToFileList(fileContent, out error);
				if(FileListUtils.FileListParseError.Null == error)
				{
					LoadPersistentDataFileList();
				}
				else
				{
					OnError("Parse streaming fileList " + error.ToString());
				}
			}, delegate() {
				OnError("Load streaming fileList");
			}));
		}

		private void LoadPersistentDataFileList()
		{
			mPersistentFileListRelativePath = FileSystemUtils.GetFileRelativePath (FileDownloadData.FileListFileName, string.Empty);
			if (!GameMain.Instance.FileOperateMgr.IsFileExist (mPersistentFileListRelativePath)) {
				GenerateCopyList();
				return;
			}
			string fileContent = GameMain.Instance.FileOperateMgr.ReadAsText (mPersistentFileListRelativePath);
			FileListUtils.FileListParseError error;
			mPersistentFileListDic = FileListUtils.StringToFileList(fileContent, out error);
			if(FileListUtils.FileListParseError.Null == error)
			{
				GenerateCopyList();
			}
			else
			{
				OnError("Parse persistent fileList " + error.ToString());
			}
		}

		private void GenerateCopyList()
		{
			bool hasPersistentListChanged = false;
			Dictionary<string, FileDetailInfo>.Enumerator enumerateo = mStreamingFileListDic.GetEnumerator ();
			while (enumerateo.MoveNext ()) {
				string fileName = enumerateo.Current.Key;
				FileDetailInfo streamDetailInfo = enumerateo.Current.Value;
				FileDetailInfo persistentDetailInfo = null;

				if (null == mPersistentFileListDic || !mPersistentFileListDic.TryGetValue (fileName, out persistentDetailInfo)) {
					mCopyList.Add (fileName);
				} 
				else {
					if (persistentDetailInfo.fileMd5 != streamDetailInfo.fileMd5 || persistentDetailInfo.filePath != streamDetailInfo.filePath) {
						string relativePath = FileSystemUtils.GetFileRelativePath (fileName, persistentDetailInfo.filePath, true);
						GameMain.Instance.FileOperateMgr.DeleteIfExist (relativePath);
						mCopyList.Add (fileName);
						hasPersistentListChanged = true;
						mPersistentFileListDic.Remove (fileName);
					}
				}
			}

			if (hasPersistentListChanged) {
				//使Persistent目录下的文件列表和实际文件一致，避免下一阶段拷贝中断造成文件列表和实际文件不一致
				string errorStr;
				FileErrorCode errorCode = FileListUtils.WriteFileList (mPersistentFileListDic, out errorStr);
				if (FileErrorCode.Null != errorCode) {
					OnError ("GenerateCopyList write persistent fileList " + errorCode.ToString () + " " + errorStr);
					return;
				}
			}
			mCopyFileMaxCount = mCopyList.Count;
			CopyFiles ();
		}

		private void CopyFiles()
		{
			Lancher.Instance.SetTips ("请稍等，正在解压资源......(" + (mCopyFileMaxCount - mCopyList.Count).ToString() + "/" + mCopyFileMaxCount + ")");
			if (mCopyList.Count == 0) {
				mHdlOnFinish.Invoke ();
				return;
			}
			string fileName = mCopyList[mCopyList.Count - 1];
			mCopyList.RemoveAt (mCopyList.Count - 1);
			FileDetailInfo fileInfo = mStreamingFileListDic [fileName];
			string relativePath = FileSystemUtils.GetFileRelativePath (fileName, fileInfo.filePath, true);
			this.StartCoroutine (LoadFile (relativePath, delegate(byte[] fileBytes) {
				GameMain.Instance.FileOperateMgr.CreateDirIfNotExist(Path.GetDirectoryName(relativePath));
				string fileOpeErrorStr;
				FileErrorCode errorCode = FileOperateUtils.TryFileWrite(delegate(){
					Logger.LogInfo("AndroidStreamingCopy copy file " + relativePath);
					GameMain.Instance.FileOperateMgr.WriteBinaryFile(relativePath, fileBytes);
				}, out fileOpeErrorStr);

				if(FileErrorCode.Null != errorCode)
				{
					OnError("AndroidStreamingCopy Write " + relativePath + " " + errorCode.ToString() + " " + fileOpeErrorStr );
					return;
				}

				errorCode = FileOperateUtils.TryFileWrite(delegate(){
					string detailStr = FileListUtils.DetailInfoToString(fileInfo);
					GameMain.Instance.FileOperateMgr.WriteTextFile(mPersistentFileListRelativePath, detailStr, true);
				}, out fileOpeErrorStr);
				if(FileErrorCode.Null != errorCode)
				{
					OnError("AndroidStreamingCopy write fileList " + errorCode.ToString() + " " + fileOpeErrorStr );
					return;
				}
				CopyFiles();
			}, delegate() {
				OnError("Load streaming FileList");
			}));
		}

		private void OnError(string errorCode)
		{
			//TODO show tips
//			App.ShowConfirm (delegate() {
//				App.Quit(LoadingLogProxy.QuitReason.StreamingCopyError);
//			}, errorCode);
		}

		private IEnumerator LoadFile(string relativePath, Action<byte[]> finishHdl, Action errorHdl)
		{
			string url = Path.Combine (Application.streamingAssetsPath, relativePath);
			#if UNITY_EDITOR
			url = "file:///" + url;
			#endif
			WWW reader = new WWW(url);
			yield return reader;
			if (string.IsNullOrEmpty(reader.error))
			{
				finishHdl.Invoke(reader.bytes);
			}
			else
			{
				errorHdl.Invoke();
			}
			reader.Dispose();
		}
	}
}
