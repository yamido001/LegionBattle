using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle
{
	public class BattleUtils{
		public static float FastDistance(Vector2 pos1, Vector2 pos2)
		{
			return (pos1 - pos2).magnitude;
		}
	}
}