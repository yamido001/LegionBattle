using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NewFileSystem
{
	public class FileListUtils{

		public enum FileListParseError
		{
			DetailInfoIsNull,
			DetailInfoElementCountError,
			DetailInfoLengthError,
			FileRepeat,
			Null,
		}

		private const char DetailSeparator = '\n';
		private const char VariableSeparator = ',';

		public static Dictionary<string, FileDetailInfo> StringToFileList(string content, out FileListParseError error)
		{
			error = FileListParseError.Null;
			Dictionary<string, FileDetailInfo> retList = new Dictionary<string, FileDetailInfo> ();
			string[] detailInfos = content.Split (DetailSeparator);
			//因为在序列化时，每一条信息后边都追加了一个DetailSeparator，所以detailInfos数组最后一条回事null
			for (int i = 0; i < detailInfos.Length - 1; ++i) {
				if (string.IsNullOrEmpty (detailInfos [i])) {
					error = FileListParseError.DetailInfoIsNull;
					return null;
				}
				FileDetailInfo info = StringToFileDetailInfo (detailInfos [i], out error);
				if (null == info) {
					return null;
				}
				if (retList.ContainsKey (info.fileName)) {
					error = FileListParseError.FileRepeat;
					return null;
				}
				retList.Add (info.fileName, info);
			}
			return retList;
		}


		public static string FileListToString(Dictionary<string, FileDetailInfo> fileListDic)
		{
			StringBuilder sb = new StringBuilder ();
			var enumerator = fileListDic.GetEnumerator ();
			while (enumerator.MoveNext ()) {
				FileDetailInfo detailInfo = enumerator.Current.Value;
				AppendDetailInfo (sb, detailInfo);
				sb.Append (DetailSeparator);
			}
			return sb.ToString ();
		}

		public static FileErrorCode WriteFileList(Dictionary<string, FileDetailInfo> fileListDic, out string errorStr)
		{
			return FileOperateUtils.TryFileWrite (delegate() {
				string persistentFileListContent = FileListToString (fileListDic);
				string relativePath = FileSystemUtils.GetFileRelativePath (FileDownloadData.FileListFileName, string.Empty);
				GameMain.Instance.FileOperateMgr.CreateDirIfNotExist(System.IO.Path.GetDirectoryName(relativePath));
				GameMain.Instance.FileOperateMgr.WriteTextFile (relativePath, persistentFileListContent, false);
			}, out errorStr);
		}

		public static string DetailInfoToString(FileDetailInfo detailInfo)
		{
			StringBuilder sb = new StringBuilder ();
			AppendDetailInfo (sb, detailInfo);
			sb.Append (DetailSeparator);
			return sb.ToString ();
		}

		private static FileDetailInfo StringToFileDetailInfo(string content, out FileListParseError errorType)
		{
			errorType = FileListParseError.Null;
			string[] detailInfos = content.Split (VariableSeparator);
			if (detailInfos.Length != 4) {
				errorType = FileListParseError.DetailInfoElementCountError;
				return null;
			}
			FileDetailInfo info = new FileDetailInfo ();
			info.fileName = detailInfos[0];
			info.filePath = detailInfos[1];
			info.fileMd5 = detailInfos[2];
			if (!long.TryParse (detailInfos [3], out info.fileLength)) {
				errorType = FileListParseError.DetailInfoLengthError;
				return null;
			}
			return info;
		}

		private static void AppendDetailInfo(StringBuilder sb, FileDetailInfo info)
		{
			sb.Append (info.fileName + VariableSeparator);
			sb.Append (info.filePath + VariableSeparator);
			sb.Append (info.fileMd5 + VariableSeparator);
			sb.Append (info.fileLength);
		}
	}
}