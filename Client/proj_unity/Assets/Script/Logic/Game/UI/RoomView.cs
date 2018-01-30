using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LBCSCommon;

public class RoomView : UIViewBase {

	Text mTitle;
	Text mTip;

	InputField mInputRoomName;
	InputField mInputRoomId;

	Button mBtnCreateRoom;
	Button mBtnEnterRoom;
	Button mBtnLeaveRoom;
	Button mBtnReady;

	GameObject mAccountItemTemplate;
	GridLayoutGroup mAccountGrid;
	List<RoomViewAccountListItem> mAccountList = new List<RoomViewAccountListItem>();

	public override void OnOpend (object openParam)
	{
		base.OnOpend (openParam);
		RegisterEvent ();

		mTitle = PrefabTf.Find ("textTitle").GetComponent<Text>();
		mTip = PrefabTf.Find ("textTip").GetComponent<Text>();
		mBtnCreateRoom = PrefabTf.Find ("btnCreate").GetComponent<Button> ();
		mBtnEnterRoom = PrefabTf.Find ("btnEnter").GetComponent<Button> ();
		mBtnLeaveRoom = PrefabTf.Find ("btnLeave").GetComponent<Button> ();
		mBtnReady = PrefabTf.Find ("btnReady").GetComponent<Button> ();
		mInputRoomName = PrefabTf.Find ("inputRoomName").GetComponent<InputField> ();
		mInputRoomId = PrefabTf.Find ("inputRoomId").GetComponent<InputField> ();
		mAccountGrid = PrefabTf.Find ("accountGrid").GetComponent<GridLayoutGroup>();
		mAccountItemTemplate = PrefabTf.Find ("accountItemTemplate").gameObject;
		mAccountItemTemplate.SetActive (false);

		EventTriggerListener.Get (mBtnCreateRoom.gameObject).onClick = OnCreateClicket;
		EventTriggerListener.Get (mBtnEnterRoom.gameObject).onClick = OnEnterClicked;
		EventTriggerListener.Get (mBtnLeaveRoom.gameObject).onClick = OnLeaveClicked;
		EventTriggerListener.Get (mBtnReady.gameObject).onClick = OnReadyClicked;

		mTitle.text = "注册或者登录";
		mTip.text = string.Empty;
		RefreshView ();
	}

	public override void OnClose ()
	{
		base.OnClose ();
	}

	void RegisterEvent()
	{
		AddObjectEventListener (EventId.CreateRoomError, OnCreateError);
		AddObjectEventListener (EventId.CreateRoomSuccess, OnCreateSuccess);
		AddObjectEventListener (EventId.EnterRoomError, OnEnterError);
		AddObjectEventListener (EventId.EnterRoomSuccess, OnEnterSuccess);
		AddObjectEventListener (EventId.LeaveRoomSuccess, OnLeaveSuccess);
		AddObjectEventListener (EventId.LeaveRoomError, OnLeaveError);
		AddObjectEventListener (EventId.RoomMemberInfoRefresh, OnRefreshMemberInfo);
	}

	void OnCreateClicket(GameObject obj)
	{
		mBtnCreateRoom.interactable = false;
		mTip.text = string.Empty;
		GameMain.Instance.ProxyMgr.Room.CreateRoom(mInputRoomName.text);
	}

	void OnEnterClicked(GameObject obj)
	{
		mBtnEnterRoom.interactable = false;
		mTip.text = string.Empty;
		int roomId;
		if (!int.TryParse (mInputRoomId.text, out roomId))
			return;
		GameMain.Instance.ProxyMgr.Room.EnterRoom(roomId);
	}

	void OnLeaveClicked(GameObject obj)
	{
		mBtnLeaveRoom.interactable = false;
		mTip.text = string.Empty;
		GameMain.Instance.ProxyMgr.Room.LeaveRoom();
	}

	void OnReadyClicked(GameObject obj)
	{
		GameMain.Instance.ProxyMgr.Room.ReadyInRoom ();
	}

	void RefreshView()
	{
		bool isCurInRoom = GameMain.Instance.ProxyMgr.Room.IsInRoom;
		mBtnLeaveRoom.gameObject.SetActive (isCurInRoom);
		mBtnCreateRoom.gameObject.SetActive (!isCurInRoom);
		mBtnEnterRoom.gameObject.SetActive (!isCurInRoom);
		mBtnReady.gameObject.SetActive (isCurInRoom);
		mInputRoomId.gameObject.SetActive (!isCurInRoom);
		mInputRoomName.gameObject.SetActive (!isCurInRoom);
		mAccountGrid.gameObject.SetActive (isCurInRoom);
	}

	void OnCreateSuccess(object parameters)
	{
		mTip.text = "创建房间成功，房间号: " + parameters.ToString();
		RefreshView ();
	}

	void OnLeaveSuccess(object parameters)
	{
		mTip.text = "离开房间成功";
		RefreshView ();
	}

	void OnCreateError(object parameters)
	{
		mTip.text = "创建房间失败";
		RefreshView ();
	}

	void OnEnterError(object parameters)
	{
		mTip.text = "进入房间失败";
		RefreshView ();
	}

	void OnRefreshMemberInfo(object parameters)
	{
		if (null == GameMain.Instance.ProxyMgr.Room.RoomAccountList)
			return;
		if (mAccountList.Count < GameMain.Instance.ProxyMgr.Room.RoomAccountList.Count) {
			int createCount = GameMain.Instance.ProxyMgr.Room.RoomAccountList.Count - mAccountList.Count;
			for (int i = 0; i < createCount; ++i) {
				RoomViewAccountListItem accountListItem = new RoomViewAccountListItem ();
				GameObject itemObj = GameObject.Instantiate (mAccountItemTemplate);
				itemObj.transform.SetParent (mAccountGrid.transform);
				accountListItem.Init (itemObj);
				mAccountList.Add (accountListItem);
			}
		} else {
			for (int i = mAccountList.Count - 1; i >= GameMain.Instance.ProxyMgr.Room.RoomAccountList.Count; --i) {
				mAccountList [i].SetShowStatus (false);
			}
		}
		for (int i = 0; i < GameMain.Instance.ProxyMgr.Room.RoomAccountList.Count; ++i) {
			mAccountList [i].SetShowStatus (true);
			mAccountList [i].SetPlayerData (GameMain.Instance.ProxyMgr.Room.RoomAccountList [i]);
		}
	}

	void OnEnterSuccess(object parameters)
	{
		mTip.text = "进入房间成功";
		RefreshView ();
	}

	void OnLeaveError(object parameters)
	{
		mTip.text = "离开房间失败";
		RefreshView ();
	}
}
