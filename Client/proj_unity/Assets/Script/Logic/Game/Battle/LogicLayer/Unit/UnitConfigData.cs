using UnityEngine;
using LBMath;

namespace GameBattle{

	public class UnitConfigData{
		public int 		id;
		public CampType	camp;
		public int 		life;
		public int 		speed;
		public int 		attack;
		public int 		attackRange;
		public int[] 	skillList;
		public IntVector2 	borthPos;
        public bool     isRandomMove;

		public bool IsAttack
		{
			get{
                //TODO
                return true;
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

    public enum CampType : byte
    {
        Justice = 1,
        Evil,
    }
}


