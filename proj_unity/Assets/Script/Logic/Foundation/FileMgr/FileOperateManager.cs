using System.IO;
using UnityEngine;
using System.Text;

public class FileOperateManager{

	string mPersistentDataPath = Application.persistentDataPath;

	public void Init()
	{
		
	}



	public string AbsolutePath(string relativePath)
	{
		string ret = Path.Combine(mPersistentDataPath, relativePath);
		return ret;
	}

	public bool IsDirExist(string relativePath)
	{
		return Directory.Exists(AbsolutePath(relativePath));
	}

	public bool IsFileExist(string relativePath)
	{
		return File.Exists(AbsolutePath(relativePath));
	}

	public string ReadAsText(string relativePath)
	{
		return File.ReadAllText(AbsolutePath(relativePath), Encoding.UTF8);
	}

	public byte[] ReadAsBinary(string relativePath)
	{
		return File.ReadAllBytes(AbsolutePath(relativePath));
	}

	public void DeleteIfExist(string relativePath)
	{
		if (IsFileExist(relativePath))
		{
			string absPath = AbsolutePath(relativePath);
			File.Delete(absPath);
		}
	}

	public long GetFileLength(string relativePath)
	{
		string filePath = AbsolutePath (relativePath);
		FileInfo info = new FileInfo (filePath);
		return info.Length;
	}

	public DirectoryInfo GetDirectoryInfo(string relativePath)
	{
		CreateDirIfNotExist(relativePath);
		return new DirectoryInfo(AbsolutePath(relativePath));
	}

	public void CreateDirIfNotExist(string relativePath)
	{
		if (!IsDirExist(relativePath))
		{
			Directory.CreateDirectory(AbsolutePath(relativePath));
		}
	}

	public void WriteTextFile(string relativePath, string fileContent, bool isAppend)
	{
		string path = AbsolutePath(relativePath);
		if (isAppend)
		{
			File.AppendAllText(path, fileContent, Encoding.UTF8);
		}
		else
		{
			File.WriteAllText(path, fileContent, Encoding.UTF8);
		}
	}

	public void WriteBinaryFile(string relativePath, byte[] fileContent)
	{
		string path = AbsolutePath(relativePath);
		File.WriteAllBytes(path, fileContent);
	}

	public void MoveFile(string fromRelativePath, string toRelativePath)
	{
		if (!IsFileExist (fromRelativePath))
			return;
		string fromAbsPath = AbsolutePath (fromRelativePath);
		string toAbsPath = AbsolutePath (toRelativePath);
		File.Move (fromAbsPath, toAbsPath);
	}
}
