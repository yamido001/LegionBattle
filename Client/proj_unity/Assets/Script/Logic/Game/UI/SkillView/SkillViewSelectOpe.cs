﻿using System;
using UnityEngine;
using GameBattle.LogicLayer;
using UnityEngine.UI;

/// <summary>
/// 目标技能的释放按钮和操作逻辑
/// </summary>
public class SkillViewSelectOpe : SkillViewOpe
{
    short mSkillId;
    Button mBtn;
    GameObject mObj;

    public void Init(short skillId, Transform rootTf)
    {
        mSkillId = skillId;
        GameMain.Instance.ResMgr.LoadResourceAsync(this, "selectSkillOpe", typeof(GameObject), delegate (UnityEngine.Object prefab)
        {
            mObj = GameObject.Instantiate<GameObject>(prefab as GameObject);
            mObj.transform.SetParent(rootTf);
            mObj.transform.Reset();
            mBtn = mObj.GetComponent<Button>();
            EventTriggerListener.Get(mBtn.gameObject).onClick = delegate (GameObject obj) {
                
            };
        }, delegate (string errorCode)
        {

        });
    }

    public void Destroy()
    {
        GameMain.Instance.ResMgr.UnloadResource("selectSkillOpe", typeof(GameObject));
    }
}
