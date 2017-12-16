using UnityEngine;
using System.Collections;
using System;

namespace NewFileSystem
{
	public class FileOperateUtils{

		public static FileErrorCode TryFileWrite(Action proc, out string errorInfo)
		{
			int ERROR_HANDLE_DISK_FULL = 0x27;
			int ERROR_DISK_FULL = 0x70;
			FileErrorCode ret = FileErrorCode.Null;
			errorInfo = string.Empty;
			try {
				proc.Invoke();
			}
			catch (System.Security.SecurityException e){
				ret = FileErrorCode.WriteFileNoPermission;
				errorInfo = e.ToString ();
			}
			catch (System.UnauthorizedAccessException e){
				ret = FileErrorCode.WriteFileNoPermission;
				errorInfo = e.ToString ();
			}
			catch (System.IO.IOException e){
				var hResult = System.Runtime.InteropServices.Marshal.GetHRForException(e) & 0xFFFF;
				if (hResult == ERROR_DISK_FULL || hResult == ERROR_HANDLE_DISK_FULL)
				{
					ret = FileErrorCode.NoSpace;
				}
				else
				{
					ret = FileErrorCode.Unknown;
				}
				errorInfo = e.ToString ();
			}
			catch (Exception e){
				ret = FileErrorCode.Unknown;
				errorInfo = e.ToString ();
			}
			return ret;
		}
	}
}
