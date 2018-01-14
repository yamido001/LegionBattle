using System.Collections;
using System.Collections.Generic;
namespace GDSKit
{
	public class Effect{
		#region 成员自定义类型
		#endregion

		#region 成员变量
		public short id;
		public short type;
		public string prefabName;
		#endregion


		protected static Dictionary<int, Effect> gdsDic = new Dictionary<int, Effect>();
		protected static List<Effect> gdsList;

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
				Effect data = new Effect ();
				data.id = GDSParseUtils.ParseShort (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.type = GDSParseUtils.ParseShort (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.prefabName = GDSParseUtils.ParseString (content, ref curIndex);
				gdsDic.Add (data.id, data);
			}, content, ref curIndex);
			OutPut ();
		}

		public static Effect GetInstance(int id)
		{
			Effect ret = null;
			if (!gdsDic.TryGetValue (id, out ret)) {
				Logger.LogError ("Effect 未找到,id:" + id);
			}
			return ret;
		}

		public static List<Effect> GetAllList()
		{
			if (null == gdsList) {
				gdsList = new List<Effect> ();
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