using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle{

	public class FighterConfigData{
		public int 		id;
		public int		camp;
		public int 		life;
		public int 		speed;
		public int 		attack;
		public int 		attackRange;
		public int[] 	skillList;
		public BattlePosition 	borthPos;

		public bool IsAttack
		{
			get{
				return camp == 1;
			}
		}
	}

	public enum FighterAttributeType
	{
		Life = 0,
		Speed,
		Attack,
		AttackRange,
		Count,
	}

	public class FighterAttribute
	{
		public int 		life;
		public int 		speed;
		public int 		attack;
		public int 		attackRange;
		public Vector2 	pos;
	}
}


