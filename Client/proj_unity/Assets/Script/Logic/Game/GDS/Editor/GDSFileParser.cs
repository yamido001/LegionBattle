using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDSTools
{
	public class MemberVariablesInfo
	{
		public string type;
		public string name;
		List<MemberVariablesInfo> childInfoList;

		public MemberVariablesInfo()
		{
			IsSelfDefineType = false;
			IsWaitSelfDefineClose = false;
			IsArray = false;
			IsWaitCloseArray = false;
			curChildIndex = 0;
			Depth = 0;
		}

		public List<MemberVariablesInfo> childList {
			get{ 
				return childInfoList;
			}
		}

		public void AddChild(MemberVariablesInfo child)
		{
			if (childInfoList == null)
				childInfoList = new List<MemberVariablesInfo> ();
			childInfoList.Add (child);
			child.Parent = this;
		}

		int curChildIndex = 0;
		public MemberVariablesInfo GetNextChild()
		{
			if (curChildIndex >= childInfoList.Count)
				return null;
			return childInfoList [curChildIndex++];
		}

		public void ForeachChild(System.Action<MemberVariablesInfo> foreachHdl)
		{
			if (childInfoList == null)
				return;
			for(int i = 0; i < childInfoList.Count; ++i)
			{
				foreachHdl.Invoke (childInfoList [i]);
			}
		}

		public bool HasNextChild()
		{
			return curChildIndex < childInfoList.Count;
		}

		public MemberVariablesInfo Parent {
			get;
			set;
		}

		public bool IsSelfDefineType {
			get;
			set;
		}

		public bool IsWaitSelfDefineClose {
			get;
			set;
		}

		public bool IsArray {
			get;
			set;
		}

		public bool IsWaitCloseArray {
			get;
			set;
		}

		protected int Depth {
			get;
			set;
		}

		public override string ToString ()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder ();
			sb.Append (GetIntentaion() + "{\n");
			sb.Append (GetIntentaion() + "\t" + type + (IsArray ? "[]" : "") + "\n");
			sb.Append (GetIntentaion() + "\t"+ name + "\n");
			if (childInfoList != null && childInfoList.Count > 0) {
				sb.Append (GetIntentaion() + "\t" + "{\n");
				for (int i = 0; i < childInfoList.Count; ++i) {
					childInfoList [i].Depth = Depth + 2;
					sb.Append (childInfoList [i].ToString ());
				}
				sb.Append (GetIntentaion() + "\t" + "}\n");
			}
			sb.Append (GetIntentaion() + "}\n");
			return sb.ToString ();
		}

		protected string GetIntentaion()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder ();
			for (int i = 0; i < Depth; ++i) {
				sb.Append ("\t");
			}
			return sb.ToString ();
		}
	}

	public class GDSFileParser{

		enum LineContentType
		{
			VariablesName = 0,
			VariablesType = 1,
			VariablesNote = 2,
		}

		enum ParseNameState{
			
		}

		const char NextLine = '\n';
		const char NextLine2 = '\r';
		const char NextData = ',';


		string mFileContent;
		string mFileName;

		public GDSFileParser(string fileName, string fileContent)
		{
			mFileName = fileName;
			mFileContent = fileContent;
		}

		public List<MemberVariablesInfo> GetMemberVariableInfos()
		{
			List<List<string>> fileDataBlocks = ParseCSV (mFileContent);
			if (null == fileDataBlocks) {
				return null;
			}

			System.Text.StringBuilder sb = new System.Text.StringBuilder ();
			for (int i = 0; i < fileDataBlocks.Count; ++i) {
				List<string> lineDatas = fileDataBlocks [i];
				for (int j = 0; j < lineDatas.Count; ++j) {
					sb.Append (" " + lineDatas[j]);
				}
				sb.Append ("\n");
			}

			List<MemberVariablesInfo> memberVaraiablesList = new List<MemberVariablesInfo> ();
			for (int i = 0; i < fileDataBlocks [(int)LineContentType.VariablesType].Count; ++i) {
				MemberVariablesInfo info = ParseNameAndType (fileDataBlocks[(int)LineContentType.VariablesName][i], fileDataBlocks [(int)LineContentType.VariablesType] [i]);
				memberVaraiablesList.Add (info);
			}
			return memberVaraiablesList;
		}

		MemberVariablesInfo ParseNameAndType(string name, string type)
		{
			//{}表示为对象,[]表示为数组，|表示成员之间的分隔符
			//rewards{info{type|id}|count}[]
			//RewardInfo{Info{int|id}|int}[]
			MemberVariablesInfo info = new MemberVariablesInfo();
			int curIndex = 0;
			ParseName (info, null, name, ref curIndex);
			curIndex = 0;
			ParseType (info, null, type, ref curIndex);
			return info;
		}

		void ParseType(MemberVariablesInfo curInfo, MemberVariablesInfo parent, string typeStr, ref int curIndex)
		{
			bool isCloseWhile = false;
			while (curIndex < typeStr.Length && !isCloseWhile) {
				char curCh = typeStr [curIndex];
				++curIndex;
				switch (curCh) {
				case '{':
					{
						MemberVariablesInfo child = curInfo.GetNextChild ();
						if (null == child) {
							throw new System.Exception ("解析类型出错，发现额外的符号‘{’，请确保类型定义和名字定义一致，类型字符串：" + typeStr);
						}
						ParseType (child, curInfo, typeStr, ref curIndex);
					}
					break;
				case '}':
					{
						MemberVariablesInfo child = parent.GetNextChild ();
						if (null != child) {
							throw new System.Exception ("解析类型出错，符号‘{’之前缺少了成员变量的类型定义，请确保类型定义和名字定义一致，类型字符串：" + typeStr);
						}
						isCloseWhile = true;
					}
					break;
				case '[':
					if (!curInfo.IsArray) {
						throw new System.Exception ("解析类型出错，名称定义是是数组，但是类型定义时却不是数组，类型字符串：" + typeStr);
					}
					break;
				case ']':
					if (typeStr [curIndex - 2] != '[') {
						throw new System.Exception ("解析类型出错，请确保符号'['和']'成对出现，类型字符串：" + typeStr);
					}
					break;
				case '|':
					if (string.IsNullOrEmpty (curInfo.type)) {
						throw new System.Exception ("解析类型出错，成员变量定义的类型为空，类型字符串：" + typeStr);
					}
					MemberVariablesInfo nextInfo = parent.GetNextChild ();
					ParseType (nextInfo, parent, typeStr, ref curIndex);
					if (!parent.HasNextChild ()) {
						isCloseWhile = true;
					}
					break;
				default:
					if (curInfo == null) {
						int a = 12;
						++a;
					}
					curInfo.type += curCh;
					break;
				}
			}
		}

		void ParseName(MemberVariablesInfo curInfo, MemberVariablesInfo parent, string nameStr, ref int curIndex)
		{
			bool isCloseWhile = false;
			while (curIndex < nameStr.Length && !isCloseWhile) {
				char curCh = nameStr [curIndex];
				++curIndex;
				switch (curCh) {
				case '{':
					//代表当前的类型为自定义类型
					if (curInfo.IsSelfDefineType) {
						//重复设置自定义类型
						throw new System.Exception ("解析属性名称时，发现重复设置自定义类型,请检查{符号使用是否合理，字段为:" + nameStr);
					}
					curInfo.IsWaitSelfDefineClose = true;
					curInfo.IsSelfDefineType = true;
					MemberVariablesInfo childInfo = new MemberVariablesInfo ();
					curInfo.AddChild (childInfo);
					ParseName (childInfo, curInfo, nameStr, ref curIndex);
					break;
				case '}':
					//代表当前自定义类型结束
					if (!parent.IsSelfDefineType) {
						//不合法的自定义类型结束符
						throw new System.Exception ("解析属性名称时，非自定义类型不应该设置符号‘}’:" + nameStr);
					}
					if (!parent.IsWaitSelfDefineClose) {
						//
						throw new System.Exception ("解析属性名称时，发现多余的符号‘}’，请确保符号‘{’和‘}’是成对出现的，字段为:" + nameStr);
					}
					if (string.IsNullOrEmpty (curInfo.name)) {
						throw new System.Exception ("解析属性名称时，发现非法的符号‘|’,请确保前一个属性名字设置后再使用符号‘|’，字段为:" + nameStr);
					}
					parent.IsWaitSelfDefineClose = false;
					isCloseWhile = true;
					break;
				case '[':
					if (curInfo.IsArray) {
						//当前字段已经设置过是数组了，重复设置抛出错误
						throw new System.Exception ("解析属性名称时，发现重复设置数组，字段为:" + nameStr);
					}
					curInfo.IsArray = true;
					curInfo.IsWaitCloseArray = true;
					break;
				case ']':
					if (!curInfo.IsWaitCloseArray) {
						//当前字段没有设置过数组开始，非法结束符
						throw new System.Exception ("解析属性名称时，发现数组结束符号前没有开始符号，字段为:" + nameStr);
					}
					char lastCh = nameStr [curIndex - 2];
					if ('[' != lastCh) {
						//'['前面必须为'['
						throw new System.Exception ("解析属性名称时，发现符号']'之前不为'['，字段为:" + nameStr);
					}
					curInfo.IsWaitCloseArray = false;
					break;
				case '|':
					//进入下一个属性的解析
					if (string.IsNullOrEmpty (curInfo.name)) {
						throw new System.Exception ("解析属性名称时，发现非法的符号‘|’,请确保前一个属性名字设置后再使用符号‘|’，字段为:" + nameStr);
					}
					MemberVariablesInfo nextInfo = new MemberVariablesInfo ();
					parent.AddChild (nextInfo);
					ParseName (nextInfo, parent, nameStr, ref curIndex);
					if (!parent.IsWaitCloseArray) {
						isCloseWhile = true;
					}
					break;
				default:
					curInfo.name += curCh;
					break;
				}
			}
		}

		List<List<string>> ParseCSV(string fileContent)
		{
			List<List<string>> ret = new List<List<string>> ();
			List<string> curLine = new List<string> ();
			int curIndex = 0;
			int curDataBeginIndex = -1;
			string data = string.Empty;
			while (curIndex < fileContent.Length) {
				char curChar = fileContent [curIndex];

				switch (curChar) {
				case NextLine:
					if (curDataBeginIndex >= 0) {
						int subStringLength = curIndex - curDataBeginIndex;
						if (curIndex > 0 && fileContent [curIndex - 1] == NextLine2)
							subStringLength -= 1;
						data = fileContent.Substring (curDataBeginIndex, subStringLength);
						curDataBeginIndex = -1;
					} else {
						data = string.Empty;
					}
					if (curLine == null) {
						curLine = new List<string> ();
					}
					curLine.Add (data);
					ret.Add (curLine);
					curLine = null;
					break;
				case NextData:
					if (curDataBeginIndex >= 0) {
						data = fileContent.Substring (curDataBeginIndex, curIndex - curDataBeginIndex);
						curDataBeginIndex = -1;
					} else {
						data = string.Empty;
					}
					if (curLine == null) {
						curLine = new List<string> ();
					}
					curLine.Add (data);
					break;
				default:
					if (curDataBeginIndex < 0) {
						curDataBeginIndex = curIndex;
					}
					break;
				}
				++curIndex;
			}
			if (ret.Count == 0) {
				Debug.LogError (mFileName + " 未解析到数据块，文件为空？");
				return null;
			}
			if (ret.Count < (int)LineContentType.VariablesNote) {
				Debug.LogError (mFileName + " 内容不全");
				return null;
			}
			ret.RemoveAt ((int)LineContentType.VariablesNote);
			for (int i = 1; i < ret.Count; ++i) {
				if (ret [0].Count != ret [i].Count) {
					Debug.LogError (mFileName + " 文件第" + i + "行的数据数量和第一行不一致");
					return null;
				}
			}
			return ret;
		}
	}
}

