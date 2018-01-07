using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIManager{

	public void RegisterUIView()
	{
		RegisterView (UIViewId.MainView, "MainView", typeof(MainView));
		RegisterView (UIViewId.Loading, "LoadingView", typeof(LoadingView));
		RegisterView (UIViewId.Login, "LoginView", typeof(LoginView));
		RegisterView (UIViewId.RoomView, "RoomView", typeof(RoomView));
		RegisterView (UIViewId.MoveJostick, "MoveJostickView", typeof(MoveJostickView));
	}
}
