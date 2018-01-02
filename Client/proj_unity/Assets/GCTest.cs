using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GCTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		float tShort = 12342545.67f;
		byte[] bytes = new byte[4];
		int index = 0;
		SerializeUtils.WriteFloat (bytes, ref index, tShort);
		index = 0;
		float newShort = SerializeUtils.ReadFloat (bytes, ref index);
		Debug.LogError (newShort);

		Debug.LogError (BitConverter.IsLittleEndian);

		byte[] testShort = new byte[2];
		for (short i = short.MinValue; i <= short.MaxValue; ++i) {
			index = 0;
			SerializeUtils.WriteShort (testShort, ref index, i);
			index = 0;
			short j = SerializeUtils.ReadShort (testShort, ref index);
			if (i != j)
				Debug.LogError ("转换失败 " + i + " " + j);
			if (i == short.MaxValue)
				break;
		}

		/*byte[] bytesBf = null;
		using (var ms = new MemoryStream())
		{
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(ms, 123);
			bf.Serialize(ms, 12.3f);
			bf.Serialize(ms, "abcdef");
			bytesBf = ms.GetBuffer ();
		}

		var mD = new MemoryStream ();
		mD.Write (bytesBf, 0, bytesBf.Length);
		BinaryFormatter tf = new BinaryFormatter ();
		Debug.LogError(tf.Deserialize (mD));
		Debug.LogError(tf.Deserialize (mD));
		Debug.LogError(tf.Deserialize (mD));

		return;*/

		string testStr = "abcdfeg";
		byte[] strBytes = new byte[testStr.Length + 2];
		index = 0;
		SerializeUtils.WriteString (strBytes, ref index, testStr);
		index = 0;
		string testSerStr = SerializeUtils.ReadString (strBytes, ref index);
		Debug.LogError (testStr + "\n" + testSerStr);
	}
	
	// Update is called once per frame
	void Update () {
		int a = 12;
		TestFunc (ref a);
	}

	void TestFunc(ref int value)
	{
		++value;
		if (value > 15) {
			Debug.LogError ("aa");
		}
	}
}
