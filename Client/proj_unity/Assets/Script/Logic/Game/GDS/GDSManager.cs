using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDSKit
{
	/// <summary>
	/// 游戏固定代码部分
	/// </summary>
	public partial class GDSManager{

		public void Init()
		{
			
		}

		public void LoadGDS()
		{
			LoadAll ();
		}

		public void DestroyGDS()
		{
			ClearAll ();
		}

		protected string GetFileContent(string fileName)
		{
			return GameMain.Instance.FileMgr.ReadFileAsString (fileName + ".csv");
		}
	}
}