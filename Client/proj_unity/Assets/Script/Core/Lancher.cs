using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏的启动器，通过反射来做代码热更新的逻辑
/// </summary>
public class Lancher : MonoBehaviour {

	public static Lancher Instance {
		private set;
		get;
	}

	public GameObject eventSystemObj;
	Text mTipsText;
	Text mDialogText;
	GameObject mDialogObj;
	GameObject mOneBtnGroup;
	GameObject mTwoBtnGroup;
	System.Action mBtnConfirmHdl;
	System.Action mBtnCancelHdl;

	void Awake(){
		mTipsText = transform.Find ("Canvas/txtTips").GetComponent<Text>();

		mDialogObj = transform.Find ("Canvas/Dialog").gameObject;
		mDialogText = transform.Find ("Canvas/Dialog/txtContent").GetComponent<Text> ();
		mOneBtnGroup = transform.Find ("Canvas/Dialog/oneBtnGroup").gameObject;
		mTwoBtnGroup = transform.Find ("Canvas/Dialog/twnBtnGroup").gameObject;
		EventTriggerListener.Get (mOneBtnGroup.transform.Find ("btnConfim").gameObject).onClick = OnBtnConfirmClicked;
		EventTriggerListener.Get (mTwoBtnGroup.transform.Find ("btnConfim").gameObject).onClick = OnBtnConfirmClicked;
		EventTriggerListener.Get (mTwoBtnGroup.transform.Find ("btnCancel").gameObject).onClick = OnBtnCancelClicked;
		mDialogObj.SetActive (false);

		Instance = this;

		GameObject obj = new GameObject ();
		obj.name = "GameMain";
		obj.AddComponent<GameMain> ();
	}

	public void RemoveEventSystem()
	{
		GameObject.DestroyImmediate (eventSystemObj);
	}

	void OnDestroy()
	{
		Instance = null;
	}

	public void RemoveLancher()
	{
		GameObject.Destroy (gameObject);
	}

	public void SetTips(string tips)
	{
		mTipsText.text = tips;
	}

	public void ShowDialog(string content, System.Action hdlOnConfirm, System.Action hdlOnCancel = null)
	{
		mDialogObj.SetActive (true);
		mDialogText.text = content;
		mBtnConfirmHdl = hdlOnConfirm;
		mBtnCancelHdl = hdlOnCancel;
		mOneBtnGroup.SetActive (mBtnCancelHdl == null);
		mTwoBtnGroup.SetActive (mBtnCancelHdl != null);
	}

	void CloseDialog()
	{
		mBtnConfirmHdl = null;
		mBtnCancelHdl = null;
		mDialogObj.SetActive (false);
	}

	void OnBtnConfirmClicked(GameObject go)
	{
		if (null != mBtnConfirmHdl)
			mBtnConfirmHdl.Invoke ();
		CloseDialog ();
	}

	void OnBtnCancelClicked(GameObject go)
	{
		if (null != mBtnCancelHdl)
			mBtnCancelHdl.Invoke ();
		CloseDialog ();
	}
}
