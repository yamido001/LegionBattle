using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


namespace GDSTools
{
	public class GDSFileCreater{

		string mFileName;
		string mFileContent;

		protected const string UsingStr = "using System.Collections;\nusing System.Collections.Generic;\n";
		protected const string NamespaceStr = "namespace GDSKit\n{\n";
		protected const string SelfClassParseBegin = "GDSParseUtils.AssertChar (content, GDSParseUtils.SelfDefineBeginChar, ref curIndex);\n";
		protected const string SelfClassParseEnd = "GDSParseUtils.AssertChar (content, GDSParseUtils.SelfDefineEndChar, ref curIndex);\n";
		protected const string VariableSeparator = "GDSParseUtils.AssertChar (content, GDSParseUtils.SelfDefineVariableSeparatorChar, ref curIndex);\n";
		protected static string ParseSystemTypeCode(string type)
		{
			string ret = "";
			switch (type) {
			case "int":
				ret = "GDSParseUtils.ParseInt (content, ref curIndex)";
				break;
			case "string":
				ret = "GDSParseUtils.ParseString (content, ref curIndex)";
				break;
			case "short":
				ret = "GDSParseUtils.ParseShort (content, ref curIndex)";
				break;
            case "bool":
                ret = "GDSParseUtils.ParseBool (content, ref curIndex)";
                break;
            case "byte":
                ret = "GDSParseUtils.ParseByte (content, ref curIndex)";
                break;
			default:
				throw new System.NotImplementedException ("需要添加解析类型(" + type + ")的对应解析代码");
			}
			return ret;
		}

		HashSet<string> mCreatedSelfDefClassSet = new HashSet<string>();

		public GDSFileCreater(string fileName, string fileContent)
		{
			mFileName = fileName;
			mFileContent = fileContent;
		}

		public string GetCSFileContent()
		{
			GDSFileParser fileParser = new GDSFileParser (mFileName, mFileContent);
		 	List<MemberVariablesInfo> memberVariablesInfos = fileParser.GetMemberVariableInfos ();

			StringBuilder strSb = new StringBuilder ();

			strSb.Append (UsingStr);
			strSb.Append (NamespaceStr);

			strSb.Append ("\tpublic class " + mFileName + "{\n");

			//自定义类型的class的定义
			strSb.Append ("\t\t#region 成员自定义类型\n");
			for (int i = 0; i < memberVariablesInfos.Count; ++i) {
				MemberVariablesInfo variableInfo = memberVariablesInfos [i];
				GetSelfDefineClassStr (strSb, variableInfo);
			}
			strSb.Append ("\t\t#endregion\n\n");

			//成员变量的定义
			GetVariableDefineStr (strSb, memberVariablesInfos, "\t\t");

			//存储多个数据的字典的定义
			strSb.Append ("\n\n\t\tprotected static Dictionary<int, " + mFileName + "> gdsDic = new Dictionary<int, " + mFileName + ">();\n");
			strSb.Append ("\t\tprotected static List<" + mFileName + "> gdsList;\n\n");

			//成员函数的定义
			GetParseFuncStr(strSb, memberVariablesInfos, mFileName);
			GetInstanceFuncStr (strSb, mFileName);
			GetFindAllFuncStr (strSb, mFileName);
			GetClearFuncStr (strSb);
			GetOutPutFuncStr (strSb);

			strSb.Append ("\t}\n");
			strSb.Append ("}");

			return strSb.ToString();
		}

		void GetSelfDefineClassStr(StringBuilder sb, MemberVariablesInfo variableInfo)
		{
			variableInfo.ForeachChild (delegate(MemberVariablesInfo child) {
				GetSelfDefineClassStr(sb, child);
			});
			if (variableInfo.IsSelfDefineType && !mCreatedSelfDefClassSet.Contains(variableInfo.type)) {
				mCreatedSelfDefClassSet.Add (variableInfo.type);
				sb.Append ("\t\tpublic class " + variableInfo.type + "\n");
				sb.Append ("\t\t{\n");
				//成员变量
				if(variableInfo.childList != null && variableInfo.childList.Count > 0)
					GetVariableDefineStr(sb, variableInfo.childList, "\t\t\t");
				//对应的解析函数
				sb.Append ("\t\t\tpublic static " + variableInfo.type + " Parse(string content, ref int curIndex)\n");
				sb.Append ("\t\t\t{\n");
				sb.Append ("\t\t\t\t" + variableInfo.type + " ret = new " + variableInfo.type + " ();\n" );
				sb.Append ("\t\t\t\t" + SelfClassParseBegin);

				bool isFirstChild = true;
				variableInfo.ForeachChild (delegate(MemberVariablesInfo child) {
					if(!isFirstChild)
					{
						sb.Append("\t\t\t\t" + VariableSeparator);
					}
					isFirstChild = false;
					if(child.IsArray)
					{
						sb.Append("\t\t\t\tGDSParseUtils.ParseArray(delegate() {\n");
					}
					sb.Append("\t\t\t\tret." + child.name + " = ");
					if(child.IsSelfDefineType)
					{
						sb.Append(child.type + ".Parse (content, ref curIndex);\n");
					}
					else
					{
						sb.Append(ParseSystemTypeCode(child.type) + ";\n");
					}

					if(child.IsArray){
						sb.Append("\t\t\t\t}, content, ref curIndex);\n");
					}
				});
				sb.Append ("\t\t\t\t" + SelfClassParseEnd);
				sb.Append ("\t\t\t\treturn ret;\n");
				sb.Append ("\t\t\t}\n");
				//类的结束符号
				sb.Append ("\t\t}\n\n");
			}
		}

		void GetVariableDefineStr(StringBuilder sb, List<MemberVariablesInfo>variables, string indentation)
		{
			sb.Append (indentation + "#region 成员变量\n");
			for (int i = 0; i < variables.Count; ++i) {
				MemberVariablesInfo info = variables [i];
				if (info.IsArray) {
					sb.Append (indentation + "public List<" + info.type + "> " + info.name + " = new List<" + info.type + ">();\n");
				} 
				else {
					sb.Append (indentation + "public " + info.type + " " + info.name + ";\n");
				}
			}
			sb.Append (indentation + "#endregion\n");
		}

		void GetParseFuncStr(StringBuilder sb, List<MemberVariablesInfo> varialbes, string className)
		{
			sb.Append (
				"\t\tpublic static void Parse(string content)\n" +
				"\t\t{\n" +
				"\t\t\tClear ();\n" +
				"\t\t\tint headEndIndex = GDSParseUtils.GetCharIndex (content, GDSParseUtils.ObjectSeparator, GDSParseUtils.DataBeginLineIndex);\n" +
				"\t\t\tif (headEndIndex < 0) {\n" +
				"\t\t\t\tLogger.LogError (\"数据文件头缺少结束符'\\'n\");\n" +
				"\t\t\t\treturn;\n" +
				"\t\t\t}\n" +
				"\t\t\tint curIndex = headEndIndex + 1;\n" +
				"\t\t\tif (content.Length <= curIndex) {\n" +
				"\t\t\t\tLogger.LogWarning (\"数据内容为空，请注意\");\n" +
				"\t\t\t\treturn;\n" +
				"\t\t\t}\n" +
				"\t\t\tGDSParseUtils.ParseLine (delegate() {\n" + 
				"\t\t\t\t" + className + " data = new " + className + " ();\n"
			);
			for(int i = 0; i < varialbes.Count; ++i)
			{
				MemberVariablesInfo info = varialbes [i];
				if (i != 0) {
					sb.Append ("\t\t\t\tGDSParseUtils.MoveNextVariable (content, ref curIndex);\n");
				}
				string parseCode = string.Empty;
				if (info.IsSelfDefineType)
				{
					parseCode = info.type + ".Parse (content, ref curIndex)";
				}
				else
				{
					parseCode = ParseSystemTypeCode(info.type);
				}
				if (info.IsArray) {
					sb.Append ("\t\t\t\tGDSParseUtils.ParseArray(delegate() {\n");
					sb.Append ("\t\t\t\t\tdata." + info.name + ".Add(" + parseCode + ");\n");
					sb.Append ("\t\t\t\t}, content, ref curIndex);\n");
				} 
				else {
					sb.Append ("\t\t\t\tdata." + info.name + " = " + parseCode + ";\n");
				}
			}
			sb.Append (
				"\t\t\t\tgdsDic.Add (data.id, data);\n" +
				"\t\t\t}, content, ref curIndex);\n" +
				"\t\t\tOutPut ();\n" +
				"\t\t}\n\n");
		}

		void GetInstanceFuncStr(StringBuilder sb, string className)
		{
			string funcTemplate = 
				"\t\tpublic static ClassName GetInstance(int id)\n" +
				"\t\t{\n" +
				"\t\t\tClassName ret = null;\n" +
				"\t\t\tif (!gdsDic.TryGetValue (id, out ret)) {\n" +
				"\t\t\t\tLogger.LogError (\"ClassName 未找到,id:\" + id);\n" +
				"\t\t\t}\n" +
				"\t\t\treturn ret;\n" +
				"\t\t}\n\n";
			string funcStr = funcTemplate.Replace ("ClassName", className);
			sb.Append (funcStr);
		}

		void GetFindAllFuncStr(StringBuilder sb, string className)
		{
			sb.Append (
				"\t\tpublic static List<" + className + "> GetAllList()\n" +
				"\t\t{\n" +
				"\t\t\tif (null == gdsList) {\n" +
				"\t\t\t\tgdsList = new List<" + className + "> ();\n" +
				"\t\t\t\tvar gdsEnumerator = gdsDic.GetEnumerator ();\n" +
				"\t\t\t\twhile (gdsEnumerator.MoveNext ()) {\n" +
				"\t\t\t\t\tgdsList.Add (gdsEnumerator.Current.Value);\n" +
				"\t\t\t\t}\n" +
				"\t\t\t}\n" +
				"\t\t\treturn gdsList;\n" +
				"\t\t}\n\n");
		}

		void GetClearFuncStr(StringBuilder sb)
		{
			sb.Append (
				"\t\tpublic static void Clear ()\n" +
				"\t\t{\n" +
				"\t\t\tgdsDic.Clear ();\t\n" +
				"\t\t\tif (null != gdsList)\n" +
				"\t\t\t\tgdsList.Clear ();\n" + 
				"\t\t}\n\n");
		}

		void GetOutPutFuncStr(StringBuilder sb)
		{
			sb.Append (
				"\t\tprotected static void OutPut()\n" +
				"\t\t{\n" +
				"\t\t\tforeach (var item in gdsDic) {\n" +
				"\t\t\t\tLogger.LogInfo (Newtonsoft.Json.JsonConvert.SerializeObject (item.Value));\n" +
				"\t\t\t}\n" +
				"\t\t}\n\n");
		}
	}
}

