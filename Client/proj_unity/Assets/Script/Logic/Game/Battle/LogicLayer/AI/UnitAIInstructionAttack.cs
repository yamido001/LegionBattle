﻿using System.Collections;
using System.Collections.Generic;
using LBCSCommon;

namespace GameBattle.LogicLayer
{
	public class UnitAIInstructionAttack : PreCreateStateBase {

        UnitBase mUnitBase;
        UnitAIComponent mAIComponent;
        short mSkillId;
        int mSkillTargetUnitId;
        short mSkillAngle;
        int mSkillParam1;
        bool mUsed = false;

		public UnitAIInstructionAttack(UnitBase unitBase, UnitAIComponent unitAIComp)
		{
            mUnitBase = unitBase;
            mAIComponent = unitAIComp;
		}

		public override void Entered (object param)
		{
            mUsed = false;
            BattleInstructionBase instruction = param as BattleInstructionBase;
			switch (instruction.InstructionType) {
			case BattleInstructionType.AreaTargetSkill:
                    BattleAreaTargetSkill targetSkillIns = instruction as BattleAreaTargetSkill;
                    mSkillId = targetSkillIns.SkillId;
                    mSkillAngle = targetSkillIns.SkillAngle;
                    mSkillParam1 = targetSkillIns.SkillParam1;
				break;
			case BattleInstructionType.NoTargetSkill:
                    BattleNoTargetSkill noTargetSkillIns = instruction as BattleNoTargetSkill;
                    mSkillId = noTargetSkillIns.SkillId;
				break;
			case BattleInstructionType.UnitTargetSkill:
                    BattleUnitTargetSkill unitTargetSkillIns = instruction as BattleUnitTargetSkill;
                    mSkillId = unitTargetSkillIns.SkillId;
                    mSkillTargetUnitId = unitTargetSkillIns.TargetUnitId;
				break;
			default:
				break;
			}
		}


        public override void Execute()
        {
            if (mUsed)
                return;
            mUnitBase.skillComp.UseSkill(mSkillId, mSkillTargetUnitId, mSkillAngle, mSkillParam1);
            mUsed = true;
        }
	}
}
