using UnityEngine;
using System.Collections.Generic;

namespace NewFileSystem
{
	public class ServerFileInfo
	{
		public string md5;
		public string path;
		public string fileListName;
	}
	
	public class FileDownloadManager {

		private FileDownloadData mData;
		private System.Action mFinishHdl;
		private int mHttpDownloadId = int.MinValue;

		#region 对外接口
		public void Init()
		{
			mData = new FileDownloadData ();
			InitStateMachine ();
		}

		public void BeginSynchronize(ServerFileInfo serverInfo, System.Action hdlFinish)
		{
			mFinishHdl = hdlFinish;
			SetServerInfo (serverInfo);
		}

		public void Update()
		{
			StateMachineUpdate ();
		}

		public void Destroy()
		{
			mData.Clear ();
			StateMachineClean ();
			GameMain.Instance.HttpMgr.RemoveDownload (mHttpDownloadId);
		}

		public Dictionary<string, FileDetailInfo> GetPersistentFileList()
		{
			return mData.persistentFileListDic;
		}

		public Dictionary<string, FileDetailInfo> GetStreamingFileList()
		{
			return mData.streamingFileListDic;
		}
		#endregion

		#region 状态机
		private Dictionary<int, System.Type> mAllState = new Dictionary<int, System.Type>();
		private IDownloadState mCurState = null;

		private void InitStateMachine()
		{
			RegistState (FileDownloadStateId.CheckPersistentInfo, typeof(CheckPersistentInfoState));
			RegistState (FileDownloadStateId.CompareVersion, typeof(CompareVersionState));
			RegistState (FileDownloadStateId.LoadStreamingFileList, typeof(LoadStreamingFileList));
			RegistState (FileDownloadStateId.GetServerFileList, typeof(GetServerFileListState));
			RegistState (FileDownloadStateId.CompareFileList, typeof(CompareFileListState));
			RegistState (FileDownloadStateId.DownLoadFile, typeof(DownLoadFileState));
			RegistState (FileDownloadStateId.WritePersistentVersionFile, typeof(WritePersistentVersionFileState));
		}

		private void RegistState(FileDownloadStateId stateId, System.Type type)
		{
			mAllState.Add ((int)stateId, type);
		}

		private void StateMachineUpdate()
		{
			if (null == mCurState)
				return;
			mCurState.Update ();
		}

		private void StateMachineClean()
		{
			if (null != mCurState) {
				mCurState.Exit ();
				mCurState = null;
			}
		}

		private void EnterState(FileDownloadStateId stateId) 
		{
			Logger.LogInfo("下载模块进入状态：" + stateId.ToString());
			if (null != mCurState)
				mCurState.Exit ();
			System.Type newStateType = mAllState [(int)stateId];
			mCurState = System.Activator.CreateInstance(newStateType) as IDownloadState;
			mCurState.Init (mData, EnterState, OnFinish, OnError);
			mCurState.Enter ();
		}

		private void OnFinish()
		{
			Logger.LogInfo("下载完成");
			mFinishHdl.Invoke ();
		}

		private void OnError(FileErrorCode error, string param)
		{
			Logger.LogError ("Download onError  " + error.ToString() + "  "+ param);
			switch (error) {
			case FileErrorCode.DownLoadFileListError:
			case FileErrorCode.ParseFileListError:
			case FileErrorCode.DownloadFileError:
			case FileErrorCode.FileMd5Error:
			case FileErrorCode.FileLengthError:
			case FileErrorCode.Unknown:
				Lancher.Instance.ShowDialog ("下载失败，错误码:" + error.ToString (), delegate() {
					EnterState(FileDownloadStateId.GetServerFileList);
				});
				break;
			case FileErrorCode.NoSpace:
			case FileErrorCode.WriteFileNoPermission:
				break;
			default:
				break;
			}
		}
		#endregion

		#region 设置URL等信息
		private void SetServerInfo(ServerFileInfo serverInfo)
		{
			mData.serverVersionInfo = new ServerVersionInfo ();

			#if LOCAL_RES_SERVER
			RequestLocalResServerInfo();
			#else
			mData.serverVersionInfo.md5 = serverInfo.md5;
			mData.serverRootPath = serverInfo.path;
			mData.serverFileListName = serverInfo.fileListName;
//			mData.serverIp = App.ProxyMgr.ApplicationProxy.ResourceServerIp;
			EnterState (FileDownloadStateId.CheckPersistentInfo);
			#endif
		}

		private void RequestLocalResServerInfo()
		{
			mData.serverRootPath = "LegionBattle";
			mData.serverFileListName = "FileList.txt";
			mData.serverIp = ConstConfig.LocalResourceUrl;

			#if UNITY_ANDROID && !UNITY_EDITOR
			mData.serverRootPath += "/Android";
			#elif UNITY_IOS && !UNITY_EDITOR
			mData.serverRootPath += "/iOS";
			#else
			mData.serverRootPath += "/Editor";
			#endif

			mHttpDownloadId = GameMain.Instance.HttpMgr.DownLoad (mData.ServerVersionFilePath, delegate(byte[] data) {
				mData.serverVersionInfo.md5 = System.Text.Encoding.UTF8.GetString (data);
				EnterState (FileDownloadStateId.CheckPersistentInfo);
			}, delegate(string error) {
				Lancher.Instance.ShowDialog ("访问自定义资源服务器(" + mData.ServerVersionFilePath + ")失败:" + error.ToString (), delegate() {
					RequestLocalResServerInfo();
				});
			});
		}
		#endregion
	}
}

