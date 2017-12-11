using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle{
	public class FighterActor {

		GameObject mObj;
		Animator mAnimator;
		Fighter mFighter;
		string mCurAnimation;

		public FighterActor(Fighter fighter)
		{
			mFighter = fighter;
			GameMain.Instance.ResMgr.LoadResourceAsync (this, "jiqiangbing", typeof(GameObject), delegate(Object obj) {
				mObj = GameObject.Instantiate<GameObject>(obj as GameObject);
				mObj.name = fighter.ID.ToString();
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

		public void OnMove()
		{
			Vector3 actorPos;
			actorPos.x = mFighter.Position.x;
			actorPos.y = 0f;
			actorPos.z = mFighter.Position.y;
			if (null != mObj) {
				mObj.transform.LookAt (actorPos);
				mObj.transform.position = actorPos;
			}
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
