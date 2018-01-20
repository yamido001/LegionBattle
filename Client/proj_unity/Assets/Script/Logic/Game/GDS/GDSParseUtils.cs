using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GDSParseUtils{

	public static char SelfDefineBeginChar = '{';
	public static char SelfDefineEndChar = '}';
	public static char ArraySeparatorChar = '|';
	public static char SelfDefineVariableSeparatorChar = '#';
	public static char DataSeparatorChar = ',';
	public static char ObjectSeparator = '\n';
    public static char ObjectSeparator1 = '\r';
	public static int DataBeginLineIndex = 3;

	public static int GetCharIndex(string content, char ch, int count)
	{
		int curIndex = 0;
		while (content.Length > curIndex) {
			if (content [curIndex] == ch) {
				if (--count == 0) {
					return curIndex; 
				}
			}
			curIndex++;
		} 
		return -1;
	}

	public static bool IsCurCharEquals(char ch, string content, ref int curIndex)
	{
		if (content.Length <= curIndex)
			return false;
		if (content [curIndex] == ch) {
			++curIndex;
			return true;
		}
		return false;
	}

	public static void MoveNextVariable(string content, ref int curIndex)
	{
		AssertChar (content, GDSParseUtils.DataSeparatorChar, ref curIndex);
	}

	public static void AssertChar(string content, char ch, ref int curIndex)
	{
		if (content.Length <= curIndex) {
			throw new System.Exception ("字符串长度小于索引:" + curIndex + ",字符串为:" + content);
		}
		if (content [curIndex++] != ch) {
			throw new System.Exception ("字符串索引:" + curIndex + ",内容不为: " + ch + " 错误");
		}
	}

	public static int ParseInt(string content, ref int curIndex)
	{
		string data = ParseStrData (content, ref curIndex);
		return int.Parse(data);
	}

	public static string ParseString(string content, ref int curIndex)
	{
		return ParseStrData(content, ref curIndex);
	}

	public static short ParseShort(string content, ref int curIndex)
	{
		string data = ParseStrData (content, ref curIndex);
		return short.Parse(data);
	}

    public static bool ParseBool(string content, ref int curIndex)
    {
        string data = ParseStrData(content, ref curIndex);
        return bool.Parse(data);
    }

    public static byte ParseByte(string content, ref int curIndex)
    {
        string data = ParseStrData(content, ref curIndex);
        return byte.Parse(data);
    }

	public static string ParseStrData(string content, ref int curIndex)
	{
		int beginIndex = curIndex;
        int lenghtOffset = 0;
		while (content.Length > curIndex++ + 1) {
			char nextCh = content [curIndex];
            if(nextCh == GDSParseUtils.ObjectSeparator1)
            {
                //在windows上发现，换行是/r/n,先把/r过滤掉
                lenghtOffset = -1;
                continue;
            }
			if (nextCh == GDSParseUtils.ArraySeparatorChar ||
				nextCh == GDSParseUtils.SelfDefineVariableSeparatorChar ||
				nextCh == GDSParseUtils.SelfDefineEndChar ||
				nextCh == GDSParseUtils.ObjectSeparator ||
				nextCh == GDSParseUtils.DataSeparatorChar) {
				break;
			}
		}
		return content.Substring (beginIndex, curIndex - beginIndex + lenghtOffset);
	}

	public static void ParseArray(System.Action hdlParse, string content, ref int curIndex)
	{
		do
		{
			hdlParse.Invoke();
		}while(GDSParseUtils.IsCurCharEquals(GDSParseUtils.ArraySeparatorChar, content, ref curIndex) && curIndex < content.Length);
	}

	public static void ParseLine(System.Action hdlParse, string content, ref int curIndex)
	{
		do {
			hdlParse.Invoke();
		} while(GDSParseUtils.IsCurCharEquals (GDSParseUtils.ObjectSeparator, content, ref curIndex) && curIndex < content.Length);
	}
}
