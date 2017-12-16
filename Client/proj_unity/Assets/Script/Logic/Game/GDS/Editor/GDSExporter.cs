using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

namespace GDSTools
{
	public class GDSExporter{
		[MenuItem("工具/导出GDS文件")]
		public static void ExportGDS()
		{
			string unityAssetsPath = Application.dataPath;
			DirectoryInfo unityAssetDirectory = new DirectoryInfo (unityAssetsPath);
			string sourceFileFolderPath = Path.Combine (unityAssetDirectory.Parent.Parent.FullName, "ResServerSrc/GDSFile");
			string targetFileFolderPath = Path.Combine (unityAssetsPath, "Script/Logic/Game/GDS/Generate");
			HashSet<string> filterFileExtensionSet = new HashSet<string>{ ".DS_Store"};


			if (!Directory.Exists (targetFileFolderPath)) {
				Directory.CreateDirectory (targetFileFolderPath);
			}

			HashSet<string> fileSet = new HashSet<string> ();

			DirectoryInfo fileFolderInfo = new DirectoryInfo (sourceFileFolderPath);
			FileInfo[] gdsFiles = fileFolderInfo.GetFiles ();
			for (int i = 0; i < gdsFiles.Length; ++i) {
				FileInfo fileInfo = gdsFiles [i];
				if (filterFileExtensionSet.Contains (fileInfo.Extension))
					continue;
				string fileName = fileInfo.Name.Replace (fileInfo.Extension, string.Empty);
				fileSet.Add (fileName);
				string codeFileContent = Export (fileInfo);
				string codeFilePath = Path.Combine (targetFileFolderPath, fileName + ".cs");
				File.WriteAllText (codeFilePath, codeFileContent);
			}

			string gdsMgrContent = GenerateMgr (fileSet);
			string gdsMgrFilePath = Path.Combine(targetFileFolderPath, "GDSMgrGenerated.cs");
			File.WriteAllText (gdsMgrFilePath, gdsMgrContent);

			AssetDatabase.Refresh ();
		}

		protected static string Export(FileInfo fileInfo)
		{
			string gdsFileContent = File.ReadAllText (fileInfo.FullName);
			GDSFileCreater fileCreate = new GDSFileCreater (fileInfo.Name.Replace(fileInfo.Extension, string.Empty), gdsFileContent);
			return fileCreate.GetCSFileContent ();
		}

		protected static string GenerateMgr(HashSet<string> fileSet)
		{
			
			StringBuilder sb = new StringBuilder ();
			sb.Append ("namespace GDSKit\n{\n" +
						"\tpublic partial class GDSManager {\n\n" +
						"\t\tprotected void LoadAll()\n" +
						"\t\t{\n");
			foreach (var fileName in fileSet) {
				sb.Append ("\t\t\t" + fileName + ".Parse (GetFileContent (\"" + fileName + "\"));\n");
			}

			sb.Append ("\t\t}\n\n" +
						"\t\tprotected void ClearAll()\n" +
						"\t\t{\n");
			foreach (var fileName in fileSet) {
				sb.Append ("\t\t\t" + fileName + ".Clear ();\n");
			}
			sb.Append ("\t\t}\n" +
						"\t}\n" +
						"}");
			return sb.ToString ();
		}
	}
}
