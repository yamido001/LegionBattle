using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle{
	namespace BattleView
	{
		/// <summary>
		/// 展示层战斗单位管理器
		/// </summary>
		public class BattlePlayerManager : Singleton<BattlePlayerManager> 
		{
			Dictionary<int, BattleUnit> mUnitDic = new Dictionary<int, BattleUnit>();
			List<int> mToRemoveList = new List<int>();
			
			public void Start(List<FighterConfigData> fighterDatas)
			{
				for (int i = 0; i < fighterDatas.Count; ++i) {
					BattleUnit battleUnit = new BattleUnit ();
					battleUnit.Init (fighterDatas[i]);
					mUnitDic.Add (battleUnit.ID, battleUnit);
				}
			}

			public void Update()
			{
				mToRemoveList.Clear ();
				
				List<BaseInstruction> instructionList = InstructionManager.Instance.GetAllInstruction ();
				for (int i = 0; i < instructionList.Count; ++i) {
					BaseInstruction instruction = instructionList [i];
					BattleUnit battleUnit = GetUnit (instruction.selfId);
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

			BattleUnit GetUnit(int id)
			{
				BattleUnit ret;
				mUnitDic.TryGetValue (id, out ret);
				return ret;
			}
		}
	}
}
