using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utils{

	public static string MD5(string content)
	{
		return MD5 (System.Text.Encoding.UTF8.GetBytes (content));
	}

	public static string MD5(byte[] content)
	{
		System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider ();
		byte[] hash = md5.ComputeHash (content);
		md5.Clear ();

		System.Text.StringBuilder sb = new System.Text.StringBuilder ();
		for (int i = 0; i < hash.Length; ++i) {
			sb.Append(hash[i].ToString("X2"));
		}
		return sb.ToString ();
	}

	public static string GetFrameAndTime()
	{
		return " Frame:" + Time.frameCount.ToString () + " time:" + Time.realtimeSinceStartup.ToString ();
	}

	private const ulong fileSizeAdvanced = 1024;
	public static string GetStrFileSize(ulong byteSize)
	{
		string ret = string.Empty;
		ulong decimalCount = 2;
		ulong decimalCoefficient = (ulong)Math.Pow(10, decimalCount);

		if (byteSize < fileSizeAdvanced / decimalCoefficient) {
			ret = byteSize.ToString() + "B";
		}
		else {
			string unitStr = string.Empty;
			ulong divisor = 1;
			if (byteSize < fileSizeAdvanced * fileSizeAdvanced / decimalCoefficient) {
				unitStr = "KB";
				divisor = fileSizeAdvanced / decimalCoefficient;
			} 
			else
			{
				unitStr = "MB";
				divisor = fileSizeAdvanced * fileSizeAdvanced / decimalCoefficient;
			}
			ulong valueI = byteSize / divisor;
			ret = (valueI / ((float)decimalCoefficient)).ToString ("F" + decimalCount.ToString ()) + unitStr;
		}
		return ret;
	}

	public static void Assert(bool con, string errorCode)
	{
		if (!con)
			Debug.LogError ("Assert: " + errorCode);
	}

	public static void AddAndReset(Transform parTf, Transform childTf)
	{
		childTf.SetParent (parTf);
		childTf.Reset ();
	}

	public static string CombiePath(params string[] nodeStr)
	{
		string ret = nodeStr [0];
		for (int i = 1; i < nodeStr.Length; ++i) {
			ret = System.IO.Path.Combine (ret, nodeStr [i]);
		}
		return ret;
	}
}
