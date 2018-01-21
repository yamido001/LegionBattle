using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;

namespace GameBattle.BattleView{
	public class BattleActorBase
	{
		GameObject mObj;
		Animator mAnimator;
		string mCurAnimation;
        ActorBillBoard mBillBoard;
        Transform mRootTf;

		public int Id {
			get;
			private set;
		}

		public BattleActorBase(int id, int life, IntVector2 initPos)
		{
			Id = id;
            mRootTf = new GameObject().transform;
            mRootTf.name = id.ToString();
            mRootTf.SetParent(BattleActorManager.Instance.ActorRootTf);
            mRootTf.Reset();
            GameMain.Instance.ResMgr.LoadResourceAsync (this, "fab_jaina", typeof(GameObject), delegate(Object obj) {
				mObj = GameObject.Instantiate<GameObject>(obj as GameObject);
				mObj.name = id.ToString();
				mAnimator = mObj.GetComponent<Animator>();
                mObj.transform.SetParent(mRootTf);
                mObj.transform.Reset();
            }, null);
            mBillBoard = new ActorBillBoard();
            mBillBoard.Init(mRootTf, life);
            RefreshPos(initPos, initPos);
        }

		public void Update()
		{
			if (null != mAnimator) {
				AnimatorStateInfo stateInfo = mAnimator.GetCurrentAnimatorStateInfo (0);
				if (!stateInfo.loop && stateInfo.normalizedTime >= 1f) {
					PlayAnimation ("idle");
				}
			}
            mBillBoard.Update();
		}

		public void OnMove(IntVector2 fromPos, IntVector2 toPos)
		{
            RefreshPos(fromPos, toPos);
            if (null != mObj) {
				PlayAnimation ("run");
			}
		}

        public void OnUseSkill(short skillId)
        {
            if (null != mObj)
            {
                PlayAnimation("attack1");
            }
        }

        void RefreshPos(IntVector2 fromPos, IntVector2 toPos)
        {
            Vector3 pos = BattleUtils.LogicPosToScenePos(toPos);
            mRootTf.transform.LookAt(pos);
            mRootTf.transform.position = pos;
        }

		public void OnEnterIdle()
		{
			PlayAnimation ("idle");
		}

        public void OnLifeChg(int curLife)
        {
            mBillBoard.UpdateLife(curLife);
        }

		public void PlayAnimation(string animation)
		{
			if (mCurAnimation == animation)
				return;
			if (null != mAnimator) {
				mCurAnimation = animation;
				mAnimator.Play (animation);
			}
		}

		public void Destroy()
		{
			if (null != mObj) {
				GameObject.DestroyImmediate (mObj);
				mObj = null;
				mAnimator = null;
			}
            mBillBoard.Destroy();
        }
	}
}
