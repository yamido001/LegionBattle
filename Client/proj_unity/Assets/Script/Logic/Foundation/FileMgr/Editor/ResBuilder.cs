using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

/// <summary>
/// 文件列表生成的功能
/// </summary>
public class ResBuilder{

	[MenuItem("工具/資源/生成StreamingAsset目錄下的文件列表")]
	public static void BuildStreamingAssetFolder()
	{
		string relativeTargetPath = NewFileSystem.FileSystemUtils.GetFileRelativePath (string.Empty, string.Empty);
		string absTargetPath = Path.Combine (Application.streamingAssetsPath, relativeTargetPath);


		string relativeSourcePath = NewFileSystem.FileSystemUtils.GetFolderRelativePath (string.Empty);
		string sourceFileRootPath = Path.Combine (Application.streamingAssetsPath, relativeSourcePath);

		ExportFileSystem (false, absTargetPath, sourceFileRootPath);
		AssetDatabase.Refresh ();
	}

	[MenuItem("工具/資源/生成服务器文件")]
	public static void BuildServerFile()
	{
		DirectoryInfo dirInfo = new DirectoryInfo (Application.dataPath);
		string dataSourcePath = Path.Combine (dirInfo.Parent.Parent.FullName, "ResServerSrc");
		string dataTargetPath = Path.Combine (dirInfo.Parent.Parent.FullName, "ResServer");
		dataTargetPath = Path.Combine (dataTargetPath, "Editor");
		ExportFileSystem (true, dataTargetPath, dataSourcePath);
	}

	[MenuItem("工具/資源/把资源推送到服务器上")]
	public static void PushServerFile()
	{
		BuildServerFile ();
		DirectoryInfo dirInfo = new DirectoryInfo (Application.dataPath);
		string sourceFilePath = Path.Combine (dirInfo.Parent.Parent.FullName, "ResServer");
		string targetFilePath = string.Empty;
		#if UNITY_STANDALONE_WIN
		DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
		targetFilePath = Path.Combine(directoryInfo.Parent.Parent.Parent.FullName, "Tools");
		targetFilePath = Path.Combine(targetFilePath, "LegionBattle");
		#elif UNITY_STANDALONE_OSX
		targetFilePath = "/Library/WebServer/Documents/LegionBattle";
		#endif
		EditFileOpeUtils.ClearDirectory (targetFilePath);
		EditFileOpeUtils.CopyDirectory(new DirectoryInfo(sourceFilePath), targetFilePath, new HashSet<string>(){".DS_Store"});
	}

	private static void ExportFileSystem(bool isServerBuild, string targetFileRootPath, string sourceFileRootPath)
	{
		Dictionary<string, NewFileSystem.FileDetailInfo> fileListDic = new Dictionary<string, NewFileSystem.FileDetailInfo> ();
		string targetFileDataFolder = Path.Combine (targetFileRootPath, NewFileSystem.FileDownloadData.DataFolder);
		if (isServerBuild && Directory.Exists (targetFileDataFolder)) {
			DirectoryInfo targetDirInfo = new DirectoryInfo (targetFileDataFolder);
			EditFileOpeUtils.RemoveDirectory (targetDirInfo);
		}
		DirectoryInfo directoryInfo =  Directory.CreateDirectory (sourceFileRootPath);
		SeachDirectoryFIles (fileListDic, directoryInfo, string.Empty, targetFileDataFolder, isServerBuild);

		string fileListContent = NewFileSystem.FileListUtils.FileListToString (fileListDic);
		File.WriteAllText (Path.Combine (targetFileRootPath, NewFileSystem.FileDownloadData.FileListFileName), fileListContent);

		if (isServerBuild) {
			string fileListMd5 = Utils.MD5 (fileListContent);
			File.WriteAllText (Path.Combine (targetFileRootPath, NewFileSystem.FileDownloadData.VersionFileName), fileListMd5);
		}
	}

	private static void SeachDirectoryFIles(Dictionary<string, NewFileSystem.FileDetailInfo> fileListDic, DirectoryInfo directoryInfo, string relativePath, string targetPath, bool isServerBuild)
	{
		FileInfo[] fileInfos = directoryInfo.GetFiles ();
		if (isServerBuild && !Directory.Exists (targetPath)) {
			Directory.CreateDirectory (targetPath);
		}
		for (int i = 0; i < fileInfos.Length; ++i) {
			NewFileSystem.FileDetailInfo detailInfo = new NewFileSystem.FileDetailInfo ();
			FileInfo fileInfo = fileInfos [i];
			if (fileInfo.Name == ".DS_Store")
				continue;
			if (fileInfo.Extension == ".meta")
				continue;
			if (fileInfo.Length == 0) {
				Debug.LogError ("Find file size is zero " + fileInfo.FullName);
				continue;
			}
			byte[] fileContent = File.ReadAllBytes (fileInfo.FullName);
			detailInfo.fileMd5 = Utils.MD5 (fileContent);
			detailInfo.fileName = fileInfo.Name;
			detailInfo.fileLength = fileInfo.Length;
			detailInfo.filePath = relativePath;
			if (fileListDic.ContainsKey (detailInfo.fileName)) {
				Debug.LogError ("Has same name file " + fileInfo.FullName);
			} else {
				fileListDic.Add (detailInfo.fileName, detailInfo);
			}

			if (isServerBuild) {
				string targetFilePath = Path.Combine (targetPath, detailInfo.fileMd5);
				if (File.Exists (targetFilePath)) {
					File.Delete (targetFilePath);
				}
				File.Copy(fileInfo.FullName, targetFilePath);
			}
		}

		DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories ();
		for (int i = 0; i < directoryInfos.Length; ++i) {
			SeachDirectoryFIles (fileListDic, directoryInfos [i]
				, Path.Combine (relativePath, directoryInfos [i].Name)
				, Path.Combine(targetPath, directoryInfos [i].Name)
				, isServerBuild);
		}
	}
}
