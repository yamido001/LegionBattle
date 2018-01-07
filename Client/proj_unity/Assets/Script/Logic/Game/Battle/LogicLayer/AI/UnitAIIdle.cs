using System.Collections;
using System.Collections.Generic;

namespace GameBattle.LogicLayer
{
	public class UnitAIIdle : PreCreateStateBase {

		UnitBase mUnitBase;
		UnitAIComponent mAIComponent;

		public UnitAIIdle(UnitBase unitBase, UnitAIComponent unitAIComp)
		{
			mUnitBase = unitBase;
			mAIComponent = unitAIComp;
		}

		public override void Entered (object param)
		{
			mUnitBase.EnterIdle ();
		}
	}
}