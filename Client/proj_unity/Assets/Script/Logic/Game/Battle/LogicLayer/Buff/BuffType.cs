using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType : byte{
    ChgAttr = 1,
    SetSign,
}

public enum BuffEffectWayType
{
    Once = 1,   //只执行一次
    EverySomeFrame,     //每隔多少帧执行一次（特殊情况就是每一帧）
}