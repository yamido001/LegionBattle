using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEditor;
using LegionBattle.ServerClientCommon;

public class GenerateTrigonometric{

	delegate float GetValueDelegate(float radians);

	[MenuItem("工具/三角函数表/生成sin函数表")]
	static void OutPutSinTable () {
		GenerateValueTable (delegate(float radians) {
			return Mathf.Sin (radians);
		});
	}

	[MenuItem("工具/三角函数表/生成cos函数表")]
	static void OutPutCosTable(){
		GenerateValueTable (delegate(float radians) {
			return Mathf.Cos (radians);
		});
	}

	static void GenerateValueTable(GetValueDelegate getValueHdl)
	{
		StringBuilder logSb = new StringBuilder ();
		logSb.Append ("\t\tprotected static IntegerFloat[] valueTable = new IntegerFloat[] {");
		for (int i = 0; i < 360; ++i) {
			if (i % 5 == 0)
				logSb.Append ("\n\t\t\t");
			float floatValue = getValueHdl(i * Mathf.Deg2Rad);
			IntegerFloat integerFloat = IntegerFloat.FloatToIntegerFloat(floatValue);
			logSb.Append ("new IntegerFloat(" + integerFloat.x + ", " + integerFloat.y + ")");
			if (i != 360 - 1) {
				logSb.Append (",\t\t");
			}
		}
		logSb.Append ("};\n");
		Debug.LogError (logSb.ToString());
	}
}
