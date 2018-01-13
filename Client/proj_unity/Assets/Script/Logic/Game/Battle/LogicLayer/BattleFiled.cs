using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;

namespace GameBattle.LogicLayer
{
	/// <summary>
	/// 战场管理器，负责战斗逻辑的判断和效果的执行
	/// </summary>
	public class BattleFiled : Singleton<BattleFiled>{

		#region 回调通知
		System.Action<int, IntVector2, IntVector2> mOnMoveListener;
		public void AddUnitMoveListener(System.Action<int, IntVector2, IntVector2> listener)
		{
			mOnMoveListener = listener;
		}

		public void OnUnitMove(int unitId, IntVector2 fromPos, IntVector2 toPos)
		{
			if (null != mOnMoveListener)
				mOnMoveListener.Invoke (unitId, fromPos, toPos);
		}

		System.Action<int> mOnEnterIdleListener;
		public void AddUnitEnterIdleListener(System.Action<int> listener)
		{
			mOnEnterIdleListener = listener;
		}

		public void OnUnitEnterIdle(int unitId)
		{
			if (null != mOnEnterIdleListener)
				mOnEnterIdleListener.Invoke (unitId);
		}
		#endregion

		#region 技能
		public void FighterTryUseSkill(int fighterId, int skillId, int targetFighterId)
		{
			UnitBase casterUnit = BattleUnitManager.Instance.GetUnitByUnitId (fighterId);
			if (casterUnit == null || casterUnit.IsDead) {
				return;
			}
			UnitBase targetUnit = BattleUnitManager.Instance.GetUnitByUnitId (targetFighterId);
			if (null == targetUnit || targetUnit.IsDead) {
				return;
			}
			GDSKit.SkillConfig skillConfig = GDSKit.SkillConfig.GetInstance (skillId);

			bool skillResult = false;
			switch (skillConfig.targetInfo.type) {
			case (int)SkillType.NoTargetSkill:
				skillResult = ExecuteNoTargetEffect (casterUnit, skillConfig);
				break;
			case (int)SkillType.UnitTargetSkill:
				skillResult = ExecuteUnitTargetEffect (casterUnit, targetUnit, skillConfig);
				break;
			case (int)SkillType.AreaTargetSkill:
				skillResult = ExecuteAreaTargetEffect (casterUnit, skillConfig);
				break;
			default:
				break;
			}

			if(skillResult)
				casterUnit.OnUseSkill (skillId, targetFighterId);
		}

		bool ExecuteNoTargetEffect(UnitBase skillUser, GDSKit.SkillConfig skillConfig)
		{
			return true;
		}

		bool ExecuteUnitTargetEffect(UnitBase skillUser, UnitBase skillTarget, GDSKit.SkillConfig skillConfig)
		{
			return true;
		}

		bool ExecuteAreaTargetEffect(UnitBase skillUser, GDSKit.SkillConfig skillConfig)
		{
			return true;
		}

        void ExecuteSkillEffect(UnitBase )

		//TODO
		//int damage = skillUser.GetAttribute(FighterAttributeType.Attack) * skillConfig.attackPercentage / 100;
		/*int damage = 50;
		int preLife = targetUnit.GetAttribute (FighterAttributeType.Life);
		preLife -= damage;

		if (preLife <= 0) {
			BattleUnitManager.Instance.OnUnitDead (targetFighterId);
		} else {
			targetUnit.SetAttribute (FighterAttributeType.Life, preLife);
		}*/
		#endregion
	}
}


