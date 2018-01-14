using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LegionBattle.ServerClientCommon;

public class LoginView : UIViewBase {

	Text mTitle;
	Text mTip;
	InputField mInputAccountName;
	InputField mInputPassword;

	Button mBtnLogin;
	Button mBtnRegist;

	public override void OnOpend (object openParam)
	{
		base.OnOpend (openParam);
		RegisterEvent ();

		mTitle = PrefabTf.Find ("textTitle").GetComponent<Text>();
		mTip = PrefabTf.Find ("textTip").GetComponent<Text>();
		mBtnLogin = PrefabTf.Find ("btnLogin").GetComponent<Button> ();
		mBtnRegist = PrefabTf.Find ("btnRegist").GetComponent<Button> ();
		mInputAccountName = PrefabTf.Find ("inputAccountName").GetComponent<InputField> ();
		mInputPassword = PrefabTf.Find ("inputPassword").GetComponent<InputField> ();

		EventTriggerListener.Get (mBtnLogin.gameObject).onClick = OnLoginClicked;
		EventTriggerListener.Get (mBtnRegist.gameObject).onClick = OnRegistClicked;

		mTitle.text = "注册或者登录";
		mTip.text = string.Empty;
	}

	public override void OnClose ()
	{
		base.OnClose ();
	}

	void RegisterEvent()
	{
		AddObjectEventListener (EventId.LoginError, OnLoginError);
		AddObjectEventListener (EventId.CreateAccountError, OnCreateError);
	}

	void OnLoginClicked(GameObject obj)
	{
		mBtnLogin.interactable = false;
		mTip.text = string.Empty;
		GameMain.Instance.ProxyMgr.Login.Login(mInputAccountName.text, mInputPassword.text);
	}

	void OnRegistClicked(GameObject obj)
	{
		mBtnRegist.interactable = false;
		mTip.text = string.Empty;
		GameMain.Instance.ProxyMgr.Login.CreateAccount(mInputAccountName.text, mInputPassword.text);
	}

	void OnLoginError(object parameters)
	{
		RpLoginResult.LoginErrorCode errorCode = (RpLoginResult.LoginErrorCode)parameters;
		switch (errorCode) {
		case RpLoginResult.LoginErrorCode.ParseError:
			mTip.text = "数据长度不合法";
			break;
		case RpLoginResult.LoginErrorCode.PasswordError:
			mTip.text = "账号或者密码错误";
			break;
		}
	}

	void OnCreateError(object parameters)
	{
		RpCreateAccountResult.CreateAccountErrorCode errorCode = (RpCreateAccountResult.CreateAccountErrorCode)parameters;
		switch (errorCode) {
		case RpCreateAccountResult.CreateAccountErrorCode.ParseError:
			mTip.text = "数据长度不合法";
			break;
		case RpCreateAccountResult.CreateAccountErrorCode.AccountExist:
			mTip.text = "账号已经存在";
			break;
		}
	}
}
