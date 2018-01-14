using System.Collections;
using System.Collections.Generic;
namespace GDSKit
{
	public class SkillEffect{
		#region 成员自定义类型
		#endregion

		#region 成员变量
		public short id;
		public short type;
		public int param1;
		public int param2;
		#endregion


		protected static Dictionary<int, SkillEffect> gdsDic = new Dictionary<int, SkillEffect>();
		protected static List<SkillEffect> gdsList;

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
				SkillEffect data = new SkillEffect ();
				data.id = GDSParseUtils.ParseShort (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.type = GDSParseUtils.ParseShort (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.param1 = GDSParseUtils.ParseInt (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.param2 = GDSParseUtils.ParseInt (content, ref curIndex);
				gdsDic.Add (data.id, data);
			}, content, ref curIndex);
			OutPut ();
		}

		public static SkillEffect GetInstance(int id)
		{
			SkillEffect ret = null;
			if (!gdsDic.TryGetValue (id, out ret)) {
				Logger.LogError ("SkillEffect 未找到,id:" + id);
			}
			return ret;
		}

		public static List<SkillEffect> GetAllList()
		{
			if (null == gdsList) {
				gdsList = new List<SkillEffect> ();
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