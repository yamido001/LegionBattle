using UnityEngine;
using System.Collections;

namespace NewFileSystem
{
	public enum FileErrorCode
	{
		Null,
		DownLoadFileListError,
		ParseFileListError,
		FileLengthError,
		FileMd5Error,
		DownloadFileError,
		WriteFileNoPermission,
		NoSpace,
		Unknown,
	}
}