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
        Effect newEffect = new Effect(effectId, effectTfRoot, pos, DestroyEffect);
        mEffectDic[newEffect.iD] = newEffect;
        return newEffect.iD;
    }

    public void DestroyEffect(int effectId)
    {
        Effect effect = null;
        if (!mEffectDic.TryGetValue(effectId, out effect))
            return;
        effect.Destroy();
        mEffectDic.Remove(effectId);
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
