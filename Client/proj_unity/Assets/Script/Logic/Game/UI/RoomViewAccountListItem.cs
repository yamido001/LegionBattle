using UnityEngine;
using UnityEngine.UI;
using LegionBattle.ServerClientCommon;

public class RoomViewAccountListItem{

	Text mAccountName;
	Text mAccountId;
	Text mIsReady;

	GameObject mObj;
	Transform mRootTf;

	public void Init(GameObject obj)
	{
		mObj = obj;
		mRootTf = obj.transform;
		mAccountName = mRootTf.Find ("textAccountName").GetComponent<Text> ();
		mAccountId = mRootTf.Find ("textAccountId").GetComponent<Text> ();
		mIsReady = mRootTf.Find ("textReady").GetComponent<Text> ();
	}

	public void SetAccountData(RpAccountData accountData)
	{
		mAccountId.text = accountData.AccountId.ToString ();
		mAccountName.text = accountData.AccountName;
		mIsReady.gameObject.SetActive (accountData.IsReady);
	}

	public void SetShowStatus(bool isShow)
	{
		mObj.SetActive (isShow);
	}
}
