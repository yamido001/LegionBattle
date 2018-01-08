using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;

public class BattleProxy : DataProxy {


	protected override void OnInit ()
	{
		GameMain.Instance.SocketMgr.RegistResponseListener (LegionBattle.ServerClientCommon.RpId.BattleInstruction, OnRpBattleInstruction);
	}

	protected override void OnDestroy ()
	{
		GameMain.Instance.SocketMgr.RemoveResponseListener (LegionBattle.ServerClientCommon.RpId.BattleInstruction, OnRpBattleInstruction);
	}

	#region 向服务器发送数据
	public void SendMoveOperate(short angle)
	{
		Logger.LogInfo ("发送移动角度 " + angle);
		RqBattleInstruction rqInstruction = new RqBattleInstruction ();
		rqInstruction.instruction = new BattleMove (angle);
		GameMain.Instance.SocketMgr.SendMessage (RqId.BattleInstruction, rqInstruction);
	}

	public void StopMoveOperate()
	{
		Logger.LogInfo ("发送停止移动");
		RqBattleInstruction rqInstruction = new RqBattleInstruction ();
		rqInstruction.instruction = new BattleStopMove ();
		GameMain.Instance.SocketMgr.SendMessage (RqId.BattleInstruction, rqInstruction);
	}
	#endregion

	#region 返回数据
	void OnRpBattleInstruction(object param)
	{
		GameBattle.LogicLayer.BattleInstructionManager.Instance.AddInstruction (param as RpBattleInstructionList);
	}
	#endregion
}
