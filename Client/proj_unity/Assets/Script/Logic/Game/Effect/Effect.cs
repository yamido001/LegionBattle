using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect{

    static int Count = 0;

    GDSKit.Effect mConfig;
    Transform mEffectTf;
    int mTimerId = -1;

    public Effect(short effectGdsId, Transform effectRoot, Vector3 pos, System.Action<int> hdlOnFinish)
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
        if(mConfig.type == (int)EffectType.AutoDestroy)
        {
            mTimerId = TimerManager.Instance.DelayCall((ulong)mConfig.param1, delegate ()
            {
                mTimerId = -1;
                hdlOnFinish.Invoke(iD);
            });
        }
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
        if (-1 != mTimerId)
            TimerManager.Instance.DestroyTimer(mTimerId);
    }
}
