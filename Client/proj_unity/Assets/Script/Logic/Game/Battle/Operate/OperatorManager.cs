using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameBattle.LogicLayer;
using System;

namespace GameBattle.LogicLayer
{
	public class OperatorManager : Singleton<OperatorManager> {

		short mAngle;

		public void OnBattleStart()
		{
			mAngle = short.MaxValue;
		}

		public void MoveJoystick(Vector2 opeValue)
		{
			if (!BattleTimeLine.Instance.IsInBattle)
				return;
			
			Vector3 crossDir = Vector3.Cross (Vector2.right, opeValue);
			float angle = Vector2.Angle (Vector2.right, opeValue);
			angle = crossDir.z < 0f ? -angle : angle; 

			if (Math.Abs (mAngle - (short)angle) > BattleConfig.joyStickFilterMaxAngle) {
				mAngle = (short)(((short)angle + 360) % 360);
				GameMain.Instance.ProxyMgr.Battle.SendMoveOperate (mAngle);
			}
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