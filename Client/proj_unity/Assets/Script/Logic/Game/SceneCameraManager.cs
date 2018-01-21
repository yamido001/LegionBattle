using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCameraManager : MonoBehaviour{

	public Vector3 PlayerPosOffset;

	Camera mSceneCamera;

	public static SceneCameraManager Instance {
		get;
		private set;
	}

	void Awake()
	{
		Instance = this;
	}

	public void OnEnterScene()
	{
		mSceneCamera = Camera.main;
	}

	public void MoveToPos(Vector3 pos)
	{
		mSceneCamera.transform.position = pos;
	}

	public void ForceTo(Vector3 foucePos)
	{
		Quaternion quaternion = Quaternion.LookRotation (foucePos - mSceneCamera.transform.position);
		mSceneCamera.transform.LookAt (foucePos);
		mSceneCamera.transform.rotation = quaternion;
	}

	public void MoveWithPlayer(Vector3 unitPos)
	{
        MoveToPos(unitPos + new Vector3(0f, 7.2f, -4.75f));
        ForceTo(unitPos);
    }

	void LateUpdate()
	{
		
	}

	void OnDestroy()
	{
		Instance = null;
	}
}
