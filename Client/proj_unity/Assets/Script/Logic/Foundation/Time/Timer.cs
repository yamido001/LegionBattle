using System;

public class Timer{

	static int Count = 0;

	ulong triggerTimeMs;
	ulong curTimeMs;
	ulong finishTimeMs;
	ulong intervalMs;

	public Action hdlOnTick;
	public Action hdlOnFinish;

	public Timer(ulong intervalMs, ulong finishTimeMs)
	{
		this.intervalMs = intervalMs;
		this.finishTimeMs = finishTimeMs;
		curTimeMs = 0;
		triggerTimeMs = intervalMs;
		TimerId = ++Count;
	}

	public void Tick(ulong deltaMs)
	{
		curTimeMs += deltaMs;
		if (curTimeMs > finishTimeMs) {
			if (hdlOnFinish != null)
				hdlOnFinish.Invoke ();
			return;
		}

		if (curTimeMs > triggerTimeMs) {
			triggerTimeMs += intervalMs;
			if (hdlOnTick != null)
				hdlOnTick.Invoke ();
		}
	}

	public void Destroy()
	{
		
	}

	public bool IsFinish
	{
		get;
		private set;
	}

	public int TimerId
	{
		get;
		private set;
	}
}
