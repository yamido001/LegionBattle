using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle{

	namespace LogicLayer
	{
		public class UnitAIComponent {
			
			UnitBase mSelf;

			#region 生命周期
			public UnitAIComponent(UnitBase fighter)
			{
				mSelf = fighter;
			}

			public void Update()
			{
				
			}

			public void Destroy()
			{

			}
			#endregion
		}	
	}
}

