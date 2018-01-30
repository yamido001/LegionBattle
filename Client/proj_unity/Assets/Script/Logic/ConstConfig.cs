using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstConfig{

	#if UNITY_STANDALONE_WIN
	public static string LocalResourceUrl = "127.0.0.1:8080";
	#elif UNITY_STANDALONE_OSX
	public static string LocalResourceUrl = "10.0.6.31";
//	public static string LocalResourceUrl = "192.168.1.4";
	#else
	public static string LocalResourceUrl = "192.168.1.9:8080";
	#endif

	public static int MaxHttpTask = 2;

	public const int BattleOneFramemMaxUpdateTimes = 5;
}
