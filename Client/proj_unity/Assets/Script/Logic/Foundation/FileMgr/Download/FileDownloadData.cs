using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace NewFileSystem
{
	public class FileDetailInfo
	{
		public string fileName;
		public string filePath;
		public string fileMd5;
		public long fileLength;

		public bool IsInvalid()
		{
			return string.IsNullOrEmpty (fileName) || string.IsNullOrEmpty(fileMd5) || fileLength <= 0;
		}

		public override string ToString()
		{
			return filePath + "/" + fileName + "," + fileMd5 + "," + fileLength;
		}
	}

	public class ServerVersionInfo
	{
		public string version;
		public string md5;

		public void Clear()
		{
			version = string.Empty;
			md5 = string.Empty;
		}
	}

	public class LocalVersionInfo
	{
		public string version;
		public string persistentMd5;
		public string serverMd5;
		public void Clear()
		{
			version = string.Empty;
			persistentMd5 = string.Empty;
			serverMd5 = string.Empty;
		}
	}

	public class FileDownloadData {

		public const string VersionFileName = "Version.txt";
		public const string FileListFileName = "FileList.txt";
		public const string DataFolder = "Data";

		public LocalVersionInfo localVersionInfo;
		public Dictionary<string, FileDetailInfo> persistentFileListDic;
		public Dictionary<string, FileDetailInfo> streamingFileListDic;

		public ServerVersionInfo serverVersionInfo;
		public string serverRootPath;
		public string serverIp;
		public string serverFileListName;
		public Dictionary<string, FileDetailInfo> serverFileListDic;

		public List<FileDetailInfo> needDownloadSet;

		private string mServerRootPath;
		private string ServerRootUrl
		{
			get{
				if (string.IsNullOrEmpty (mServerRootPath)) {
					mServerRootPath = "http://" + serverIp;
					if (!string.IsNullOrEmpty (serverRootPath)) {
						mServerRootPath += "/" + serverRootPath + "/";
					} 
					else {
						mServerRootPath += "/";
					}
				}
				return mServerRootPath;
			}
		}

		public string ServerFileListPath
		{
			get {
				return ServerRootUrl + serverFileListName;
			}
		}

		public string ServerVersionFilePath
		{
			get{
				return ServerRootUrl + VersionFileName;
			}
		}

		private string mServerDataPath;
		public string ServerDataPath
		{
			get {
				if (string.IsNullOrEmpty (mServerDataPath)) {
					mServerDataPath = ServerRootUrl + DataFolder + "/";
				}
				return mServerDataPath;
			}
		}

		public void Clear()
		{
			localVersionInfo.Clear();
			persistentFileListDic.Clear ();
			if(null != serverVersionInfo)
				serverVersionInfo.Clear ();
			if(null != serverFileListDic)
				serverFileListDic.Clear ();
		}
	}
}