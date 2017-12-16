using System.Collections;
using System.Collections.Generic;
namespace GDSKit
{
	public class BattleTest{
		#region 成员自定义类型
		public class BorthPos
		{
			#region 成员变量
			public int x;
			public int y;
			#endregion
			public static BorthPos Parse(string content, ref int curIndex)
			{
				BorthPos ret = new BorthPos ();
				GDSParseUtils.AssertChar (content, GDSParseUtils.SelfDefineBeginChar, ref curIndex);
				ret.x = GDSParseUtils.ParseInt (content, ref curIndex);
				GDSParseUtils.AssertChar (content, GDSParseUtils.SelfDefineVariableSeparatorChar, ref curIndex);
				ret.y = GDSParseUtils.ParseInt (content, ref curIndex);
				GDSParseUtils.AssertChar (content, GDSParseUtils.SelfDefineEndChar, ref curIndex);
				return ret;
			}
		}

		#endregion

		#region 成员变量
		public int id;
		public int camp;
		public int life;
		public int speed;
		public int attack;
		public int attackRange;
		public BorthPos borthPos;
		public List<int> skillList = new List<int>();
		#endregion


		protected static Dictionary<int, BattleTest> gdsDic = new Dictionary<int, BattleTest>();
		protected static List<BattleTest> gdsList;

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
				BattleTest data = new BattleTest ();
				data.id = GDSParseUtils.ParseInt (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.camp = GDSParseUtils.ParseInt (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.life = GDSParseUtils.ParseInt (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.speed = GDSParseUtils.ParseInt (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.attack = GDSParseUtils.ParseInt (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.attackRange = GDSParseUtils.ParseInt (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.borthPos = BorthPos.Parse (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				GDSParseUtils.ParseArray(delegate() {
					data.skillList.Add(GDSParseUtils.ParseInt (content, ref curIndex));
				}, content, ref curIndex);
				gdsDic.Add (data.id, data);
			}, content, ref curIndex);
			OutPut ();
		}

		public static BattleTest GetInstance(int id)
		{
			BattleTest ret = null;
			if (!gdsDic.TryGetValue (id, out ret)) {
				Logger.LogError ("BattleTest 未找到,id:" + id);
			}
			return ret;
		}

		public static List<BattleTest> GetAllList()
		{
			if (null == gdsList) {
				gdsList = new List<BattleTest> ();
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