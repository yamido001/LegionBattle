using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;

public class BattleLogManager : Singleton<BattleLogManager> {

	string mLogFilePath = null;

	public void Start()
	{
		DirectoryInfo dataDire = new DirectoryInfo (Application.dataPath);
		string folderPath = Path.Combine (dataDire.Parent.FullName, "Log");
		if (!Directory.Exists (folderPath)) {
			Directory.CreateDirectory (folderPath);
		}
		mLogFilePath = Path.Combine(folderPath, System.DateTime.Now.Ticks.ToString ());
	}

	[Conditional("BATTLE_LOG")]
	public void Log(string logTag, string log)
	{
		if (null == mLogFilePath)
			Start ();
		File.AppendAllText (mLogFilePath + logTag + ".txt", log + "\n");
	}
}
