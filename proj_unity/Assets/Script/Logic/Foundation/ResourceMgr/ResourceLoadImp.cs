using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoadImp : IResourceImp {

	class LoadTask
	{
		string mResourcePath;
		ResourceRequest mLoadRequest;
		List<System.Action<Object>> mHdlSuccessList = new List<System.Action<Object>>();
		List<System.Action<string>> mHdlErrorList = new List<System.Action<string>>();

		public bool IsSameResource(string filePath)
		{
			return mResourcePath == filePath;
		}

		public bool IsFinish
		{
			get{
				if (null == mLoadRequest)
					return false;
				return mLoadRequest.isDone;
			}
		}

		public string LoadFilePath
		{
			get{
				return mResourcePath;
			}
		}

		public void SetResourceRequest(string resourcePath, ResourceRequest request)
		{
			mLoadRequest = request;
			mResourcePath = resourcePath;
		}

		public void AddTaskHdl(System.Action<Object> hdlSuccess, System.Action<string> hdlError)
		{
			mHdlSuccessList.Add (hdlSuccess);
			mHdlErrorList.Add (hdlError);
		}

		public void RunHandler()
		{
			if (mLoadRequest.asset != null) {
				for (int i = 0; i < mHdlSuccessList.Count; ++i) {
					mHdlSuccessList [i].Invoke (mLoadRequest.asset);
				}
			} 
			else {
				for (int i = 0; i < mHdlErrorList.Count; ++i) {
					mHdlErrorList [i].Invoke (string.Empty);
				}
			}
		}

		public void Clear()
		{
			mHdlSuccessList.Clear ();
			mHdlErrorList.Clear ();
			mLoadRequest = null;
			mResourcePath = string.Empty;
		}
	}

	private Queue<LoadTask> mCachedLoadTask = new Queue<LoadTask> ();
	private List<LoadTask> mRemoveList = new List<LoadTask> ();
	private Dictionary<string, LoadTask> mLoadTaskDic = new Dictionary<string, LoadTask>();

	public void Init(System.Action hdlOnFinish)
	{
		hdlOnFinish.Invoke ();
	}

	public void LoadResourceAsync(string filePath, System.Type resourceType, System.Action<Object> hdlSuccess, System.Action<string> hdlError)
	{
		LoadTask task = null;
		if (mLoadTaskDic.ContainsKey (filePath)) {
			task = mLoadTaskDic [filePath];
		} 
		else {
			if (mCachedLoadTask.Count > 0) {
				task = mCachedLoadTask.Dequeue ();
			} 
			else {
				task = new LoadTask ();
			}
			ResourceRequest loadRequest = Resources.LoadAsync (filePath, resourceType);
			task.SetResourceRequest (filePath, loadRequest);
			mLoadTaskDic.Add (filePath, task);
		}
		task.AddTaskHdl (hdlSuccess, hdlError);
	}

	public void Update()
	{
		//提前清理，防止RunHandler内部如果崩溃了，导致mRemoveList不能及时清理掉
		mRemoveList.Clear ();
		var taskDicEnumerator = mLoadTaskDic.GetEnumerator ();
		while (taskDicEnumerator.MoveNext ()) {
			LoadTask loadTask = taskDicEnumerator.Current.Value;
			if (loadTask.IsFinish) {
				mRemoveList.Add (loadTask);
			}
		}
		//之所以不在LoadTaskDic的foreach中处理，是为了防止回调执行后，又会触发LoadResource，导致向mLoadTaskDic中添加数据，从而foreach出现问题
		for (int i = 0; i < mRemoveList.Count; ++i) {
			LoadTask loadTask = mRemoveList [i];
			mLoadTaskDic.Remove (loadTask.LoadFilePath);
			loadTask.RunHandler ();
			loadTask.Clear ();
			mCachedLoadTask.Enqueue (loadTask);
		}
	}

	public void UnloadResource(string filePath)
	{
//		Resources.UnloadAsset (resourceObj);
	}

	public void Destroy()
	{
		Resources.UnloadUnusedAssets ();
	}
}
