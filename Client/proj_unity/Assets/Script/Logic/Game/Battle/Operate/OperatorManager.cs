using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameBattle.LogicLayer;
using System;
using LegionBattle.ServerClientCommon;

namespace GameBattle.LogicLayer
{
	public class OperatorManager : Singleton<OperatorManager> {

		short mMoveAngle;

		public void OnBattleStart()
		{
			mMoveAngle = short.MaxValue;
		}

		public void MoveJoystick(Vector2 opeValue)
		{
			if (!BattleTimeLine.Instance.IsInBattle)
				return;

            short angle = Vector2ToAngle(opeValue);
			if (Math.Abs (mMoveAngle - angle) > BattleConfig.joyStickFilterMaxAngle) {
				mMoveAngle = angle;
				GameMain.Instance.ProxyMgr.Battle.SendMoveOperate (mMoveAngle);
			}
		}

        public void UseAreaSkill(short skillId, Vector2 opeValue)
        {
            short skillAngle = Vector2ToAngle(opeValue);
            IntegerFloat disPercent = IntegerFloat.FloatToIntegerFloat(opeValue.magnitude);
            GameMain.Instance.ProxyMgr.Battle.SendAreaUseSkillOperate(skillId, skillAngle, (short)disPercent.x, (short)disPercent.y);
        }

        public void UseNoTargetSkill(short skillId)
        {
            GameMain.Instance.ProxyMgr.Battle.SendNoTargetSkillUseOperate(skillId);
        }

        public void UseUnitTargetSkill(short skillId, int targetUnitId)
        {
            GameMain.Instance.ProxyMgr.Battle.SendUnitTargetSkillUseOperate(skillId, targetUnitId);
        }

        short Vector2ToAngle(Vector2 opeValue)
        {
            Vector3 crossDir = Vector3.Cross(Vector2.right, opeValue);
            int angle = (int)Vector2.Angle(Vector2.right, opeValue);
            angle = crossDir.z < 0f ? -angle : angle;
            angle = (angle + 360) % 360;
            return (short)angle;
        }

		public void StopMove()
		{
			GameMain.Instance.ProxyMgr.Battle.StopMoveOperate ();
		}

		public void OnBattleEnd()
		{
			
		}
	}		
}