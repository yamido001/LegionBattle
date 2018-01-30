using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LBCSCommon;

public class BattleProxy : DataProxy {


	protected override void OnInit ()
	{
		GameMain.Instance.SocketMgr.RegistResponseListener (LBCSCommon.RpId.BattleInstruction, OnRpBattleInstruction);
	}

	protected override void OnDestroy ()
	{
		GameMain.Instance.SocketMgr.RemoveResponseListener (LBCSCommon.RpId.BattleInstruction, OnRpBattleInstruction);
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

    public void SendAreaUseSkillOperate(short skillId, short angle, int param1)
    {
        RqBattleInstruction rqInstruction = new RqBattleInstruction();
        rqInstruction.instruction = new BattleAreaTargetSkill(skillId, angle, param1);
        GameMain.Instance.SocketMgr.SendMessage(RqId.BattleInstruction, rqInstruction);
    }

    public void SendNoTargetSkillUseOperate(short skillId)
    {
        RqBattleInstruction rqInstruction = new RqBattleInstruction();
        rqInstruction.instruction = new BattleNoTargetSkill(skillId);
        GameMain.Instance.SocketMgr.SendMessage(RqId.BattleInstruction, rqInstruction);
    }

    public void SendUnitTargetSkillUseOperate(short skillId, int unitId)
    {
        RqBattleInstruction rqInstruction = new RqBattleInstruction();
        rqInstruction.instruction = new BattleUnitTargetSkill(unitId, skillId);
        GameMain.Instance.SocketMgr.SendMessage(RqId.BattleInstruction, rqInstruction);
    }
	#endregion

	#region 返回数据
	void OnRpBattleInstruction(object param)
	{
		GameBattle.LogicLayer.BattleInstructionManager.Instance.AddInstruction (param as RpBattleInstructionList);
	}
	#endregion
}
