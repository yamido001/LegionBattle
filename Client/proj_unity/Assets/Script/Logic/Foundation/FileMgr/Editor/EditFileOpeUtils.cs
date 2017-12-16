using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class EditFileOpeUtils{

	public static void RemoveDirectory(DirectoryInfo directoryInfo)
	{
		DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories ();
		for (int i = 0; i < directoryInfos.Length; ++i) {
			RemoveDirectory (directoryInfos [i]);
		}
		FileInfo[] fileInfos = directoryInfo.GetFiles ();
		for (int i = 0; i < fileInfos.Length; ++i) {
			fileInfos [i].Delete ();
		}
		directoryInfo.Delete ();
	}

	public static void ClearDirectory(string path)
	{
		if (Directory.Exists (path)) {
			EditFileOpeUtils.RemoveDirectory (new DirectoryInfo (path));
		}
		Directory.CreateDirectory (path);
	}

	public static void CopyDirectory(DirectoryInfo sourceDirInfo, string targetPath, HashSet<string> filterExtensionSet)
	{
		targetPath = Path.Combine (targetPath, sourceDirInfo.Name);
		if (!Directory.Exists (targetPath)) {
			Directory.CreateDirectory (targetPath);
		}
		CopyFiles (sourceDirInfo, targetPath, filterExtensionSet);
		DirectoryInfo[] directInfos = sourceDirInfo.GetDirectories ();
		for (int i = 0; i < directInfos.Length; ++i) {
			CopyDirectory (directInfos [i], targetPath, filterExtensionSet);
		}
	}

	public static void CopyFiles(DirectoryInfo source, string targetPath, HashSet<string> filterExtensionSet)
	{
		FileInfo[] fileInfos = source.GetFiles ();
		for (int i = 0; i < fileInfos.Length; ++i) {
			if (filterExtensionSet.Contains(fileInfos [i].Extension))
				continue;
			string targetFilePath = Path.Combine (targetPath, fileInfos [i].Name);
			fileInfos[i].CopyTo(targetFilePath);
		}
	}
}
