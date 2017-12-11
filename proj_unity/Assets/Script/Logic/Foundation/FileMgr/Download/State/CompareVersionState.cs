using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

namespace NewFileSystem
{
	public class CompareVersionState : IDownloadState {

		private FileDownloadData mSynchronizeData;
		private Action<FileDownloadStateId> mNextStateHandler;
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
			mFinishHandler = finishHandler;
		}

		public void Update()
		{

		}

		public void Enter ()
		{
			CheckNeedDowload ();
		}

		public void Exit()
		{
			
		}
		#endregion

		private void CheckNeedDowload()
		{
			#if NOT_UPDATE_RESOURCE
			mFinishHandler.Invoke ();
			return;
			#endif
			if (mSynchronizeData.serverVersionInfo.md5 == mSynchronizeData.localVersionInfo.serverMd5)
				mFinishHandler.Invoke ();
			else
				mNextStateHandler.Invoke (FileDownloadStateId.GetServerFileList);
		}
	}
}