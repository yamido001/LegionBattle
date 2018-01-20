using GameBattle.LogicLayer.Move;
using LegionBattle.ServerClientCommon;
using GameBattle.LogicLayer.Skill;
using GameBattle.LogicLayer.Buff;

namespace GameBattle{

	namespace LogicLayer
	{
		/// <summary>
		/// 逻辑层战斗基础单位
		/// </summary>
		public class UnitBase{

			UnitAIComponent mAI;

            public UnitSkillComponent skillComp
            {
                get;
                private set;
            }

            public UnitMoveComponent moveComp
            {
                get;
                private set;
            }

            public UnitBuffComponent buffComp
            {
                get;
                private set;
            }

			public UnitConfigData Data {
				get;
				private set;
			}

			public int ID {
				get;
				private set;
			}

			public bool IsDead
			{
				get;
				private set;
			}

			public IntVector2 Position {
				get;
				set;
			}

			int[] mAttributes = new int[(int)FighterAttributeType.Count];

			public void Init(UnitConfigData data)
			{
				Data = data;

				mAttributes [(int)FighterAttributeType.Life] = data.life;
				mAttributes [(int)FighterAttributeType.Speed] = data.speed;
				mAttributes [(int)FighterAttributeType.Attack] = data.attack;
				mAttributes [(int)FighterAttributeType.AttackRange] = data.attackRange;

				Position = Data.borthPos;
				ID = Data.id;
				IsDead = false;

				mAI = new UnitAIComponent (this);
				mAI.Init ();

                skillComp = new UnitSkillComponent(this);
                skillComp.Init((data.skillList));

                moveComp = new UnitMoveComponent();
                moveComp.Init(this);

                buffComp = new UnitBuffComponent(this);
			}

			public void SetBattleInstruction(BattleInstructionBase instruction)
			{
				mAI.SetBattleInstruction (instruction);
			}

            public void EnterIdle()
            {
                Logger.LogInfo("进入到停止状态");
                BattleFiled.Instance.OnUnitEnterIdle(ID);
            }

			public int GetAttribute(FighterAttributeType attrType)
			{
				return mAttributes[(int)attrType];
			}

			public void SetAttribute(FighterAttributeType attrType, int value)
			{
				mAttributes [(int)attrType] = value;
			}

			public void Update()
			{
				if (IsDead) {
					return;
				}
				mAI.Execute ();
                skillComp.Update();
                buffComp.Update();
            }

			public void Destroy()
			{
				mAI.Destroy();
				mAI = null;
                skillComp.Destroy();
                buffComp.Destroy();
			}

			protected bool IsSameCamp(UnitConfigData fighterData)
			{
				return fighterData.camp == Data.camp;
			}

			public void OnDead()
			{
				if (IsDead)
					return;
				IsDead = true;
			}
		}
	}
}
