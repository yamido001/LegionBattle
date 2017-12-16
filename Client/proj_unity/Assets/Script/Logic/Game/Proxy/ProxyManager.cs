using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyManager{

	public PlayerProxy Player
	{
		get;
		private set;
	}

	public void Init()
	{
		Player = new PlayerProxy ();
		Player.Init ();
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
	}
}
