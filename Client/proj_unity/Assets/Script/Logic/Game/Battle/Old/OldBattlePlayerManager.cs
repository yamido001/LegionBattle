﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle{
	namespace BattleView
	{
		/// <summary>
		/// 展示层战斗单位管理器
		/// </summary>
		public class OldBattlePlayerManager : Singleton<OldBattlePlayerManager> 
		{
			Dictionary<int, OldBattleUnit> mUnitDic = new Dictionary<int, OldBattleUnit>();
			List<int> mToRemoveList = new List<int>();
			
			public void Start(List<UnitConfigData> fighterDatas)
			{
				for (int i = 0; i < fighterDatas.Count; ++i) {
					OldBattleUnit battleUnit = new OldBattleUnit ();
					battleUnit.Init (fighterDatas[i]);
					mUnitDic.Add (battleUnit.ID, battleUnit);
				}
			}

			public void Update()
			{
				mToRemoveList.Clear ();
				
				List<OldBaseInstruction> instructionList = OldInstructionManager.Instance.GetAllInstruction ();
				for (int i = 0; i < instructionList.Count; ++i) {
					OldBaseInstruction instruction = instructionList [i];
					OldBattleUnit battleUnit = GetUnit (instruction.selfId);
					battleUnit.ExecuteInstruction (instruction);
					if (battleUnit.IsDead) {
						mToRemoveList.Add (battleUnit.ID);
					}
				}

				for (int i = 0; i < mToRemoveList.Count; ++i) {
					int removeId = mToRemoveList [i];
					mUnitDic [removeId].Destroy ();
					mUnitDic.Remove (removeId);
				}

				var battleUnitEnumerator = mUnitDic.GetEnumerator ();
				while (battleUnitEnumerator.MoveNext ()) {
					battleUnitEnumerator.Current.Value.Update ();
				}
			}

			public void Destroy()
			{
				var battleUnitEnumerator = mUnitDic.GetEnumerator ();
				while (battleUnitEnumerator.MoveNext ()) {
					battleUnitEnumerator.Current.Value.Destroy ();
				}
				mUnitDic.Clear ();
			}

			OldBattleUnit GetUnit(int id)
			{
				OldBattleUnit ret;
				mUnitDic.TryGetValue (id, out ret);
				return ret;
			}
		}
	}
}