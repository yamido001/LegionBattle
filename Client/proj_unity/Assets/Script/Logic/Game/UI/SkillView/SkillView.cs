using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LBCSCommon;
using UnityEngine.EventSystems;
using GameBattle.LogicLayer;

public class SkillView : UIViewBase {

    SkillViewOpe[] mSkillOpes;
	

	public override void OnOpend (object openParam)
	{
		base.OnOpend (openParam);
        List<int> selfSkillList = openParam as List<int>;
        mSkillOpes = new SkillViewOpe[selfSkillList.Count];
        for (int i = 0; i < selfSkillList.Count; ++i)
        {
            int skillId = selfSkillList[i];
            GDSKit.SkillConfig skillConfig = GDSKit.SkillConfig.GetInstance(skillId);
            Transform tf = PrefabTf.Find("skill" + i);
            switch((SkillType)skillConfig.targetInfo.type)
            {
                case SkillType.NoTargetSkill:
                    break;
                case SkillType.UnitTargetSkill:
                    break;
                case SkillType.AreaTargetSkill:
                    if(skillConfig.distance <= 0)
                    {
                        SkillViewClickOpe clickOpe = new SkillViewClickOpe();
                        clickOpe.Init(skillConfig, tf);
                        mSkillOpes[i] = clickOpe;
                    }
                    else
                    {
                        SkillViewJostickOpe jostickOpe = new SkillViewJostickOpe();
                        jostickOpe.Init(skillConfig, tf);
                        mSkillOpes[i] = jostickOpe;
                    }
                    break;
                default:
                    throw new System.NotImplementedException("技能界面未实现的技能类型 " + skillConfig.targetInfo.type);
            }
        }
    }

	public override void OnClose ()
	{
		base.OnClose ();
		
	}


	void BeginDrag(BaseEventData eventData)
	{
		
	}

	void EndDrag(BaseEventData eventData)
	{
		
	}

	void OnValueChanged(Vector2 value)
	{
		
	}

}
