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

		public int Id {
			get;
			private set;
		}

		public BattleActorBase(int id)
		{
			Id = id;
			GameMain.Instance.ResMgr.LoadResourceAsync (this, "jiqiangbing", typeof(GameObject), delegate(Object obj) {
				mObj = GameObject.Instantiate<GameObject>(obj as GameObject);
				mObj.name = id.ToString();
				mAnimator = mObj.GetComponent<Animator>();
			}, null);
		}

		public void Update()
		{
			if (null != mAnimator) {
				AnimatorStateInfo stateInfo = mAnimator.GetCurrentAnimatorStateInfo (0);
				if (!stateInfo.loop && stateInfo.normalizedTime >= 1f) {
					PlayAnimation ("idle");
				}
			}
		}

		public void OnMove(IntVector2 fromPos, IntVector2 toPos)
		{
			if (null != mObj) {
				Vector3 pos = BattleUtils.LogicPosToScenePos (toPos);
				mObj.transform.LookAt (pos);
				mObj.transform.position = pos;
				PlayAnimation ("run");
			}
		}

		public void OnEnterIdle()
		{
			PlayAnimation ("idle");
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
		}
	}
}
