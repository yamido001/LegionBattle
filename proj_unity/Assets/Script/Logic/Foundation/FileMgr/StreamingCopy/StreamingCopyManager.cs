using UnityEngine;
using System.Collections;

namespace NewFileSystem
{
	public class StreamingCopyManager : MonoBehaviour {

		private const string CopyVersionCodeKey = "CopyVersionCodeKey";
		private System.Action mFinishHdl;
		private AndroidStreamingCopy mStreamingCopy;

		public void BeginCopy(System.Action hdlOnFinish)
		{
			mFinishHdl = hdlOnFinish;
			if (!IsNeedCopy ()) {
				OnCopyFinish ();
				return;
			}
			mStreamingCopy = gameObject.AddComponent<NewFileSystem.AndroidStreamingCopy>();
			mStreamingCopy.Init (OnCopyFinish);
			mStreamingCopy.BeginCopy ();
		}

		private bool IsNeedCopy()
		{
			bool isPlatformNeed = false;
			#if UNITY_ANDROID && !UNITY_EDITOR
			isPlatformNeed = true;
			#endif
			if (!isPlatformNeed)
				return false;

			string lastVersionCode = PlayerPrefs.GetString (CopyVersionCodeKey);
			if (string.IsNullOrEmpty (lastVersionCode)) {
				return true;
			}
			string version = GameMain.Instance.PlatformMgr.GetAppVersion ();
			if (version != lastVersionCode) {
				return true;
			}
			return false;
		}

		private void OnCopyFinish()
		{
			PlayerPrefs.SetString (CopyVersionCodeKey, GameMain.Instance.PlatformMgr.GetAppVersion ());
			mFinishHdl.Invoke ();
		}
	}
}