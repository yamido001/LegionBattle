using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager> {

    Dictionary<int, Effect> mEffectDic = new Dictionary<int, Effect>();


    public Transform effectTfRoot
    {
        get;
        private set;
    }

    public void Init()
    {
        effectTfRoot = GameMain.Instance.CreateChildTransform("effectMgr");
    }

    public int CreateEffect(short effectId, Vector3 pos)
    {
        Effect newEffect = new Effect(effectId, effectTfRoot, pos);
        mEffectDic[newEffect.iD] = newEffect;
        return newEffect.iD;
    }

    public void ClearAllEffect()
    {
        foreach(var item in mEffectDic)
        {
            item.Value.Destroy();
        }
        mEffectDic.Clear();
    }
}
