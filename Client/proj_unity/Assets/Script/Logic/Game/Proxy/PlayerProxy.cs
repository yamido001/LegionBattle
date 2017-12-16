using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoStruct;

public class PlayerProxy : DataProxy {
	Dictionary<int, PlayerSceneInfo> mScenePlayerDic = new Dictionary<int, PlayerSceneInfo>();
	PlayerSceneInfo mMainPlayerInfo;

	public PlayerSceneInfo MainPlayer {
		get;
		private set;
	}

	Dictionary<uint, string> mCachedModel = new Dictionary<uint, string>();
		
	protected override void OnInit ()
	{
		MainPlayer = new PlayerSceneInfo ();
		MainPlayer.name = "杨明东";
		MainPlayer.model = 1;
		MainPlayer.posX = 100000;
		MainPlayer.posY = 100000;
		MainPlayer.attributes = new int[(int)AttributeType.Count];
		MainPlayer.attributes[(int)AttributeType.Life] = 100;
		MainPlayer.attributes[(int)AttributeType.MoveSpeed] = 500;
	}

	protected override void OnDestroy ()
	{
		
	}

	public string GetModelStrByUint(uint modelIndex)
	{
		string ret = null;
		if (mCachedModel.TryGetValue (modelIndex, out ret)) {
			return ret;
		}
		ret = "PlayerModel" + modelIndex;
		mCachedModel [modelIndex] = ret;
		return ret;
	}
}
