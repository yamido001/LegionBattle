using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NewFileSystem;

public class FileManager{

	private FileDownloadManager mDownLoadMgr;
	private StreamingCopyManager mStreamingCopyMgr;
	private Dictionary<string, FileDetailInfo> mPersistentFileListDic;	//Persistent目录下的文件列表
	private Dictionary<string, FileDetailInfo> mStreamingFileListDic;	//Streaming目录下的文件列表

	public void Init()
	{


	}

	public void Update()
	{
		if (null != mDownLoadMgr)
			mDownLoadMgr.Update ();
	}

	public void CleanUp()
	{
		if (null != mDownLoadMgr) {
			mDownLoadMgr.Destroy ();
			mDownLoadMgr = null;
		}
	}

	#region 下载
	public void BeginDownload(ServerFileInfo serverInfo)
	{
		mDownLoadMgr = new NewFileSystem.FileDownloadManager ();
		mDownLoadMgr.Init ();
		mDownLoadMgr.BeginSynchronize (serverInfo, delegate() {
			mPersistentFileListDic = new Dictionary<string, FileDetailInfo>(mDownLoadMgr.GetPersistentFileList ());
			Dictionary<string, FileDetailInfo> streamingFileListDic = mDownLoadMgr.GetStreamingFileList();
			if(null != streamingFileListDic)
			{
				mStreamingFileListDic = new Dictionary<string, FileDetailInfo>(streamingFileListDic);
			}
			mDownLoadMgr.Destroy ();
			mDownLoadMgr = null;
			GameMain.Instance.EventMgr.PostObjectEvent(EventId.DownLoadFinish, null);
		});
	}
	#endregion

	#region StreamingAssets目录拷贝
	public void BeginStreamingCopy()
	{
		Transform copyTf = GameMain.Instance.CreateChildTransform ("StreamingCopy");
		mStreamingCopyMgr = copyTf.gameObject.AddComponent<StreamingCopyManager> ();

		mStreamingCopyMgr.BeginCopy (delegate() {
			GameObject.Destroy(mStreamingCopyMgr.gameObject);
			mStreamingCopyMgr = null;
			GameMain.Instance.EventMgr.PostObjectEvent (EventId.StreamingCopyFinish, null);
		});
	}
	#endregion

	public string ReadFileAsString(string fileName)
	{
		string fileAbsPath = GetFileAbsPath (fileName);
		return GameMain.Instance.FileOperateMgr.ReadAsText(fileAbsPath);
	}

	public byte[] ReadFileAsBytes(string fileName)
	{
		string fileAbsPath = GetFileAbsPath (fileName);
		return GameMain.Instance.FileOperateMgr.ReadAsBinary(fileAbsPath);
	}

	#region 获取文件目录
	//从服务器下载的文件都存在Application.persistentDataPath目录下
	//游戏包中的文件，IOS和PC时是在Application.streamingAssetsPath目录下，但是Android时特殊，因为不能同步访问，所以拷贝到了Application.persistentDataPath目录下

	/// <summary>
	/// 获取文件的绝对路径
	/// </summary>
	/// <returns>The file abs path.</returns>
	/// <param name="fileName">File name.</param>
	private string GetFileAbsPath(string fileName)
	{
		#if UNITY_EDITOR
		//编辑器模式下，为了避免修改完资源后一定要Build资源服务器，使用的是本地的文件
		return GetFileAbsPathPriorityStreaming(fileName);
		#else
		return GetFileAbsPathPriorityServer (fileName);
		#endif
	}

	/// <summary>
	/// 优先使用从服务器下载的文件
	/// Android
	/// </summary>
	/// <returns>The file abs path priority server.</returns>
	/// <param name="fileName">File name.</param>
	public string GetFileAbsPathPriorityServer(string fileName)
	{
		FileDetailInfo fileInfo;
		if (null != mPersistentFileListDic && mPersistentFileListDic.TryGetValue (fileName, out fileInfo)) {
			string relativePath = FileSystemUtils.GetFileRelativePath (fileName, fileInfo.filePath, true);
			return Path.Combine (Application.persistentDataPath, relativePath);

		} else if (null != mStreamingFileListDic && mStreamingFileListDic.TryGetValue (fileName, out fileInfo)) {
			string relativePath = FileSystemUtils.GetFileRelativePath (fileName, fileInfo.filePath, true);
			return Path.Combine (Application.streamingAssetsPath, relativePath);
		}
		return null;
	}
	/// <summary>
	/// 优先使用游戏包中的文件
	/// </summary>
	/// <returns>The file abs path use streaming.</returns>
	/// <param name="fileName">File name.</param>
	private string GetFileAbsPathPriorityStreaming(string fileName)
	{
		FileDetailInfo fileInfo;
		if (null != mStreamingFileListDic && mStreamingFileListDic.TryGetValue (fileName, out fileInfo)) {
			string relativePath = FileSystemUtils.GetFileRelativePath (fileName, fileInfo.filePath, true);
			return Path.Combine (Application.streamingAssetsPath, relativePath);
		}
		else if(mPersistentFileListDic.TryGetValue (fileName, out fileInfo))
		{
			string relativePath = FileSystemUtils.GetFileRelativePath (fileName, fileInfo.filePath, true);
			return Path.Combine (Application.persistentDataPath, relativePath);
		}
		return null;
	}
	#endregion
}