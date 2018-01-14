using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameBattle.BattleView
{
    public class ActorBillBoard
    {
        SpriteRenderer mSprRender;
        GameObject mObj;
        float mMaxLifeScale;
        int mMaxLife;
        int mCurLife;

        public void Init(Transform rootTf, int maxLife)
        {
            mMaxLife = maxLife;
            GameMain.Instance.ResMgr.LoadResourceAsync(this, "BattleBillBoard", typeof(GameObject), delegate (UnityEngine.Object obj)
                {
                    mObj = GameObject.Instantiate<GameObject>(obj as GameObject);
                    mObj.SetActive(true);
                    mObj.transform.SetParent(rootTf);
                    mObj.transform.Reset();
                    mObj.transform.localPosition = new Vector3(0f, 3f, 0f);
                    mSprRender = mObj.transform.Find("spr_bg/spr_mid").GetComponent<SpriteRenderer>();
                    SpriteRenderer sprBg = mObj.transform.Find("spr_bg").GetComponent<SpriteRenderer>();
                    mMaxLifeScale = sprBg.sprite.texture.width / mSprRender.sprite.texture.width;
                    RefreshBill();
                }, delegate(string error)
                {
                });
        }

        public void UpdateLife(int life)
        {
            mCurLife = life;
            RefreshBill();
        }
        
        public void Destroy()
        {
            GameMain.Instance.ResMgr.UnloadResource("BattleBillBoard", typeof(GameObject));
            if(null != mObj)
            {
                GameObject.Destroy(mObj);
                mObj = null;
            }
        }

        void RefreshBill()
        {
            if (null == mSprRender)
                return;
            mSprRender.transform.localScale = new Vector3((float)mCurLife / mMaxLife * mMaxLifeScale, 1f, 1f);
        }
    }

}
