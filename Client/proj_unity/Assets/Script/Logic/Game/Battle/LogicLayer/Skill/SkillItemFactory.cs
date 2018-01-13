using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle.LogicLayer.Skill
{
    public class SkillItemFactory
    {
        public static SkillItem CreateSkillItem(UnitBase casterUnit, int skillId, System.Action<short> hdlOnFinish)
        {
            GDSKit.SkillConfig gdsConfig = GDSKit.SkillConfig.GetInstance(skillId);
            SkillItem ret = null;
            switch((SkillType)gdsConfig.targetInfo.type)
            {
                case SkillType.NoTargetSkill:
                    ret = new NoTargetSkillItem();
                    break;
                case SkillType.UnitTargetSkill:
                    ret = new UnitTargetSkillItem();
                    break;
                case SkillType.AreaTargetSkill:
                    ret = new AreaTargetSkillItem();
                    break;
                default:
                    break;
            }
            ret.SetSkillInfo(casterUnit, gdsConfig, hdlOnFinish);
            return ret;
        }

    }
}