namespace GDSKit
{
	public partial class GDSManager {

		protected void LoadAll()
		{
			BattleBuff.Parse (GetFileContent ("BattleBuff"));
			BattleTest.Parse (GetFileContent ("BattleTest"));
			Effect.Parse (GetFileContent ("Effect"));
			GameScene.Parse (GetFileContent ("GameScene"));
			SkillConfig.Parse (GetFileContent ("SkillConfig"));
			SkillEffect.Parse (GetFileContent ("SkillEffect"));
		}

		protected void ClearAll()
		{
			BattleBuff.Clear ();
			BattleTest.Clear ();
			Effect.Clear ();
			GameScene.Clear ();
			SkillConfig.Clear ();
			SkillEffect.Clear ();
		}
	}
}