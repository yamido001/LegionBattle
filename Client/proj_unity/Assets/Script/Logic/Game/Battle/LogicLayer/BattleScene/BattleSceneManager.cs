using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle.LogicLayer.Scene
{
    public class BattleSceneManager : Singleton<BattleSceneManager>
    {
        public BattleSceneManager()
        {
            SceneWidth = 200 * 1000;
            SceneHeight = 200 * 1000;
        }
        
        
        /// <summary>
        /// 场景的宽，对应x轴坐标
        /// </summary>
        /// <value>The width of the scene.</value>
        public int SceneWidth
        {
            get;
            private set;
        }

        /// <summary>
        /// 场景的高，对应y轴坐标
        /// </summary>
        /// <value>The height of the scene.</value>
        public int SceneHeight
        {
            get;
            private set;
        }

    } 
}


