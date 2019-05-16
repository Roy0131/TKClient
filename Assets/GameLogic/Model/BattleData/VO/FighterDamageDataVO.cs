using Msg.ClientMessage;
using System.Collections.Generic;

public class FighterDamageDataVO : DataBaseVO
{
    public int mSeatIndex { get; private set; }
    public int mCurHp { get; private set; }
    public int mMaxHp { get; private set; }
   
    public int mEnergy { get; private set; }
    public int mSide { get; private set; }
    public bool mBlCritical { get; private set; }
    public bool mBlBlock { get; private set; }
    public int mAntiType { get; private set; }
    public bool mBlAbsorb { get; private set; }
    public string mSkillHitEffect { get; private set; }
    public string mSkillHitSound { get; private set; }
    public int mShield { get; private set; }

    public bool mBlInjury { set; get; }


    private int _skillId;
    private int _index = 0;
    private List<int> _lstDamages = new List<int>();
    private List<int> _lstIntervalFrames = new List<int>();

    public bool mHasMultiDamage { get; set; }

    public FighterDamageDataVO(int skillId = 0)
    {
        _skillId = skillId;
        mHasMultiDamage = false;
    }

    protected override void OnInitData<T>(T value)
    {
        BattleFighter fighterData = value as BattleFighter;
        mSeatIndex = fighterData.Pos;
        mCurHp = fighterData.HP;
        mMaxHp = fighterData.MaxHP;
        mEnergy = fighterData.Energy;
        mSide = fighterData.Side;
        mBlCritical = fighterData.IsCritical;
        mBlBlock = fighterData.IsBlock;
        //_damage = fighterData.Damage;
        mAntiType = fighterData.AntiType;
        mBlAbsorb = fighterData.IsAbsorb;
        mShield = fighterData.Shield;
    }

    //TODO:兼容多发子弹伤害
    private int _damage;
    public int mDamage
    {
        get { return _damage; }
        set
        {
            _damage = value;
            if (_skillId != 0)
            {
                SkillConfig cfg = GameConfigMgr.Instance.GetSkillConfig(_skillId);
                mSkillHitEffect = cfg.ChaHitEffect;
                mSkillHitSound = cfg.ChaHitSound;
                string[] times = cfg.HitShowTime.Split(',');
                _lstIntervalFrames.Add(0);
                int cf = 0;
                int lf = 0;
                if (times.Length > 1)
                {
                    for (int i = 1; i < times.Length; i++)
                    {
                        int.TryParse(times[i - 1], out lf);
                        int.TryParse(times[i], out cf);
                        _lstIntervalFrames.Add(cf - lf);
                    }
                }
                SpliteDamage(cfg.DamageSplit);
            }
            else
            {
                _lstIntervalFrames.Add(0);
                _lstDamages.Add(mDamage);
                mSkillHitEffect = null;
                mSkillHitSound = null;
            }
            _index = 0;
            //if (_skillId != 0)
            //{
            //    SkillConfig cfg = GameConfigMgr.Instance.GetSkillConfig(_skillId);
            //    SpliteDamage(cfg.DamageSplit);
            //}
        }
    }

    public void SpliteDamage(string damageSplit)
    {
        string[] per = damageSplit.Split(',');
        int total = 0;
        List<int> lstPer = new List<int>();
        int tmpInt = 0;
        int i = 0;
        for (i = 0; i < per.Length; i++)
        {
            int.TryParse(per[i], out tmpInt);
            total += tmpInt;
            lstPer.Add(tmpInt);
        }
        int leftDamage = mDamage;
        float p;
        int td;
        for (i = 0; i < lstPer.Count; i++)
        {
            p = (float)lstPer[i] / (float)total;
            td = (int)(p * leftDamage);
            leftDamage -= td;
            total -= lstPer[i];
            _lstDamages.Add(td);
        }
    }

    public int CurDamage
    {
        get
        {
            if (_index > _lstDamages.Count - 1)
            {
                LogHelper.LogError("error, index was invalid, index:" + _index + ", count:" + _lstDamages.Count);
            }
            return _lstDamages[_index];
        }
    }

    public int NextIntervalTime
    {
        get { return _lstIntervalFrames[_index]; }
    }

    public void DoNextIndex()
    {
        _index++;
    }
    
    public bool BlDamageEnd
    {
        get { return _index >= _lstDamages.Count; }
    }

    public int Index
    {
        get { return _index; }
    }

    public int Total
    {
        get { return _lstDamages.Count; }
    }
}