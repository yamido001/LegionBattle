using UnityEngine;
using System.Collections;
using System.IO;


namespace NewFileSystem
{
	public class FileSystemUtils{

		private static string LocalFileRootPath = "FileSystem";
		private static string LocalFileDataRoot = Path.Combine(LocalFileRootPath, NewFileSystem.FileDownloadData.DataFolder);
		public static string GetFileRelativePath(string fileName, string folderPath, bool isActualData = false)
		{
			string relativelyFilePath = string.Empty;
			if (!string.IsNullOrEmpty (folderPath)) {
				relativelyFilePath = Path.Combine (folderPath, fileName);
			} 
			else {
				relativelyFilePath = fileName;
			}
			if (isActualData) {
				relativelyFilePath = Path.Combine (NewFileSystem.FileDownloadData.DataFolder, relativelyFilePath);
			}
			relativelyFilePath = Path.Combine (LocalFileRootPath, relativelyFilePath);
			return relativelyFilePath;
		}

		public static string GetFolderRelativePath(string folerPath)
		{
			string relativelyPath = null;
			if (string.IsNullOrEmpty (folerPath)) {
				relativelyPath = LocalFileDataRoot;
			} 
			else {
				relativelyPath = Path.Combine (LocalFileDataRoot, folerPath); 
			}
			return relativelyPath;
		}

		/// <summary>
		/// Android时返回false，IOS和PC返回true.因为Android下不能同步访问非AssetBundle文件
		/// </summary>
		/// <returns><c>true</c> if can use streaming file; otherwise, <c>false</c>.</returns>
		public static bool CanUseStreamingFile()
		{
			bool ret = false;
			#if UNITY_EDITOR
			ret = true;
			#elif UNITY_ANDROID
			//Android的StreamingAsserts目录下的非AssetBundle不能同步访问，只能使用WWW加载
			ret = false;
			#elif UNITY_IOS
			ret = true;
			#endif
			return ret;
		}
	}
}

