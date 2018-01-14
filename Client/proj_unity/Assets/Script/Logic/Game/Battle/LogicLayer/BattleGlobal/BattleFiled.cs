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

        System.Action<short, IntVector2> mEffectHdlListener;
        public void AddSkillEffectListener(System.Action<short, IntVector2> listener)
        {
            mEffectHdlListener = listener;
        }

        System.Action<int, FighterAttributeType, int> mAttrChgListener;
        public void AddUnitAttrChgListener(System.Action<int, FighterAttributeType, int> listener)
        {
            mAttrChgListener = listener;
        }
		#endregion

		#region 技能效果
        public void OnUnitDamaged(short effectId, int unitId, int damage)
        {
            UnitBase targetUnit = BattleUnitManager.Instance.GetUnitByUnitId(unitId);
            if (null == targetUnit || targetUnit.IsDead)
                return;
            int preLife = targetUnit.GetAttribute(FighterAttributeType.Life);
            preLife -= damage;
            if(preLife <= 0)
            {
                BattleUnitManager.Instance.OnUnitDead(unitId);
            }
            else
            {
                targetUnit.SetAttribute(FighterAttributeType.Life, preLife);
            }
            mAttrChgListener.Invoke(unitId, FighterAttributeType.Life, preLife);
            mEffectHdlListener.Invoke(effectId, targetUnit.Position);
        }
		#endregion
	}
}


