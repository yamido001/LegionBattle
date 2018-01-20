namespace GameBattle.LogicLayer.Skill
{
    public class UnitSkillComponent
    {
        UnitBase mUnit;
        SkillItem[] mSkills;
        System.Action<short> mFinishHdl;

        public UnitSkillComponent(UnitBase unitBase)
        {
            mUnit = unitBase;
        }

        public bool IsCasting
        {
            get
            {
                for (int i = 0; i < mSkills.Length; ++i)
                {
                    if (mSkills[i].isCasting)
                        return true;
                }
                return false;
            }
        }

        public short CurCasintSkillId
        {
            get
            {
                for (int i = 0; i < mSkills.Length; ++i)
                {
                    if (mSkills[i].isCasting)
                        return mSkills[i].skillId;
                }
                return BattleConfig.invalidSkillId;
            }
        }

        public void Init(int[] skills)
        {
            mSkills = new SkillItem[skills.Length];
            for (int i = 0; i < skills.Length; ++i)
            {
                mSkills[i] = SkillItemFactory.CreateSkillItem(mUnit, skills[i], OnSkillFinish);
            }
        }

        public void AddSkillListener(System.Action<short> hdlOnFinish)
        {
            mFinishHdl = hdlOnFinish;
        }

        public void RemoveSkillListener()
        {
            mFinishHdl = null;
        }

        public void Update()
        {
            for (int i = 0; i < mSkills.Length; ++i)
            {
                mSkills[i].Update();
            }
        }

        public void UseSkill(short skillId, int targetUnitId, short skillAngle, short skillParam1, short skillParam2)
        {
            if(CurCasintSkillId != BattleConfig.invalidSkillId)
            {
                for (int i = 0; i < mSkills.Length; ++i){
                    if(mSkills[i].isCasting)
                    {
                        mSkills[i].BreakSkill();
                    }
                }
            }
            for (int i = 0; i < mSkills.Length; ++i)
            {
                if (mSkills[i].skillId != skillId)
                    continue;
                mSkills[i].TryUseSkill(targetUnitId, skillAngle, skillParam1, skillParam2);
                break;
            }
        }

        public void Destroy()
        {
            for (int i = 0; i < mSkills.Length; ++i)
            {
                mSkills[i].Destroy();
            }
            mSkills = null;
        }

        void OnSkillFinish(short skillId)
        {
            if (null != mFinishHdl)
                mFinishHdl.Invoke(skillId);
        }
    }
}