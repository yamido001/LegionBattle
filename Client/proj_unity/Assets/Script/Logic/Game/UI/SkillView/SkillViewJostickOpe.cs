using System;
using UnityEngine;
using GameBattle.LogicLayer;

/// <summary>
/// 区域技能的释放按钮和操作逻辑
/// </summary>
public class SkillViewJostickOpe : SkillViewOpe
{
    short mSkillId;
    JostickController mJostickControl;
    GameObject mObj;

    public void Init(short skillId, Transform rootTf)
    {
        mSkillId = skillId;
        GameMain.Instance.ResMgr.LoadResourceAsync(this, "jostickSkillOpe", typeof(GameObject), delegate (UnityEngine.Object prefab)
            {
                mObj =GameObject.Instantiate<GameObject>(prefab as GameObject);
                mObj.transform.SetParent(rootTf);
                mObj.transform.Reset();
                mJostickControl = new JostickController();
                mJostickControl.Init(mObj.transform, null, null, delegate()
                    {
                        OperatorManager.Instance.UseAreaSkill(mSkillId, mJostickControl.movementVector);
                    }
                );
            }, delegate (string errorCode)
            {

            });
    }

    public void Destroy()
    {
        mJostickControl.Destroy();
        GameMain.Instance.ResMgr.UnloadResource("jostickSkillOpe", typeof(GameObject));
    }
}
