using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

namespace NewFileSystem
{
	public class GetServerFileListState : IDownloadState {

		private FileDownloadData mSynchronizeData;
		private System.Action<FileDownloadStateId> mNextStateHandler;
		private Action<FileErrorCode, string> mErrorHandler;
		int mDownloadId = int.MinValue;

		#region Interface function
		public FileDownloadStateId StateId
		{
			get{ return FileDownloadStateId.GetServerFileList; }
		}

		public void Init (FileDownloadData synchronizerData, System.Action<FileDownloadStateId> nextStateHandler, Action finishHandler, Action<FileErrorCode, string> hdlError)
		{
			mSynchronizeData = synchronizerData;
			mNextStateHandler = nextStateHandler;
			mErrorHandler = hdlError;
		}

		public void Update()
		{

		}

		public void Enter ()
		{
			RequestServerFileList ();
		}

		public void Exit()
		{
			GameMain.Instance.HttpMgr.RemoveDownload (mDownloadId);
		}
		#endregion

		void RequestServerFileList()
		{
			mDownloadId = GameMain.Instance.HttpMgr.DownLoad (mSynchronizeData.ServerFileListPath, delegate(byte[] data) {

				string fileContent = Encoding.UTF8.GetString (data);
				FileListUtils.FileListParseError errorCode;
				mSynchronizeData.serverFileListDic = FileListUtils.StringToFileList (fileContent, out errorCode);
				if (FileListUtils.FileListParseError.Null != errorCode) {
					mErrorHandler.Invoke (FileErrorCode.ParseFileListError, "Parse server fileList " + errorCode.ToString());
					return;
				}
				if (mSynchronizeData.serverFileListDic.Count == 0) {
					mErrorHandler.Invoke (FileErrorCode.ParseFileListError, "Server fileList parse error, file length:" + fileContent.Length);
					return;
				}
				mNextStateHandler.Invoke (FileDownloadStateId.CompareFileList);

			}, delegate(string error) {
				mErrorHandler.Invoke (FileErrorCode.DownLoadFileListError, " Request server file list error: " + error);
			});
		}
	}
}