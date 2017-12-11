using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ResourceListExporter{

	static HashSet<string> filterFileSuffix = new HashSet<string>(){
		".meta",
		".DS_Store"
	};

	[MenuItem("工具/資源/生成Resource目錄下的资源列表")]
	public static void BuildResourceList()
	{
		ExportResourceList ();
	}

	public static bool ExportResourceList()
	{
		string folderPaht = Path.Combine (Application.dataPath, "Resources");
		Dictionary<System.Type, Dictionary<string, string>> resourceListInfos = new Dictionary<System.Type, Dictionary<string, string>> ();
		bool collectResult = CollectFolderInfo (folderPaht, resourceListInfos);
		if (!collectResult)
			return false;
		string fileContent = Newtonsoft.Json.JsonConvert.SerializeObject (resourceListInfos);
		string resourceListFilePaht = Path.Combine (Path.Combine (Application.dataPath, "Resources"), ResourceManager.ResourceListName);
		File.WriteAllText (resourceListFilePaht, fileContent);
		AssetDatabase.Refresh ();
		return true;
	}

	private static bool CollectFolderInfo(string folderPath, Dictionary<System.Type, Dictionary<string, string>> fileInfos)
	{
		DirectoryInfo direcInfo = new DirectoryInfo (folderPath);
		FileInfo[] files = direcInfo.GetFiles ();
		for (int i = 0; i < files.Length; ++i) {
			FileInfo fileInfo = files [i];
			if (filterFileSuffix.Contains (fileInfo.Extension)) {
				continue;
			}
			string fileFullPath = fileInfo.FullName.Replace ("\\", "/");
			string fileRelativePath = fileFullPath.Replace (Application.dataPath, "Assets");
			UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath (fileRelativePath);
			if (obj == null) {
				Debug.LogError ("发现Unity不识别的文件，路径为: " + fileRelativePath);
				return false;
			} 
			else {
				System.Type fileType = obj.GetType ();
				Dictionary<string, string> fileNameToPathDic = null;
				if (!fileInfos.TryGetValue(fileType, out fileNameToPathDic)) {
					fileNameToPathDic = new Dictionary<string, string> ();
					fileInfos [fileType] = fileNameToPathDic;
				}
				if (fileNameToPathDic.ContainsKey (obj.name)) {
					Debug.LogError ("Find same file name, Paht1:\n" + fileNameToPathDic [obj.name] + "\npath2:\n" + fileRelativePath);
					return false;
				}else {
					string fileFinalRelativePath = fileRelativePath.Replace(fileInfo.Extension, string.Empty);
					fileFinalRelativePath = fileFinalRelativePath.Replace ("Assets/Resources/", string.Empty);
					fileNameToPathDic [obj.name] = fileFinalRelativePath;
				}
			}
		}

		DirectoryInfo[] childDirecInfos = direcInfo.GetDirectories ();
		for (int i = 0; i < childDirecInfos.Length; ++i) {
			bool ret = CollectFolderInfo (childDirecInfos [i].FullName, fileInfos);
			if (!ret)
				return false;
		}
		return true;
	}
}
