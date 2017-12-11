using UnityEngine;
using System.Collections;
using System;

namespace NewFileSystem
{
	public class WritePersistentVersionFileState : IDownloadState {

		private FileDownloadData mSynchronizeData;
		private Action<FileErrorCode, string> mErrorHandler;
		private Action mFinishHandler;

		public FileDownloadStateId StateId
		{
			get{ return FileDownloadStateId.WritePersistentVersionFile; }
		}

		public void Init (FileDownloadData synchronizerData, Action<FileDownloadStateId> hdlNextState, Action hdlFinish, Action<FileErrorCode, string> hdlError)
		{
			mSynchronizeData = synchronizerData;
			mErrorHandler = hdlError;
			mFinishHandler = hdlFinish;
		}

		public void Update()
		{

		}

		public void Enter ()
		{
			WriteVersionFile ();
		}

		public void Exit()
		{

		}

		private void WriteVersionFile()
		{
			string persistentFileListRelativePath = FileSystemUtils.GetFileRelativePath(FileDownloadData.FileListFileName, string.Empty);
			byte[] fileBytes = GameMain.Instance.FileOperateMgr.ReadAsBinary (persistentFileListRelativePath);
			mSynchronizeData.localVersionInfo.persistentMd5 = Utils.MD5 (fileBytes);

			mSynchronizeData.localVersionInfo.serverMd5 = mSynchronizeData.serverVersionInfo.md5;
			string relativePath = FileSystemUtils.GetFileRelativePath (FileDownloadData.VersionFileName, string.Empty);
			string fileContent = Newtonsoft.Json.JsonConvert.SerializeObject (mSynchronizeData.localVersionInfo);

			string fileOpeError = string.Empty;
			FileErrorCode result = FileOperateUtils.TryFileWrite (delegate() {
				GameMain.Instance.FileOperateMgr.WriteTextFile (relativePath, fileContent, false);
			}, out fileOpeError);
			if (FileErrorCode.Null == result) {
				mFinishHandler.Invoke ();	
			} 
			else {
				mErrorHandler.Invoke (result,  relativePath);
			}
		}
	}
}