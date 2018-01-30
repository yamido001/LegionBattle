using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitAIStateType{
	Idle,
	InstructionMove,	//玩家操作的移动
	InstructionSkill,	//玩家操作的放技能
    RandomMove,         //临时供测试使用，或者以后可以当做技能效果
}
