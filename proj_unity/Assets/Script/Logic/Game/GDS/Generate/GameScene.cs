using System.Collections;
using System.Collections.Generic;
namespace GDSKit
{
	public class GameScene{
		#region 成员自定义类型
		#endregion

		#region 成员变量
		public int id;
		public string unitySceneId;
		#endregion


		protected static Dictionary<int, GameScene> gdsDic = new Dictionary<int, GameScene>();
		protected static List<GameScene> gdsList;

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
				GameScene data = new GameScene ();
				data.id = GDSParseUtils.ParseInt (content, ref curIndex);
				GDSParseUtils.MoveNextVariable (content, ref curIndex);
				data.unitySceneId = GDSParseUtils.ParseString (content, ref curIndex);
				gdsDic.Add (data.id, data);
			}, content, ref curIndex);
			OutPut ();
		}

		public static GameScene GetInstance(int id)
		{
			GameScene ret = null;
			if (!gdsDic.TryGetValue (id, out ret)) {
				Logger.LogError ("GameScene 未找到,id:" + id);
			}
			return ret;
		}

		public static List<GameScene> GetAllList()
		{
			if (null == gdsList) {
				gdsList = new List<GameScene> ();
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