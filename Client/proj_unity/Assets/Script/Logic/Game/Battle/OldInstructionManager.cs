using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;

namespace GameBattle{

	public class OldInstructionManager : Singleton<OldInstructionManager> {

		List<OldBaseInstruction> mInputInstructionList = new List<OldBaseInstruction>();
		List<OldBaseInstruction> mOutPutInstructionList = new List<OldBaseInstruction>();

		/// <summary>
		/// 添加指令
		/// </summary>
		/// <param name="instruction">Instruction.</param>
		public void AddInstruction(OldBaseInstruction instruction)
		{
			mInputInstructionList.Add (instruction);
		}

		/// <summary>
		/// 生成移动指令
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="fromPos">From position.</param>
		/// <param name="toPos">To position.</param>
		public void CreateMoveInstruction(int id, IntVector2 fromPos, IntVector2 toPos)
		{
//			Debug.LogError ("CreateMoveInstruction  " + id + " " + fromPos.ToString() + "  " + toPos.ToString());
			OldMoveInstruction moveInstruction = new OldMoveInstruction ();
			moveInstruction.selfId = id;
			moveInstruction.fromPos = fromPos;
			moveInstruction.toPos = toPos;
			mInputInstructionList.Add (moveInstruction);
		}

		/// <summary>
		/// 生成技能使用的指令
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="skillId">Skill identifier.</param>
		/// <param name="targetId">Target identifier.</param>
		public void CreateSkillUseInstruction(int id, int skillId, int targetId)
		{
//			Debug.LogError ("CreateSkillUseInstruction  " + id + " " + skillId.ToString() + "  " + targetId.ToString());
			OldSkillUseInstruction attackInstruction = new OldSkillUseInstruction ();
			attackInstruction.selfId = id;
			attackInstruction.skillId = skillId;
			attackInstruction.targetId = targetId;
			mInputInstructionList.Add (attackInstruction);
		}

		public void CreateEnterIdleInstruction(int id)
		{
			OldEnterIdleInstruction idleInstruction = new OldEnterIdleInstruction ();
			idleInstruction.selfId = id;
			mInputInstructionList.Add (idleInstruction);
		}

		/// <summary>
		/// 生成属性改变指令
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="attrType">Attr type.</param>
		/// <param name="value">Value.</param>
		public void CreateAttrChgInstruction(int id, FighterAttributeType attrType, int value)
		{
//			Debug.LogError ("CreateAttrChgInstruction  " + id + " " + attrType.ToString() + "  " + value.ToString());
			OldAttrChgInstruction attrChgInstruction = new OldAttrChgInstruction ();
			attrChgInstruction.selfId = id;
			attrChgInstruction.attrType = attrType;
			attrChgInstruction.curValue = value;
			mInputInstructionList.Add (attrChgInstruction);
		}

		/// <summary>
		/// 获取这一帧的所有战斗执行
		/// </summary>
		/// <returns>The all instruction.</returns>
		public List<OldBaseInstruction> GetAllInstruction()
		{
			mOutPutInstructionList.Clear ();
			mOutPutInstructionList.AddRange (mInputInstructionList);
			mInputInstructionList.Clear ();
			return mOutPutInstructionList;
		}
	}
}


