using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


public class UnitySceneExporter{

	public static string SceneConfigPath = Path.Combine (Application.dataPath, "Resources/Scene/Config");
	public static string FileExtension = ".txt";

	[MenuItem("工具/场景/导出场景")]
	public static void Export()
	{
		SceneConfigInfo info = new SceneConfigInfo();
		string curSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;
		info.sceneName = curSceneName;

		GameObject[] sceneObjs = GameObject.FindObjectsOfType<GameObject> ();
		for (int i = 0; i < sceneObjs.Length; ++i) {
			GameObject obj = sceneObjs [i];
			if (obj.transform.parent != null)
				continue;
			PrefabType pbjPrefabType = PrefabUtility.GetPrefabType (obj);
			if (pbjPrefabType == PrefabType.None) {
				continue;
			}
			Object prefab = PrefabUtility.GetPrefabParent (obj);
			if (null != prefab) {
				ScenePrefabInfo prefabInfo = new ScenePrefabInfo ();
				prefabInfo.prefabName = prefab.name;
				prefabInfo.position = obj.transform.localPosition;
				prefabInfo.eularAngles = obj.transform.localEulerAngles;
				prefabInfo.scale = obj.transform.localScale;
				info.prefabList.Add (prefabInfo);
			} 
			else {
				Debug.LogError ("未找到场景中的对象对应的预制体,对象名字：" + obj.name);	
			}
		}

		string configStr = Newtonsoft.Json.JsonConvert.SerializeObject (info);
		string filePath = Path.Combine (SceneConfigPath, curSceneName + FileExtension);
		File.WriteAllText (filePath, configStr);
		AssetDatabase.Refresh ();
	}

	[MenuItem("工具/场景/导入场景")]
	public static void Load()
	{
		string curSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;
		string configFilePath = Path.Combine (SceneConfigPath, curSceneName + FileExtension);
		string fileContent = File.ReadAllText (configFilePath);
		if (string.IsNullOrEmpty (fileContent))
			return;
		string[] allPrefabGuids = AssetDatabase.FindAssets ("t:Prefab", null);
		Dictionary<string, string> allAssetDic = new Dictionary<string, string> ();
		foreach (var fileGuid in allPrefabGuids) {
			string filePath = AssetDatabase.GUIDToAssetPath(fileGuid);
			string fileName = GetFileName(filePath);
			string fileNameNoSuffix = GetFileNameWithNoSuffix(fileName);
			if(allAssetDic.ContainsKey(fileNameNoSuffix))
			{
				Debug.LogError("发现名字相同的预制体:" + fileNameNoSuffix);
				continue;
			}
			allAssetDic.Add(fileNameNoSuffix, filePath);
		}


		SceneConfigInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<SceneConfigInfo> (fileContent);
		for (int i = 0; i < info.prefabList.Count; ++i) {
			ScenePrefabInfo prefabInfo = info.prefabList [i];
			if (!allAssetDic.ContainsKey (prefabInfo.prefabName)) {
				Debug.LogError ("未找到对应的预制体:" + prefabInfo.prefabName);
				continue;
			}
			string prefabPath = allAssetDic [prefabInfo.prefabName];
			GameObject prefabObj = AssetDatabase.LoadAssetAtPath<GameObject> (prefabPath);
			GameObject gameObj = PrefabUtility.InstantiatePrefab (prefabObj) as GameObject;
			gameObj.transform.localScale = prefabInfo.scale;
			gameObj.transform.localPosition = prefabInfo.position;
			gameObj.transform.localEulerAngles = prefabInfo.eularAngles;
		}
	}

	private static string GetFileName(string path)
	{
		string[] pathStrs = path.Split ('/');
		return pathStrs [pathStrs.Length - 1];
	}

	private static string GetFileNameWithNoSuffix(string fileName)
	{
		string[] fileNames = fileName.Split('.');
		return fileNames[0];
	}
}
