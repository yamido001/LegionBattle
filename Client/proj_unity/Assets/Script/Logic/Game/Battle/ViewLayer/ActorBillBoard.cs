using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameBattle.BattleView
{
    public class ActorBillBoard
    {
        SpriteRenderer mSprRender;
        Transform mTf;
        float mMaxLifeScale;
        int mMaxLife;
        int mCurLife;

        public void Init(Transform rootTf, int maxLife)
        {
            mMaxLife = maxLife;
            GameMain.Instance.ResMgr.LoadResourceAsync(this, "BattleBillBoard", typeof(GameObject), delegate (UnityEngine.Object obj)
                {
                    mTf = GameObject.Instantiate<GameObject>(obj as GameObject).transform;
                    mTf.gameObject.SetActive(true);
                    mTf.SetParent(rootTf);
                    mTf.Reset();
                    mTf.localPosition = new Vector3(0f, 3f, 0f);
                    mSprRender = mTf.Find("spr_bg/spr_mid").GetComponent<SpriteRenderer>();
                    SpriteRenderer sprBg = mTf.Find("spr_bg").GetComponent<SpriteRenderer>();
                    mMaxLifeScale = sprBg.sprite.texture.width / mSprRender.sprite.texture.width;
                    RefreshBill();
                }, delegate(string error)
                {
                });
            UpdateLife(maxLife);
        }

        public void UpdateLife(int life)
        {
            mCurLife = life;
            RefreshBill();
        }
        
        public void Update()
        {
            if(null != mTf)
            {
                mTf.forward = -Camera.main.transform.forward;
            }
                
        }

        public void Destroy()
        {
            GameMain.Instance.ResMgr.UnloadResource("BattleBillBoard", typeof(GameObject));
            if(null != mTf)
            {
                GameObject.Destroy(mTf.gameObject);
                mTf = null;
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
