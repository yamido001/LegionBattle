using System;
using UnityEngine;
using GameBattle.LogicLayer;
using UnityEngine.UI;

/// <summary>
/// 无目标技能的释放按钮和操作逻辑
/// </summary>
public class SkillViewClickOpe : SkillViewOpe
{
    GDSKit.SkillConfig mSkillConfig;
    Button mBtn;
    GameObject mObj;

    public void Init(GDSKit.SkillConfig skillConfig, Transform rootTf)
    {
        mSkillConfig = skillConfig;
        GameMain.Instance.ResMgr.LoadResourceAsync(this, "clickSkillOpe", typeof(GameObject), delegate (UnityEngine.Object prefab)
            {
                mObj =GameObject.Instantiate<GameObject>(prefab as GameObject);
                mObj.transform.SetParent(rootTf);
                mObj.transform.Reset();
                mBtn = mObj.GetComponent<Button>();
                EventTriggerListener.Get(mBtn.gameObject).onClick = delegate(GameObject obj) {
                    if (mSkillConfig.targetInfo.type == (short)SkillType.NoTargetSkill)
                        OperatorManager.Instance.UseNoTargetSkill(mSkillConfig.id);
                    else
                        OperatorManager.Instance.UseAreaSkill(mSkillConfig.id, Vector2.zero);
                };
            }, delegate (string errorCode)
            {

            });
    }

    public void Destroy()
    {
        GameMain.Instance.ResMgr.UnloadResource("clickSkillOpe", typeof(GameObject));
    }
}
