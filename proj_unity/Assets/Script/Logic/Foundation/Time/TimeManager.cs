using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager{

	public void Init()
	{
		
	}

	public ulong CurTimeMs
	{
		get {
			return (ulong)(Time.realtimeSinceStartup * 1000);
		}
	}

	public ulong SinceTimeMs(ulong lastTimeMs)
	{
		return CurTimeMs >= lastTimeMs ? CurTimeMs - lastTimeMs : 0;
	}

	public float SinceTimeSecond(ulong lastTimeMs)
	{
		return SinceTimeMs(lastTimeMs) / 1000f;
	}
}
