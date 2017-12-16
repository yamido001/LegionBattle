using UnityEngine;
using System.Collections;
using System;

namespace NewFileSystem
{
	public interface IDownloadState{
		FileDownloadStateId StateId
		{
			get;
		}
		void Init (FileDownloadData synchronizerData, Action<FileDownloadStateId> hdlNextState, Action hdlFinish, Action<FileErrorCode, string> hdlError);
		void Update();
		void Enter ();
		void Exit();
	}
}

