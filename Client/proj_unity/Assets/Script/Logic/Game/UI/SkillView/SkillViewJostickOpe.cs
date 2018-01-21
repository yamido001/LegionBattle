using System;
using UnityEngine;
using GameBattle.LogicLayer;
using GameBattle;

/// <summary>
/// 区域技能的释放按钮和操作逻辑
/// </summary>
public class SkillViewJostickOpe : SkillViewOpe
{
    JostickController mJostickControl;
    GameObject mObj;
    GameObject mObjEffectCircle;
    GameObject mObjEffectRing;
    GDSKit.SkillConfig mSkillConfig;

    public void Init(GDSKit.SkillConfig skillConfig, Transform rootTf)
    {
        mSkillConfig = skillConfig;
        GameMain.Instance.ResMgr.LoadResourceAsync(this, "jostickSkillOpe", typeof(GameObject), delegate (UnityEngine.Object prefab)
            {
                mObj =GameObject.Instantiate<GameObject>(prefab as GameObject);
                mObj.transform.SetParent(rootTf);
                mObj.transform.Reset();
                mJostickControl = new JostickController();
                mJostickControl.Init(mObj.transform, null, delegate()
                {
                    UpdateAreaEffectState();
                    UpdateCastDisEffectState();

                }, delegate()
                    {
                        UpdateAreaEffectState();
                        UpdateAreaEffect();
                        UpdateCastDisEffectState();
                        OperatorManager.Instance.UseAreaSkill(mSkillConfig.id, mJostickControl.movementVector);
                    }
                );
            }, delegate (string errorCode)
            {

            });
        CreateAreaEffect();
        CreateCastDisEffect();
        TimerManager.Instance.RepeatCall(ulong.MaxValue, UpdateAreaEffect);
        TimerManager.Instance.RepeatCall(ulong.MaxValue, UpdateCastDisEffect);
    }

    public void Destroy()
    {
        mJostickControl.Destroy();
        GameMain.Instance.ResMgr.UnloadResource("jostickSkillOpe", typeof(GameObject));
        GameMain.Instance.ResMgr.UnloadResource("mat_circle", typeof(Material));
        if (null != mObjEffectCircle)
            GameObject.Destroy(mObjEffectCircle);
        if (null != mObjEffectRing)
            GameObject.Destroy(mObjEffectRing);
    }

    void CreateAreaEffect()
    {
        GameMain.Instance.ResMgr.LoadResourceAsync(this, "mat_circle", typeof(Material), delegate (UnityEngine.Object matAsset)
        {
            mObjEffectCircle = ModelGenerater.AttachMeshRender(matAsset as Material
                , ModelGenerater.CreateCircle(mSkillConfig.targetInfo.param2 / 1000f));
            UpdateAreaEffect();
            UpdateAreaEffectState();
        }, delegate (string errorCode)
        {

        });
    }

    void UpdateAreaEffect()
    {
        if (null == mObjEffectCircle || null == mJostickControl)
            return;
        UnitBase unitBase = BattleUnitManager.Instance.GetUnitByUnitId(GameMain.Instance.ProxyMgr.Player.PlayerId);
        if (null == unitBase)
            return;
        Vector3 unitPos = BattleUtils.LogicPosToScenePos(unitBase.Position);
        Vector2 opeVector = mJostickControl.movementVector;
        float disPercent = opeVector.magnitude;
        opeVector.Normalize();
        Vector3 effectCenterPos = unitPos + new Vector3(opeVector.x, 0f, opeVector.y) * disPercent * mSkillConfig.distance / 1000f;
        effectCenterPos.y += 0.01f;
        mObjEffectCircle.transform.position = effectCenterPos;
    }

    void CreateCastDisEffect()
    {
        GameMain.Instance.ResMgr.LoadResourceAsync(this, "mat_ring", typeof(Material), delegate (UnityEngine.Object matAsset)
        {
            mObjEffectRing = ModelGenerater.AttachMeshRender(matAsset as Material
                , ModelGenerater.CreateRing(mSkillConfig.distance / 1000f, mSkillConfig.distance / 1000f - 0.1f));
            UpdateCastDisEffect();
            UpdateCastDisEffectState();
        }, delegate (string errorCode)
        {

        });
    }

    void UpdateAreaEffectState()
    {
        if (null != mObjEffectCircle)
            mObjEffectCircle.SetActive(IsShowSkillEffect());
    }

    void UpdateCastDisEffect()
    {
        if (null == mObjEffectRing || null == mJostickControl)
            return;
        UnitBase unitBase = BattleUnitManager.Instance.GetUnitByUnitId(GameMain.Instance.ProxyMgr.Player.PlayerId);
        if (null == unitBase)
            return;
        Vector3 unitPos = BattleUtils.LogicPosToScenePos(unitBase.Position);
        unitPos.y += 0.01f;
        mObjEffectRing.transform.position = unitPos;
    }

    bool IsShowSkillEffect()
    {
        bool isShow = false;
        if (null != mJostickControl && mJostickControl.touchPresent)
            isShow = true;
        return isShow;
    }

    void UpdateCastDisEffectState()
    {
        if (null != mObjEffectRing)
            mObjEffectRing.SetActive(IsShowSkillEffect());
    }
}
