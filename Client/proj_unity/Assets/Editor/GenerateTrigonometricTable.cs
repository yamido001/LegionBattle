using UnityEngine;
using System.Text;
using UnityEditor;
using LBMath;

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

    [MenuItem("工具/三角函数表/生成atan函数表")]
    static void OutPutArcTanTable(){
        GenerateArcTable(delegate (float floatValue)
        {
            return Mathf.Atan(floatValue);
        });
    }

    static void GenerateArcTable(GetValueDelegate getValueHdl)
    {
        StringBuilder logSb = new StringBuilder();
        logSb.Append("\t\tprotected static short[] valueTable = new short[] {");
        for (int i = 0; i <= 1000; ++i)
        {
            if (i % 30 == 0)
                logSb.Append("\n\t\t\t");
            float floatValue = getValueHdl.Invoke(i / 1000f);
            int intValue = (int)(floatValue * Mathf.Rad2Deg);
            logSb.Append(intValue);
            if (i != 1000)
            {
                logSb.Append(",\t\t");
            }
        }
        logSb.Append("};\n");
        Debug.LogError(logSb.ToString());
    }


	static void GenerateValueTable(GetValueDelegate getValueHdl)
	{
		StringBuilder logSb = new StringBuilder ();
		logSb.Append ("\t\tprotected static IntegerFloat[] valueTable = new IntegerFloat[] {");
		for (int i = 0; i < 360; ++i) {
			if (i % 5 == 0)
				logSb.Append ("\n\t\t\t");
			float floatValue = getValueHdl(i * Mathf.Deg2Rad);
			IntegerFloatXY integerFloat = IntegerFloatXY.FloatToIntegerFloatXY(floatValue);
			logSb.Append ("new IntegerFloat(" + integerFloat.x + ", " + integerFloat.y + ")");
			if (i != 360 - 1) {
				logSb.Append (",\t\t");
			}
		}
		logSb.Append ("};\n");
		Debug.LogError (logSb.ToString());
	}
}
