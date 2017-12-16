using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

namespace NewFileSystem
{
	public class LoadStreamingFileList : IDownloadState {

		private FileDownloadData mSynchronizeData;
		private Action<FileDownloadStateId> mNextStateHandler;
		private Action<FileErrorCode, string> mErrorHandler;
		private Action mFinishHandler;

		#region Interface function
		public FileDownloadStateId StateId
		{
			get{ return FileDownloadStateId.CompareVersion; }
		}

		public void Init (FileDownloadData synchronizerData, System.Action<FileDownloadStateId> nextStateHandler, System.Action finishHandler, Action<FileErrorCode, string> hdlError)
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
			if (FileSystemUtils.CanUseStreamingFile ()) {
				LoadFileList ();
			} 
			else {
				Finish ();
			}
		}

		public void Exit()
		{

		}
		#endregion

		private void LoadFileList()
		{
			string fileListRelativePath = FileSystemUtils.GetFileRelativePath (FileDownloadData.FileListFileName, string.Empty);
			string filelistPath = Path.Combine (Application.streamingAssetsPath, fileListRelativePath);
			if (!File.Exists (filelistPath)) {
				Finish ();
				return;
			}
			string fileContent = GameMain.Instance.FileOperateMgr.ReadAsText (filelistPath);
			FileListUtils.FileListParseError error;
			mSynchronizeData.streamingFileListDic = FileListUtils.StringToFileList (fileContent, out error);
			if (error != FileListUtils.FileListParseError.Null) {
				mErrorHandler.Invoke (FileErrorCode.ParseFileListError, "Load streaming file list " + error.ToString());
			} else {
				Finish ();
			}
		}

		private void Finish()
		{
			mNextStateHandler.Invoke (FileDownloadStateId.CompareVersion);
		}
	}
}