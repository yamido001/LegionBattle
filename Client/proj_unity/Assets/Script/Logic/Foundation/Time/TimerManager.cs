using System.Collections;
using System.Collections.Generic;

public class TimerManager : Singleton<TimerManager>{

	List<Timer> mTimerList = new List<Timer>();

	public int DelayCall(ulong delayTimeMs, System.Action hdlOnFinish)
	{
		Timer timer = new Timer (ulong.MaxValue, delayTimeMs);
		timer.hdlOnFinish = hdlOnFinish;
		mTimerList.Add (timer);
		return timer.TimerId;
	}

	public int RepeatCall(ulong lastTimeMs, System.Action hdlOnTick)
	{
		Timer timer = new Timer (0, lastTimeMs);
		timer.hdlOnTick = hdlOnTick;
		mTimerList.Add (timer);
		return timer.TimerId;
	}

	public void Update(ulong timeMs)
	{
		for (int i = 0; i < mTimerList.Count; ++i) {
			Timer timer = mTimerList [i];
			timer.Tick (timeMs);
		}

		int saveIndex = 0;
		for (int i = 0; i < mTimerList.Count; ++i) {
			Timer timer = mTimerList [i];
			if (timer.IsFinish) {
				timer.Destroy ();
			} else {
				mTimerList [saveIndex++] = timer;
			}
		}
		for (int i = mTimerList.Count - 1; i >= saveIndex; --i) {
			mTimerList.RemoveAt (i);
		}
	}

	public void DestroyTimer(int timerId)
	{
		if (timerId < 0)
			return;
		int removeIndex = -1;
		for (int i = 0; i < mTimerList.Count; ++i) {
			if (mTimerList [i].TimerId == timerId)
				removeIndex = i;
		}
		if (removeIndex < 0)
			return;
		mTimerList.RemoveAt (removeIndex);
	}

	public void ClearAllTimer()
	{
		mTimerList.Clear ();
	}
}
