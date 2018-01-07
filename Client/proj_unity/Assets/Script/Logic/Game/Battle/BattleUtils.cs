using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;

namespace GameBattle
{
	public class BattleUtils{
		public static float FastDistance(Vector2 pos1, Vector2 pos2)
		{
			return (pos1 - pos2).magnitude;
		}


		public static Vector3 LogicPosToScenePos(IntVector2 logicPos)
		{
			return new Vector3 (logicPos.x / 1000f, 0f, logicPos.y / 1000f);
		}
	}
}