using System.Collections;
using System.Collections.Generic;
namespace GDSKit
{
	public class BattleBuff{
		#region 成员自定义类型
		#endregion

		#region 成员变量
		public short id;
		public byte type;
		public int time;
		public byte effectType;
		public short effectTypeParam;
		public bool needClear;
		public short param1;
		public short param2;
		#endregion


		protected static Dictionary<int, BattleBuff> gdsDic = new Dictionary<int, BattleBuff>();
		protected static List<BattleBuff> gdsList;

		public static void Parse(string content)
		{
			Clear ();
			int headEndIndex = GDSParseUtils.GetCharIndex (content, GDSParseUtils.ObjectSeparator, GDSParseUtils.DataBeginLineIndex);
			if (headEndIndex < 0) {
				Logger.LogError ("数据文件头缺少结束符'\'n");
				return;
			}
			int curIndex = headEndIndex + 1;
			if (content.Length <= curIndex) {
				Logger.LogWarning ("数据内容为空，请注意");
				return;
			}
			GDSParseUtils.ParseLine (delegate() {
				BattleBuff data = new BattleBuff ();
				data.id = GDSParseUtils.ParseShort (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.type = GDSParseUtils.ParseByte (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.time = GDSParseUtils.ParseInt (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.effectType = GDSParseUtils.ParseByte (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.effectTypeParam = GDSParseUtils.ParseShort (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.needClear = GDSParseUtils.ParseBool (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.param1 = GDSParseUtils.ParseShort (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.param2 = GDSParseUtils.ParseShort (content, ref curIndex);
				gdsDic.Add (data.id, data);
			}, content, ref curIndex);
			OutPut ();
		}

		public static BattleBuff GetInstance(int id)
		{
			BattleBuff ret = null;
			if (!gdsDic.TryGetValue (id, out ret)) {
				Logger.LogError ("BattleBuff 未找到,id:" + id);
			}
			return ret;
		}

		public static List<BattleBuff> GetAllList()
		{
			if (null == gdsList) {
				gdsList = new List<BattleBuff> ();
				var gdsEnumerator = gdsDic.GetEnumerator ();
				while (gdsEnumerator.MoveNext ()) {
					gdsList.Add (gdsEnumerator.Current.Value);
				}
			}
			return gdsList;
		}

		public static void Clear ()
		{
			gdsDic.Clear ();	
			if (null != gdsList)
				gdsList.Clear ();
		}

		protected static void OutPut()
		{
			foreach (var item in gdsDic) {
				Logger.LogInfo (Newtonsoft.Json.JsonConvert.SerializeObject (item.Value));
			}
		}

	}
}