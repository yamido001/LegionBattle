using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DownLoadInfo
{
	public string url;
	public System.Action<byte[]> hdlOnSuccess;
	public System.Action<string> hdlOnError;
	public System.Action<ulong> hdlProcess;
	public int taskId;
}


public class HttpLoaderTask : MonoBehaviour {

	public DownLoadInfo taskInfo
	{
		get;
		private set;
	}

	public bool IsFinish {
		get;
		private set;
	}

	public string Error {
		get;
		private set;
	}

	public byte[] Data {
		get;
		private set;
	}

	public ulong DownloadedBytes {
		get {
			if (null == mRequest)
				return 0;
			return mRequest.downloadedBytes;
		}
	}

	UnityWebRequest mRequest;

	public void BeginDownLoad(DownLoadInfo taskInfo)
	{
		this.taskInfo = taskInfo;
		StartCoroutine (DownLoad());
	}

	public void Clear()
	{
		taskInfo = null;
		IsFinish = false;
		Error = null;
		Data = null;
		mRequest = null;
		StopCoroutine (DownLoad());
	}

	IEnumerator DownLoad()
	{
		mRequest = UnityWebRequest.Get(taskInfo.url);
		yield return mRequest.Send ();

		IsFinish = true;
		if (mRequest.isError) {
			Error = mRequest.error;
		} 
		else {
			Data = mRequest.downloadHandler.data;
		}
	}
}
