using System.Collections;
using System.Collections.Generic;
using GameBattle;
using GameBattle.LogicLayer;
using GameBattle.BattleView;
using LegionBattle.ServerClientCommon;

namespace GameBattle
{
	public class BattleTimeLine : Singleton<BattleTimeLine>{
		
		public int CurFrameCount
		{
			get;
			private set;
		}

		#region 生命周期
		public void StartBattle(List<UnitConfigData> fighterDatas)
		{
			IsInBattle = true;
			CurFrameCount = 0;
			BattleFiled.Instance.AddUnitMoveListener (delegate(int unitId, IntVector2 fromPos, IntVector2 toPos) {
				BattleActorManager.Instance.OnUnitMove(unitId, fromPos, toPos);
			});
			BattleFiled.Instance.AddUnitEnterIdleListener (delegate(int unitId) {
				BattleActorManager.Instance.OnUnitEnterIdle(unitId);
			});
            BattleFiled.Instance.AddSkillEffectListener(delegate (short skillEffectId, IntVector2 pos)
            {
                GDSKit.SkillEffect skillEffect = GDSKit.SkillEffect.GetInstance(skillEffectId);
                EffectManager.Instance.CreateEffect(skillEffect.gameEffectId, BattleUtils.LogicPosToScenePos(pos));
            });
            BattleFiled.Instance.AddUnitAttrChgListener(delegate (int unitId, FighterAttributeType attrType, int value)
            {
                BattleActorManager.Instance.OnUnitAttrChg(unitId, attrType, value);
            });
            BattleFiledLattile.Instance.InitLattile(1000);
			BattleUnitManager.Instance.StartBattle (fighterDatas);
			BattleActorManager.Instance.StartBattle (fighterDatas);
			BattleLogManager.Instance.Start ();
		}

		public void Update()
		{
			if (!IsInBattle)
				return;
			int lastFrameCount = BattleInstructionManager.Instance.LatestInstructionFrameCount;
			int curCount = 0;
			while (CurFrameCount < lastFrameCount && curCount++ < ConstConfig.BattleOneFramemMaxUpdateTimes) {
				PlayOneFrame ();
			}
		}

		public void Destroy()
		{
			BattleUnitManager.Instance.DestroyBattle ();
			BattleActorManager.Instance.DestroyBattle ();
			IsInBattle = false;
		}
		#endregion

		public bool IsInBattle {
			get;
			private set;
		}

		void PlayOneFrame()
		{
			CurFrameCount++;
			OneFrameInstructions instructions = BattleInstructionManager.Instance.GetFrameInstructions ();
			if (null == instructions)
				return;
			//Logger.LogWarning ("取得指令: " + instructions.frameCount);
			Utils.Assert (instructions.frameCount == CurFrameCount, "战斗需要第" + CurFrameCount + "但是确拿到了第" + instructions.frameCount + "帧的指令");
			BattleUnitManager unitManager = BattleUnitManager.Instance;
			for (int i = 0; i < instructions.instructionList.Count; ++i) {
				BattleInstructionBase instruction = instructions.instructionList [i];
				unitManager.SetBattleInstruction (instruction);
			}

			BattleUnitManager.Instance.Update ();
			BattleActorManager.Instance.Update ();
		}
	}
}