using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager{

	public class LoadRequestInfo
	{
		public System.Action<Object> hdlSuccess;
		public System.Action<string> hdlError;
		public string resourceName;
		public System.Type resourceType;
		public Object resourceObj;
	}

	public static string ResourceListName = "ResourceList.txt";

	IResourceImp mResourceImp;
	Dictionary<string, Dictionary<string, string>> mResourceConfigDic = new Dictionary<string, Dictionary<string, string>>();
	bool mHasInitFinish = false;
	bool mIsWaitingReloadResource = false;

	public void Init()
	{
		mResourceImp = new ResourceLoadImp ();
		mResourceImp.Init (delegate() {
			if (Lancher.Instance != null)
				Lancher.Instance.SetTips ("ResourceManager mResourceImp.Init");
			LoadResourceList(delegate() {
				mHasInitFinish = true;
				if (Lancher.Instance != null)
					Lancher.Instance.SetTips ("ResourceManager load resourcelist finish");
				GameMain.Instance.EventMgr.PostObjectEvent(EventId.ResourceManagerInitFinish, null);
			});
		});
	}

	/// <summary>
	/// 资源更新后，需要调用
	/// </summary>
	public void OnResourceUpdate()
	{
		mIsWaitingReloadResource = true;
		LoadResourceList (delegate() {
			mIsWaitingReloadResource = false;
			GameMain.Instance.EventMgr.PostObjectEvent(EventId.ResourceManagerReUpdateFinish, null);
		});
	}

	public void LoadResourceAsync(object user, string resourceName, System.Type resourceType, System.Action<Object> hdlSuccess, System.Action<string> hdlError)
	{
		if (!CheckCanLoad()) {
			return;
		}
		string resourcePath = GetResourcePath (resourceName, resourceType);
		if (string.IsNullOrEmpty (resourcePath)) {
			hdlError.Invoke ("Not find resource name: " + resourceName);
			return;
		}
		mResourceImp.LoadResourceAsync (resourcePath, resourceType, delegate(Object obj) {
			if(!user.Equals(null))
			{
				hdlSuccess.Invoke(obj);	
			}
			else
			{
				obj = null;
				UnloadResource(resourcePath);
			}
		}, delegate(string errorCode) {
			if(!user.Equals(null))
			{
				hdlError.Invoke(errorCode);
			}
		}); 
	}

	public void LoadResourceAsync(object user, LoadRequestInfo[] requests, System.Action hdlSuccess, System.Action<string> hdlError)
	{
		if (!CheckCanLoad()) {
			return;
		}
		string isError = string.Empty;
		List<LoadRequestInfo> loadedRequests = new List<LoadRequestInfo> ();

		System.Action unloadFinishLoad = () => {
			for(int j = 0; j < loadedRequests.Count; ++j)
			{
				LoadRequestInfo loadedRequest = loadedRequests[j];
				UnloadResource(GetResourcePath(loadedRequest.resourceName, loadedRequest.resourceType));
			}
			loadedRequests.Clear();
		};

		for (int i = 0; i < requests.Length; ++i) {
			LoadRequestInfo loadRequest = requests [i];
			string resourcePaht = GetResourcePath (loadRequest.resourceName, loadRequest.resourceType);
			LoadResourceAsync (user, resourcePaht, loadRequest.resourceType, delegate(Object obj) {

				if(user.Equals(null) || !string.IsNullOrEmpty(isError))
				{
					obj = null;
					UnloadResource(resourcePaht);
					unloadFinishLoad();
				}
				else
				{
					loadRequest.resourceObj = obj;
					loadedRequests.Add(loadRequest);
					if(loadedRequests.Count >= requests.Length)
					{
						hdlSuccess.Invoke();
					}
				}

			}, delegate(string errorCode) {
				unloadFinishLoad();
				if(string.IsNullOrEmpty(isError) && !user.Equals(null)){
					hdlError.Invoke(errorCode);
				}
				isError = true.ToString();
			});
		}
	}

	public void Update()
	{
		mResourceImp.Update();
	}

	private void UnloadResource(string resourcePaht)
	{
		mResourceImp.UnloadResource (resourcePaht);
	}

	public void UnloadResource(string resourceName, System.Type resourceType)
	{
		if (!CheckCanLoad()) {
			return;
		}
		string resourcePath = GetResourcePath (resourceName, resourceType);
		UnloadResource (resourcePath);
	}

	public void Destroy()
	{
		mResourceImp.Destroy ();
	}

	private string GetResourcePath(string resourceName, System.Type resourceType)
	{
		string resTypeStr = resourceType.ToString ();
		if (!mResourceConfigDic.ContainsKey (resTypeStr)) {
			return string.Empty;
		}
		Dictionary<string, string> resourceNameToPathDic = mResourceConfigDic [resTypeStr];
		string resourcePath = string.Empty;
		resourceNameToPathDic.TryGetValue (resourceName, out resourcePath);
		return resourcePath;
	}

	Dictionary<System.Type, string> mResTypeToStringDic = new Dictionary<System.Type, string>();
	private string ResourceTypeToString(System.Type resourceType)
	{
		string ret = string.Empty;
		if (!mResTypeToStringDic.TryGetValue (resourceType, out ret)) {
			ret = resourceType.ToString ();
			mResTypeToStringDic [resourceType] = ret;
		}
		return ret;
	}

	private bool CheckCanLoad()
	{
		if (!mHasInitFinish) {
			Logger.LogError ("ResourceManager has not init finish");
			return false;
		}
		if (mIsWaitingReloadResource) {
			Logger.LogError ("ResourceManager wait resource reupdate");
			return false;
		}
		return true;
	}

	private void LoadResourceList(System.Action hdlOnFinish)
	{
		mResourceImp.LoadResourceAsync ("ResourceList", typeof(TextAsset), delegate(Object obj) {
			TextAsset resourceAsset = obj as TextAsset;
			string fileContent = resourceAsset.text;
			try{
				mResourceConfigDic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(fileContent);
				hdlOnFinish.Invoke();
			}
			catch(System.Exception ex){
				if (Lancher.Instance != null)
					Lancher.Instance.SetTips ("LoadResourceList error " + ex.Message + "\n" );
			}

		}, delegate(string errorCode) {
			Logger.LogError("Resource manager load resource list error:" + errorCode);
		});
	}
}
