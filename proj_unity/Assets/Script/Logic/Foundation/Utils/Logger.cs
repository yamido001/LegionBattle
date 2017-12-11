using System;
using System.Diagnostics;

public class Logger{

	[Conditional("DEBUG_LOG")]
	public static void LogInfo(string log)
	{
		UnityEngine.Debug.Log (log);
	}

	[Conditional("DEBUG_LOG")]
	public static void LogWarning(string log)
	{
		UnityEngine.Debug.LogWarning (log);
	}

	public static void LogError(string log)
	{
		UnityEngine.Debug.LogError (log);
	}

	public static void LogException(Exception ex)
	{
		UnityEngine.Debug.LogException(ex);
	}
}
