using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

namespace NewFileSystem
{
	public class DownLoadFileState : IDownloadState {

		private FileDownloadData mSynchronizeData;
		private Action<FileDownloadStateId> mNextHandler;
		private Action<FileErrorCode, string> mErrorHandler;

		private ulong mTotalDownloadSize = 0;
		private ulong mFinishFilesSize = 0;
		private ulong mCurFileSize = 0;
		private ulong mLastShowSize = 0;
		private int mDownloadId = int.MinValue;

		public FileDownloadStateId StateId
		{
			get{ return FileDownloadStateId.DownLoadFile; }
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
			GeneralDownloadSize ();
			if (CheckWifi ()) {
				DownLoadFile ();
			} 
			else {
				Lancher.Instance.ShowDialog ("需要下载" + Utils.GetStrFileSize(mTotalDownloadSize) + " 资源，当前wifi关闭，是否继续下载", delegate() {
					DownLoadFile ();
				}, delegate() {
					GameMain.Instance.QuitGame();
				});
			}
		}

		public void Exit()
		{
			GameMain.Instance.HttpMgr.RemoveDownload (mDownloadId);
		}

		private void GeneralDownloadSize()
		{
			mTotalDownloadSize = 0;
			var enumerator = mSynchronizeData.needDownloadSet.GetEnumerator ();
			while (enumerator.MoveNext ()) {
				mTotalDownloadSize += (ulong)enumerator.Current.fileLength;
			}
		}

		private bool CheckWifi()
		{
			if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
				return true;
			return false;
		}

		private void ShowTipInfo()
		{
			if (mLastShowSize < mFinishFilesSize + mCurFileSize)
				mLastShowSize = mFinishFilesSize + mCurFileSize;

			string curSize = Utils.GetStrFileSize(mLastShowSize);
			string totalSize = Utils.GetStrFileSize(mTotalDownloadSize);
			float percentage = (float)mLastShowSize / mTotalDownloadSize;
			Lancher.Instance.SetTips ("下载中，" + curSize + "/" + totalSize + "(" + ((int)(percentage * 100f)).ToString () + "%)");
		}

		private void DownLoadFile()
		{
			if (mSynchronizeData.needDownloadSet.Count == 0) {
				mNextHandler.Invoke (FileDownloadStateId.WritePersistentVersionFile);
				return;
			}
			ShowTipInfo ();
			FileDetailInfo downloadFileInfo = mSynchronizeData.needDownloadSet [mSynchronizeData.needDownloadSet.Count - 1];
			mSynchronizeData.needDownloadSet.RemoveAt (mSynchronizeData.needDownloadSet.Count - 1);

			string fileRelativePath = null;
			if (string.IsNullOrEmpty (downloadFileInfo.filePath)) {
				//服务器中是以MD5命名的。
				fileRelativePath = downloadFileInfo.fileMd5;
			} 
			else {
				fileRelativePath = Path.Combine (downloadFileInfo.filePath, downloadFileInfo.fileMd5);
			}
			string fileUrl = Path.Combine (mSynchronizeData.ServerDataPath, fileRelativePath);
			mDownloadId = GameMain.Instance.HttpMgr.DownLoad (fileUrl, delegate(byte[] fileBytes) {

				Logger.LogInfo("NewDownload download file success,name:" + downloadFileInfo.fileName + " url:" + fileUrl);
				if(downloadFileInfo.fileLength != fileBytes.Length)
				{
					mErrorHandler.Invoke(FileErrorCode.FileLengthError, fileUrl + " serverL:" + downloadFileInfo.fileLength + "  realL:" + fileBytes.Length);
				}
				else
				{
					string md5 = Utils.MD5(fileBytes);
					if(md5 != downloadFileInfo.fileMd5)
					{
						mErrorHandler.Invoke(FileErrorCode.FileMd5Error, fileUrl + " serverMd5:" + downloadFileInfo.fileMd5 + "  realMd5:" + md5);
					}
					else
					{
						string errorInfo;
						FileErrorCode errorCode = FileOperateUtils.TryFileWrite(delegate(){
							string relativePath = FileSystemUtils.GetFileRelativePath(downloadFileInfo.fileName, downloadFileInfo.filePath, true);
							GameMain.Instance.FileOperateMgr.CreateDirIfNotExist(Path.GetDirectoryName(relativePath));
							GameMain.Instance.FileOperateMgr.WriteBinaryFile(relativePath, fileBytes);

						}, out errorInfo);
						if(FileErrorCode.Null != errorCode)
						{
							mErrorHandler.Invoke(errorCode, errorInfo);
						}
						else
						{
							mSynchronizeData.persistentFileListDic.Add(downloadFileInfo.fileName, downloadFileInfo);
							errorCode = FileOperateUtils.TryFileWrite(delegate(){
								string detailStr = FileListUtils.DetailInfoToString(downloadFileInfo);
								string relativePath = FileSystemUtils.GetFileRelativePath(FileDownloadData.FileListFileName, string.Empty);
								GameMain.Instance.FileOperateMgr.WriteTextFile(relativePath, detailStr, true);
							}, out errorInfo);
							if(FileErrorCode.Null != errorCode)
							{
								mErrorHandler.Invoke(errorCode, errorInfo);
							}
							else
							{
								mFinishFilesSize += (ulong)downloadFileInfo.fileLength;
								mCurFileSize = 0;
								DownLoadFile();
							}
						}
					}
				}
			}, delegate(string errorCode) {
				Logger.LogError("NewDownload download file failed,name:" + downloadFileInfo.fileName + " url:" + fileUrl);
				mErrorHandler.Invoke(FileErrorCode.DownloadFileError, fileUrl);
			}, delegate(ulong curDownloadedBytes) {
				mCurFileSize = curDownloadedBytes;
				ShowTipInfo();
			});
		}
	}
}