using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle.LogicLayer
{
    public abstract class SkillUseParameter
    {
        public short skillId;
    }

    public class NoTargetSkillUseParam : SkillUseParameter
    {
        
    }

    public class AreaTargetSkillUseParam : SkillUseParameter
    {
        public short skillAngle;
        public short skillParam1;
        public short skillParam2;
    }

    public class UnitTargetSkillUseParam : SkillUseParameter
    {
        public int targetUnitId;
    }
}
