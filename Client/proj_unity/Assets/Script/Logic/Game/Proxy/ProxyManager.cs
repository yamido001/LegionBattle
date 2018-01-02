using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyManager{

	public PlayerProxy Player
	{
		get;
		private set;
	}

	public LoginProxy Login {
		get;
		private set;
	}

	public RoomProxy Room {
		get;
		private set;
	}

	public void Init()
	{
		Player = new PlayerProxy ();
		Player.Init ();

		Login = new LoginProxy ();
		Login.Init ();
	
		Room = new RoomProxy ();
		Room.Init ();
	}

	void DestroyProxy(DataProxy proxy)
	{
		if (proxy != null) {
			proxy.Destroy ();
		}
	}

	public void Destroy()
	{
		DestroyProxy (Player);
		DestroyProxy (Login);
		DestroyProxy (Room);
	}
}
