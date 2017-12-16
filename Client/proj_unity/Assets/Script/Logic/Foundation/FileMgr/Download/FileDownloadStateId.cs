using UnityEngine;
using System.Collections;

namespace NewFileSystem
{
	public enum FileDownloadStateId
	{
		Null,
		CheckPersistentInfo,
		CompareVersion,
		LoadStreamingFileList,
		GetServerFileList,
		CompareFileList,
		DownLoadFile,
		WritePersistentVersionFile,
	}
}
