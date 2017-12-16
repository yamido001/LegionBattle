using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理Unity的scene
/// </summary>
public class UnitySceneManager{

	UnityScene mCurScene = null;
	UnityScene mNextScene = null;

	public void Init()
	{
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void EnterScene(string sceneId, object param)
	{
		if (null != mCurScene) {
			mCurScene.OnWillExited ();
		}
		mNextScene = new UnityScene (sceneId);
		mNextScene.OnWillEntered ();
		UnityEngine.SceneManagement.SceneManager.LoadScene (sceneId.ToString (), UnityEngine.SceneManagement.LoadSceneMode.Single);
	}

	void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadMode)
	{
		if (null != mCurScene) {
			mCurScene.OnExited ();
		}
		if (null != mNextScene) {
			mNextScene.OnEntered ();
		}
		mCurScene = mNextScene;
		mNextScene = null;
	}

	public void Destroy()
	{
		UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
