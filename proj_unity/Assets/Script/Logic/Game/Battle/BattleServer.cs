using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle
{
	/// <summary>
	/// 前端模拟的服务器层
	/// </summary>
	public class BattleServer : Singleton<BattleServer>{

		List<Fighter> mFighters = new List<Fighter>();
		Dictionary<int, Fighter> mFighterDic = new Dictionary<int, Fighter>();
		List<Fighter> mAttackFighterList = new List<Fighter>();
		List<Fighter> mDefenderFighterList = new List<Fighter>();


		public void StartBattle(List<FighterConfigData> fighterDatas)
		{
			for (int i = 0; i < fighterDatas.Count; ++i) {
				Fighter fighter = new Fighter ();
				fighter.Init (fighterDatas [i]);
				mFighters.Add (fighter);
				mFighterDic.Add (fighter.ID, fighter);
				if (fighter.Data.IsAttack) {
					mAttackFighterList.Add (fighter);
				} else {
					mDefenderFighterList.Add (fighter);
				}
			}
		}

		public void Update()
		{
			for (int i = 0; i < mFighters.Count; ++i) {
				mFighters [i].Update ();
			}
			for (int i = mFighters.Count - 1; i >= 0; --i) {
				if (!mFighters [i].IsDead)
					continue;
				mFighters [i].Destroy ();
				mFighters.RemoveAt (i);
			}
		}

		public void DestroyBattle()
		{
			for (int i = 0; i < mFighters.Count; ++i) {
				mFighters [i].Destroy ();
			}
			mFighters = null;
		}

		Fighter FindById(int id)
		{
			return mFighterDic [id];
		}

		public List<Fighter> GetEnemyFighterList(int id)
		{
			Fighter requestFighter = FindById (id);
			return requestFighter.Data.IsAttack ? mDefenderFighterList : mAttackFighterList;
		}

		public void FighterTryUseSkill(int fighterId, int skillId, int targetFighterId)
		{
			Fighter skillUser = FindById (fighterId);
			if (skillUser == null || skillUser.IsDead) {
				return;
			}
			Fighter targetFighter = FindById (targetFighterId);
			if (null == targetFighter || targetFighter.IsDead) {
				return;
			}
			GDSKit.SkillConfig skillConfig = GDSKit.SkillConfig.GetInstance (skillId);
			int damage = skillUser.GetAttribute(FighterAttributeType.Attack) * skillConfig.attackPercentage / 100;
			int preLife = targetFighter.GetAttribute (FighterAttributeType.Life);
			preLife -= damage;

			if (preLife <= 0) {
				if (targetFighter.Data.IsAttack)
					mAttackFighterList.Remove (targetFighter);
				else
					mDefenderFighterList.Remove (targetFighter);
				targetFighter.OnDead ();
			} else {
				targetFighter.SetAttribute (FighterAttributeType.Life, preLife);
			}

			skillUser.OnUseSkill (skillId, targetFighterId);
		}
	}
}