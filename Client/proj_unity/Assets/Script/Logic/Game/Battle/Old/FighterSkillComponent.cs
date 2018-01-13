using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle
{
	namespace LogicLayer
	{
		public class FighterSkillComponent{

			public bool IsInCD
			{
				get;
				private set;
			}

			public bool IsBusy {
				get{
					return mBusyTime > 0;
				}
			}

			public int[] skillList {
				get;
				private set;
			}

			UnitBase mFighter;
			int mBusyTime;
			int[] mSkillCDList;

			public FighterSkillComponent(UnitBase fighter, int[] skillList)
			{
				mFighter = fighter;
				this.skillList = skillList;
				mSkillCDList = new int[skillList.Length];
				mBusyTime = 0;
				IsInCD = false;
			}

			public void Update()
			{
				--mBusyTime;
				if (mBusyTime == 0) {

				}
				for (int i = 0; i < mSkillCDList.Length; ++i) {
					mSkillCDList [i] -= 1;
				}
			}

			public void UseSkill(int skillId, int targetFighterId)
			{
				if (!HasSkill (skillId))
					return;
				GDSKit.SkillConfig skillConfig = GDSKit.SkillConfig.GetInstance (skillId);
				if (skillConfig == null)
					return;
				if (IsBusy)
					return;
				if (IsSkillIdCd (skillId))
					return;
			}

			public void OnUseSkill(int skillId, int targetFighterId)
			{
				GDSKit.SkillConfig skillConfig = GDSKit.SkillConfig.GetInstance (skillId);
				mBusyTime = skillConfig.consumeTime;
				UpdateSkillCd (skillId, skillConfig.cd);
			}

			protected bool HasSkill(int id)
			{
				for(int i = 0; i < skillList.Length; ++i)
				{
					if (skillList [i] == id)
						return true;
				}
				return false;
			}

			protected bool IsSkillIdCd(int skillId)
			{
				int skillIndex = -1;
				for(int i = 0; i < skillList.Length; ++i)
				{
					if (skillList [i] == skillId) {
						skillIndex = i;
						break;
					}
				}
				return mSkillCDList [skillIndex] > 0;
			}

			protected void UpdateSkillCd(int skillId, int cd)
			{
				int skillIndex = -1;
				for(int i = 0; i < skillList.Length; ++i)
				{
					if (skillList [i] == skillId) {
						skillIndex = i;
						break;
					}
				}
				mSkillCDList [skillIndex] = cd;
			}
		}
	}
}