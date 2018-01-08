using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle{
	namespace BattleView
	{
		public class OldUnitActor
		{
			GameObject mObj;
			Animator mAnimator;
			OldBattleUnit mUnit;
			string mCurAnimation;
	
			public OldUnitActor(OldBattleUnit fighter)
			{
				mUnit = fighter;
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
				if (null != mObj) {
					mObj.transform.LookAt (mUnit.Position);
					mObj.transform.position = mUnit.Position;
					PlayAnimation ("run");
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
}
