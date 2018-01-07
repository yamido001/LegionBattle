using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;

namespace GameBattle.LogicLayer
{
	public class OneFrameInstructions{
		public int frameCount;
		public List<BattleInstructionBase> instructionList = new List<BattleInstructionBase>();
	}	
}