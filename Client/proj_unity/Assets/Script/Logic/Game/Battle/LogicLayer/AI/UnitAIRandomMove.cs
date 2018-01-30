using System.Collections;
using System.Collections.Generic;

using GameBattle.LogicLayer.Scene;
using System;
using LBCSCommon;

namespace GameBattle.LogicLayer
{
	public class UnitAIRandomMove : PreCreateStateBase {

		UnitBase mUnitBase;
		UnitAIComponent mAIComponent;
        short mCurFrameCount;
        short mMoveAngle;

		public UnitAIRandomMove(UnitBase unitBase, UnitAIComponent unitAIComp)
		{
			mUnitBase = unitBase;
			mAIComponent = unitAIComp;
		}

		public override void Entered (object param)
		{
            mCurFrameCount = 0;
        }

        public override void Execute()
        {
            if(++mCurFrameCount > 600)
            {
                RandomMoveTarget();
                mCurFrameCount = 0;
            }
            mUnitBase.moveComp.MoveAngle(mMoveAngle);
        }

        void RandomMoveTarget()
        {
            mMoveAngle = (short)UnityEngine.Random.Range(0, 359);
        }
    }
}