﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;

namespace GameBattle{

	public class UnitConfigData{
		public int 		id;
		public int		camp;
		public int 		life;
		public int 		speed;
		public int 		attack;
		public int 		attackRange;
		public int[] 	skillList;
		public IntVector2 	borthPos;

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

