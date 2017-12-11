using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HttpManager{

	GameObject mGameObject;
	List<DownLoadInfo> mWaitTaskList = new List<DownLoadInfo>();
	List<HttpLoaderTask> mCurDownLoadTaskList = new List<HttpLoaderTask>();
	int mCurMaxId = 0;

	public void Init()
	{
		mGameObject = GameMain.Instance.CreateChildTransform ("HttpDownloader").gameObject;
	}

	public int DownLoad(string url, System.Action<byte[]> hdlOnSuccess, System.Action<string> hdlOnError, System.Action<ulong> hdlProcess = null)
	{
		Logger.LogInfo ("DownLoad " + url);
		DownLoadInfo downloadInfo = CreateDownloadInfo (url, hdlOnSuccess, hdlOnError, hdlProcess);
		if (mCurDownLoadTaskList.Count > ConstConfig.MaxHttpTask) {
			mWaitTaskList.Add (downloadInfo);
		} 
		else {
			HttpLoaderTask downloader = CreateDownloadTask ();
			mCurDownLoadTaskList.Add (downloader);
			downloader.BeginDownLoad (downloadInfo);
		}
		return downloadInfo.taskId;
	}

	public void RemoveDownload(int downloadId)
	{
		
	}

	public void Update()
	{
		for (int i = mCurDownLoadTaskList.Count - 1; i >= 0 ; --i) {
			HttpLoaderTask task = mCurDownLoadTaskList [i];
			if (task.IsFinish) {
				if (null == task.Data) {
					Logger.LogError (task.Error);
					task.taskInfo.hdlOnError.Invoke (task.Error);
				} else {
					task.taskInfo.hdlOnSuccess.Invoke (task.Data);
				}
				ReturnDownloadInfo (task.taskInfo);
				task.Clear ();
				ReturnDownloadTask (task);
				mCurDownLoadTaskList.RemoveAt (i);
			} else {
				if (task.taskInfo.hdlProcess != null) {
					task.taskInfo.hdlProcess.Invoke (task.DownloadedBytes);
				}
			}
		}

		if (mCurDownLoadTaskList.Count <= ConstConfig.MaxHttpTask && mWaitTaskList.Count > 0) {
			HttpLoaderTask downloader = CreateDownloadTask ();
			downloader.BeginDownLoad (mWaitTaskList[0]);
			mCurDownLoadTaskList.Add (downloader);
			mWaitTaskList.RemoveAt (0);
		}
	}

	public void Destroy()
	{
		for (int i = 0; i < mWaitTaskList.Count; ++i) {
			ReturnDownloadInfo(mWaitTaskList[i]);
		}
		mWaitTaskList.Clear ();

		for (int i = 0; i < mCurDownLoadTaskList.Count; ++i) {
			ReturnDownloadInfo (mCurDownLoadTaskList [i].taskInfo);
			mCurDownLoadTaskList [i].Clear ();
			ReturnDownloadTask (mCurDownLoadTaskList [i]);
		}
		mCurDownLoadTaskList.Clear ();
	}

	#region 减少TaskInfo的GC
	Queue<DownLoadInfo> mCachedTaskInfoQueue = new Queue<DownLoadInfo>();
	DownLoadInfo CreateDownloadInfo(string url, System.Action<byte[]> hdlOnSuccess, System.Action<string> hdlOnError, System.Action<ulong> hdlProcess)
	{
		DownLoadInfo ret = null;
		if (mCachedTaskInfoQueue.Count > 0) {
			ret = mCachedTaskInfoQueue.Dequeue ();
		} else {
			ret = new DownLoadInfo ();
		}
		ret.url = url;
		ret.hdlOnSuccess = hdlOnSuccess;
		ret.hdlOnError = hdlOnError;
		ret.hdlProcess = hdlProcess;
		ret.taskId = ++mCurMaxId;
		return ret;
	}
	void ReturnDownloadInfo(DownLoadInfo task)
	{
		mCachedTaskInfoQueue.Enqueue (task);	
	}
	#endregion

	#region 减少DownloadTask的GC
	Queue<HttpLoaderTask> mCachedLoadTask = new Queue<HttpLoaderTask>();
	HttpLoaderTask CreateDownloadTask()
	{
		HttpLoaderTask ret = null;
		if (mCachedLoadTask.Count > 0) {
			ret = mCachedLoadTask.Dequeue ();
			ret.enabled = true;
		} 
		else {
			ret = mGameObject.AddComponent<HttpLoaderTask> ();
		}
		return ret;
	}
	void ReturnDownloadTask(HttpLoaderTask task)
	{
		task.enabled = false;
		mCachedLoadTask.Enqueue (task);
	}
	#endregion
//	IEnumerator GetText() {  
//		UnityWebRequest request = UnityWebRequest.Get("http://example.com");  
//		//   
//		// UnityWebRequest request = new UnityWebRequest("http://example.com");  
//		//   
//		// request.method = UnityWebRequest.kHttpVerbGET;  
//
//		//   
//		yield return request.Send();  
//
//		//   
//		if (request.isError) {  
//			Debug.Log(request.error);  
//		} else {  
//			if (request.responseCode == 200) {  
//				//   
//				string text = request.downloadHandler.text;  
//
//				//   
//				byte [] results = request.downloadHandler.data;  
//			}  
//		}  
//	}  
}
