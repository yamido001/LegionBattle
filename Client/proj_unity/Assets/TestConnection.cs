using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;

public class TestConnection : MonoBehaviour, IPhotonPeerListener {

	PhotonPeer mPeer;

	// Use this for initialization
	void Start () {
		mPeer = new PhotonPeer (this, ConnectionProtocol.Udp);
		mPeer.Connect ("localhost:5055", "LegionBattle");
	}
	
	// Update is called once per frame
	void Update () {
		mPeer.Service ();
	}

	void OnDestroy()
	{
		mPeer.Disconnect ();
	}

	public void DebugReturn(DebugLevel level, string message)
	{
		
	}

	public void OnStatusChanged(StatusCode code)
	{
		
	}

	public void OnOperationResponse(OperationResponse op)
	{
		
	}

	public void OnEvent(EventData data)
	{
	}
}
