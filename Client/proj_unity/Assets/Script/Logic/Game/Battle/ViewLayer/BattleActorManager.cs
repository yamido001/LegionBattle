using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;

namespace GameBattle.BattleView
{
	public class BattleActorManager : Singleton<BattleActorManager>{

		Dictionary<int, BattleActorBase> mBattleActorDic = new Dictionary<int, BattleActorBase>();

        public Transform ActorRootTf
        {
            get;
            private set;
        }

		public void StartBattle(List<UnitConfigData> fighterDatas)
		{
            ActorRootTf = new GameObject().transform;
            ActorRootTf.name = "BattleActorRoot";

            for (int i = 0; i < fighterDatas.Count; ++i) {
				BattleActorBase actor = new BattleActorBase (fighterDatas[i].id, fighterDatas[i].life, fighterDatas[i].borthPos);
				mBattleActorDic [actor.Id] = actor;
			}
		}

		public void Update()
		{
			var enumerator = mBattleActorDic.GetEnumerator ();
			while (enumerator.MoveNext ()) {
				enumerator.Current.Value.Update ();
			}
		}

		public void DestroyBattle()
		{
			var enumerator = mBattleActorDic.GetEnumerator ();
			while (enumerator.MoveNext ()) {
				enumerator.Current.Value.Destroy ();
			}
			mBattleActorDic.Clear ();
		}

        void DestroyActor(int unitId)
        {
            BattleActorBase ret = null;
            mBattleActorDic.TryGetValue(unitId, out ret);
            if (null != ret)
                ret.Destroy();
            mBattleActorDic.Remove(unitId);
        }

		BattleActorBase GetActorById(int unitId)
		{
			BattleActorBase ret = null;
			mBattleActorDic.TryGetValue (unitId, out ret);
			return ret;
		}

		public void OnUnitMove(int unitId, IntVector2 fromPos, IntVector2 toPos)
		{
			BattleActorBase actor = GetActorById (unitId);
			if (null != actor) {
				actor.OnMove (fromPos, toPos);
			}
		}

        public void OnUnitEnterIdle(int unitId)
		{
			BattleActorBase actor = GetActorById (unitId);
			if (null != actor) {
				actor.OnEnterIdle ();
			}
		}

        public void OnUnitAttrChg(int unitId, FighterAttributeType attrType, int value)
        {
            if(attrType == FighterAttributeType.Life)
            {
                if(value <= 0)
                {
                    DestroyActor(unitId);
                }
                else
                {
                    BattleActorBase actor = GetActorById(unitId);
                    if (null == actor)
                    {
                        return;
                    }
                    actor.OnLifeChg(value);
                }
            }
        }
	}
}

