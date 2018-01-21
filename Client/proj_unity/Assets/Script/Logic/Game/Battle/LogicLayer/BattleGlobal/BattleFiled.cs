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

        System.Action<IntVector2> mOnAngleListener;
        public void AddUnitAngleListener(System.Action<IntVector2> listener)
        {
            mOnAngleListener = listener;
        }

        public void OnUnitAngleChange(IntVector2 angle)
        {
            if (null != mOnAngleListener)
                mOnAngleListener.Invoke(angle);
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

        System.Action<int, short> mOnUseSkillListener;
        public void AddUseSkillListener(System.Action<int, short> listener)
        {
            mOnUseSkillListener = listener;
        }

        public void OnUnitUseSkill(int unitId, short skillId)
        {
            if (null != mOnUseSkillListener)
                mOnUseSkillListener.Invoke(unitId, skillId);
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

        System.Action<int, short, bool> mUnitBuffChgListener;
        public void AddUnitBuffChgListener(System.Action<int, short, bool> listener)
        {
            mUnitBuffChgListener = listener;
        }
		#endregion

		#region 技能效果
        public void OnCreateEffect(short effectId, IntVector2 pos)
        {
            mEffectHdlListener.Invoke(effectId, pos);
        }

        public void ChangeUnitAttr(UnitBase targetUnit, FighterAttributeType attrType, int chgValue)
        {
            Debug.Log("技能效果生效 目標：" + targetUnit.ID + " 傷害:" + attrType);
            int preValue = targetUnit.GetAttribute(attrType);
            preValue += chgValue;
            if(attrType == FighterAttributeType.Life && preValue <= 0)
            {
                BattleUnitManager.Instance.OnUnitDead(targetUnit.ID);
            }
            else
            {
                targetUnit.SetAttribute(attrType, preValue);
            }
            mAttrChgListener.Invoke(targetUnit.ID, attrType, preValue);
        }

        public void AddBuffer(UnitBase targetUnit, short buffId)
        {
            targetUnit.buffComp.AddBuff(buffId);
            mUnitBuffChgListener.Invoke(targetUnit.ID, buffId, true);
        }

        public void OnBuffExpire(int unitId, short buffId)
        {
            mUnitBuffChgListener.Invoke(unitId, buffId, false);
        }
		#endregion
	}
}


