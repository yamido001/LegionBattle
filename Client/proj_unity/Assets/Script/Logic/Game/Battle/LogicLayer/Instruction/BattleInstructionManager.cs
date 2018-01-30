using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LBCSCommon;

namespace GameBattle.LogicLayer
{
	public class BattleInstructionManager : Singleton<BattleInstructionManager> {

		/// <summary>
		/// 缓存指令，以延迟为代价避免卡顿、抖动现象
		/// </summary>
		const int CacheInstructionFrameCount = 1;
		Queue<OneFrameInstructions> mInstructionQueue = new Queue<OneFrameInstructions> (5);
		OneFrameInstructions mCurNotAllReceiveInstructions = null;
		int mLastInstructionFrameCount = 0;

		public int LatestInstructionFrameCount {
			get {
				if (mLastInstructionFrameCount < CacheInstructionFrameCount)
					return 0;
				return mLastInstructionFrameCount - CacheInstructionFrameCount;
			}
		}

		public void AddInstruction(RpBattleInstructionList rpMessage)
		{
			BattleLogManager.Instance.Log ("ReceIns", GetInstructionLogInfo(rpMessage));
			//Logger.LogWarning ("添加指令: " + rpMessage.FrameCount);
			if (null != mCurNotAllReceiveInstructions) {
				if (rpMessage.FrameCount != mCurNotAllReceiveInstructions.frameCount) {
					Logger.LogError ("等待接受战斗指令，但是确收到了下一帧的数据，为什么呢?新的帧号是:" + rpMessage.FrameCount);
					mInstructionQueue.Enqueue (mCurNotAllReceiveInstructions);
					mCurNotAllReceiveInstructions = null;
				} else {
					mCurNotAllReceiveInstructions.instructionList.AddRange (rpMessage.BattleInstructionList);
					if (rpMessage.IsFrameFinish) {
						mInstructionQueue.Enqueue (mCurNotAllReceiveInstructions);
						mCurNotAllReceiveInstructions = null;
						mLastInstructionFrameCount = rpMessage.FrameCount;
					} else {

					}
				}
			} else {
				if (rpMessage.IsFrameFinish) {
					OneFrameInstructions newFrameInfo = new OneFrameInstructions ();
					newFrameInfo.frameCount = rpMessage.FrameCount;
					newFrameInfo.instructionList = rpMessage.BattleInstructionList;
					mInstructionQueue.Enqueue (newFrameInfo);
					mLastInstructionFrameCount = rpMessage.FrameCount;
				}
				else
				{
					mCurNotAllReceiveInstructions = new OneFrameInstructions ();
					mCurNotAllReceiveInstructions.frameCount = rpMessage.FrameCount;
					mCurNotAllReceiveInstructions.instructionList = rpMessage.BattleInstructionList;
				}
			}
		}

		public OneFrameInstructions GetFrameInstructions()
		{
			if (mInstructionQueue.Count == CacheInstructionFrameCount) {
				return null;
			}
			return mInstructionQueue.Dequeue ();
		}

		string GetInstructionLogInfo(RpBattleInstructionList rpMessage)
		{
			System.Text.StringBuilder logSb = new System.Text.StringBuilder ();
			logSb.Append ("收到数据 " + rpMessage.FrameCount + "\t\t");
			for (int i = 0; i < rpMessage.BattleInstructionList.Count; ++i) {
				var item = rpMessage.BattleInstructionList [i];
				logSb.Append (item.SceneUnitId.ToString() + "\t");
				logSb.Append (item.InstructionType.ToString () + "\t");
				switch (item.InstructionType) {
				case BattleInstructionType.Move:
					BattleMove moveInstruct = item as BattleMove;
					logSb.Append (moveInstruct.MoveAngle);
					break;
				case BattleInstructionType.StopMove:
					break;
				default:
					break;
				}
			}
			return logSb.ToString ();
		}
	}
}