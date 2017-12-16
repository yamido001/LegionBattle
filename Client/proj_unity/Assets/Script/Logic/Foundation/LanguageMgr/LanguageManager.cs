using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager{

	public void Init()
	{
		
	}

	public string GetLanguage(string key, Dictionary<string, object> replaceDics = null)
	{
		string ret = string.Empty;
		switch (key) {
		case "GameLoading":
			ret = "游戏加载中";
		break;
		default:
			ret = key;
			break;
		}
		return ret;
	}

	public void Destroy()
	{
		
	}
}
