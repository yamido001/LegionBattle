using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager> {

    Dictionary<int, Effect> mEffectDic = new Dictionary<int, Effect>();
	
    public int CreateEffect(short effectId)
    {
        return new Effect(effectId).iD;
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
