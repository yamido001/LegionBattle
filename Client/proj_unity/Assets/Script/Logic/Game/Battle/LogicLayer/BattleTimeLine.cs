using System.Collections;
using System.Collections.Generic;
using GameBattle;

using LegionBattle.ServerClientCommon;

namespace GameBattle.LogicLayer
{
	public class BattleTimeLine : Singleton<BattleTimeLine>{

		int mCurFrameCount = 0;

		#region 生命周期
		public void StartBattle(List<UnitConfigData> fighterDatas)
		{
			IsInBattle = true;
			mCurFrameCount = 0;
			BattleUnitManager.Instance.StartBattle (fighterDatas);
		}

		public void Update()
		{
			int lastFrameCount = BattleInstructionManager.Instance.LatestInstructionFrameCount;
			int curCount = 0;
			while (mCurFrameCount < lastFrameCount && curCount++ < ConstConfig.BattleOneFramemMaxUpdateTimes) {
				PlayOneFrame ();
			}
			BattleUnitManager.Instance.Update ();
		}

		public void Destroy()
		{
			BattleUnitManager.Instance.DestroyBattle ();
			IsInBattle = false;
		}
		#endregion

		public bool IsInBattle {
			get;
			private set;
		}

		void PlayOneFrame()
		{
			mCurFrameCount++;
			OneFrameInstructions instructions = BattleInstructionManager.Instance.GetFrameInstructions ();
			if (null == instructions)
				return;
			Logger.LogWarning ("取得指令: " + instructions.frameCount);
			Utils.Assert (instructions.frameCount == mCurFrameCount, "战斗需要第" + mCurFrameCount + "但是确拿到了第" + instructions.frameCount + "帧的指令");
			BattleUnitManager unitManager = BattleUnitManager.Instance;
			for (int i = 0; i < instructions.instructionList.Count; ++i) {
				BattleInstructionBase instruction = instructions.instructionList [i];
				unitManager.SetBattleInstruction (instruction);
			}
		}
	}
}