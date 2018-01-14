namespace GDSKit
{
	public partial class GDSManager {

		protected void LoadAll()
		{
			BattleTest.Parse (GetFileContent ("BattleTest"));
			GameScene.Parse (GetFileContent ("GameScene"));
			SkillConfig.Parse (GetFileContent ("SkillConfig"));
			SkillEffect.Parse (GetFileContent ("SkillEffect"));
		}

		protected void ClearAll()
		{
			BattleTest.Clear ();
			GameScene.Clear ();
			SkillConfig.Clear ();
			SkillEffect.Clear ();
		}
	}
}