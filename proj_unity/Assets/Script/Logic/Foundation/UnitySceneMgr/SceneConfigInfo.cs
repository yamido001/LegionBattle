using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePrefabInfo
{
	public string prefabName;
	public Vector3 position;
	public Vector3 eularAngles;
	public Vector3 scale;
}

public class SceneConfigInfo{

	public string sceneName;
	public List<ScenePrefabInfo> prefabList = new List<ScenePrefabInfo>();
}


