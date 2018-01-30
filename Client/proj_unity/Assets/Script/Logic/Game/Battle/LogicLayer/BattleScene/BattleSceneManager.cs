using LBMath;

namespace GameBattle.LogicLayer.Scene
{
    public class BattleSceneManager : Singleton<BattleSceneManager>
    {
        public BattleSceneManager()
        {
            SceneSize = new IntVector2(50 * 1000, 50 * 1000);
        }

        public IntVector2 SceneSize
        {
            get;
            private set;
        }
    } 
}


