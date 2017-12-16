using System.Collections;
using System.Collections.Generic;
namespace GDSKit
{
	public class SkillConfig{
		#region 成员自定义类型
		#endregion

		#region 成员变量
		public int id;
		public int time;
		public int cd;
		public int attackPercentage;
		#endregion


		protected static Dictionary<int, SkillConfig> gdsDic = new Dictionary<int, SkillConfig>();
		protected static List<SkillConfig> gdsList;

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
				SkillConfig data = new SkillConfig ();
				data.id = GDSParseUtils.ParseInt (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.time = GDSParseUtils.ParseInt (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.cd = GDSParseUtils.ParseInt (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.attackPercentage = GDSParseUtils.ParseInt (content, ref curIndex);
				gdsDic.Add (data.id, data);
			}, content, ref curIndex);
			OutPut ();
		}

		public static SkillConfig GetInstance(int id)
		{
			SkillConfig ret = null;
			if (!gdsDic.TryGetValue (id, out ret)) {
				Logger.LogError ("SkillConfig 未找到,id:" + id);
			}
			return ret;
		}

		public static List<SkillConfig> GetAllList()
		{
			if (null == gdsList) {
				gdsList = new List<SkillConfig> ();
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