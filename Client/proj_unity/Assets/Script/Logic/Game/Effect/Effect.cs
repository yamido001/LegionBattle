using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect{

    static int Count = 0;

    GDSKit.Effect mConfig;
    Transform mEffectTf;

    public Effect(short effectGdsId, Transform effectRoot, Vector3 pos)
    {
        iD = ++Count;
        mConfig = GDSKit.Effect.GetInstance(effectGdsId);
        GameMain.Instance.ResMgr.LoadResourceAsync(this, mConfig.prefabName, typeof(GameObject), delegate (UnityEngine.Object prefab)
        {
            mEffectTf = GameObject.Instantiate<GameObject>(prefab as UnityEngine.GameObject).transform;
            mEffectTf.SetParent(effectRoot);
            mEffectTf.Reset();
            mEffectTf.position = pos;
        }, delegate (string error)
        {

        });
    }

    public int iD
    {
        get;
        private set;
    }

    public void Destroy()
    {
        GameMain.Instance.ResMgr.UnloadResource(mConfig.prefabName, typeof(GameObject));
        if(null != mEffectTf)
        {
            GameObject.Destroy(mEffectTf.gameObject);
        }
    }
}
