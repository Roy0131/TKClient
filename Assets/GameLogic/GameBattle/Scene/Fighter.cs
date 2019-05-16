using Spine;
using UnityEngine;
using System.Collections.Generic;
using Framework.UI;
using System;

public class Fighter : AnimatorFighter
{
    public List<BuffTipsView> mlstBuffTipsView;
    public bool mBlDeath { get; private set; }

    public FighterDataVO mData { get; private set; }
    private HpBarView _hpBar;

    private List<DamageFrameTicker> _lstDamageTicker = new List<DamageFrameTicker>();
    private List<BloodView> _lstShowingBloods = new List<BloodView>();
    private List<EffectBase> _lstEffects = new List<EffectBase>();
    private BuffManager _buffMgr;

    //skillaction  finish
    //bullet
    //bullet
    //{fighter、 fighter、 fighter、 fighter}
    //behit action check idle
    //skill effect effect time,
    //blood effect list
    //buff 
    private string _bornEffect;
    public Fighter(string bornEffect = null)
        : base(BattleUnitType.Fighter)
    {
        _bornEffect = bornEffect;
    }

    protected override void OnInitData<T>(T data)
    {
        mlstBuffTipsView = new List<BuffTipsView>();
        mData = data as FighterDataVO;
        if (mData.mCardConfig == null || string.IsNullOrEmpty(mData.mCardConfig.Model))
        {
            LogHelper.LogError("Fighter.OnInitData() ---> role id:" + mData.mId + " model name invalid");
            return;
        }
        _modelName = mData.mCardConfig.Model;
        mflHeight = mData.mflModelHeight;
        mflWidth = mData.mflModelWidth;
        base.OnInitData(data);
        _blControl = false;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _beHitEffect = new FighterMaterialEffect(_modelObject.transform);
    }

    protected override void OnPlayEnd(TrackEntry entry)
	{
        base.OnPlayEnd(entry);
        if (entry.Animation.Name == ActionName.Hit)
        {
            if (mData.mCurHp < 0)
            {
                DoDeath();
            }  
            else
            {
                PlayAction(ActionName.Idle, true);
            }  
        }else if(entry.Animation.Name == ActionName.Death)
        {
            _blDeathActionEnd = true;
        }else
        {
            if (entry.Animation.Name != ActionName.Idle)
                PlayAction(ActionName.Idle, true);
        }
	}

	public void DoDeath()
    {
        _buffMgr.ClearAllBuff();
        _blPlaying = false;
        PlayAction(ActionName.Death);
        _blDeathActionEnd = false;
    }

    public void DoDamage(FighterDamageDataVO value)
    {
        CreateBlood(value);
        if (value.BlDamageEnd)
            return;
        DamageFrameTicker ticker = new DamageFrameTicker(value, CreateBlood);
        _lstDamageTicker.Insert(0, ticker);
    }

    private bool _blDeathActionEnd = false;
    public override void Update()
    {
        if(mData.mCurHp < 0)
        {
            if(_blDeathActionEnd)
            {
                mBlDeath = true;
                BattleManager.Instance.mBattleScene.RemoveFighter(this);
                return;
            }
        }
        if (_lstDamageTicker.Count > 0)
        {
            for (int i = _lstDamageTicker.Count - 1; i >= 0; i--)
            {
                _lstDamageTicker[i].Update();
                if (_lstDamageTicker[i].mBlEnable)
                {
                    _lstDamageTicker[i].Dispose();
                    _lstDamageTicker.RemoveAt(i);
                }
            }
        }

        if (_lstShowingBloods.Count > 0)
        {
            for (int i = _lstShowingBloods.Count - 1; i >= 0; i--)
            {
                if (_lstShowingBloods[i].mBlMoveEnd)
                {
                    BattleManager.Instance.mBattleUIMgr.ReturnBloodView(_lstShowingBloods[i]);
                    _lstShowingBloods.RemoveAt(i);
                }
            }
        }
        if (_buffMgr != null)
            _buffMgr.Update();
        base.Update();
    }

    private void OnEffectEnd(EffectBase effect)
    {
        _lstEffects.Remove(effect);
    }

    public void CreateBlood(FighterDamageDataVO value)
    {
        if (mData.mCurHp < 0)
            return;
        if (value.CurDamage == 0 && !value.mBlAbsorb)
        {
            value.DoNextIndex();
            return;
        }
        BloodView view = BattleManager.Instance.mBattleUIMgr.ShowBlood(this, value);
        if (view.mCurDamage > 0 && !view.mBlInjury)
        {
            if (view.mCurHp < 0)
                DoDeath();
            else
                PlayAction(ActionName.Hit);
            if (!_blControl)
                _beHitEffect.StartEffect();
            if (!string.IsNullOrEmpty(view.mSkillHitEffect))
            {
                EffectBase effect = EffectMgr.Instance.CreateEffect(view.mSkillHitEffect, mHitWorldPosition, OnEffectEnd);
                bool blHero = BattleDataModel.Instance.IsHeroFighter(mData.mSide);
                Vector3 scale = !blHero ? Vector3.one : new Vector3(-1f, 1f, 1f);
                effect.mUnitRoot.localScale = scale;
                _lstEffects.Add(effect);
            }
            if (!string.IsNullOrEmpty(view.mSkillHitSound))
                SoundMgr.Instance.PlayEffectSound(view.mSkillHitSound);
        }
        RefreshHpAndEnery(view.mCurHp, view.mCurEnergy, view.mCurShield);

        Vector3 pos = mUnitRoot.position;
        pos = new Vector3(pos.x, pos.y + mData.mflModelHeight / 2f + 0.1f, pos.z);
        Vector3 p2 = GameUIMgr.Instance.WorldToUIPoint(pos);
        Vector3 p3 = new Vector3(p2.x, p2.y +  _lstShowingBloods.Count * 40, p2.z);
        view.Play(p3);
        _lstShowingBloods.Insert(0, view);
    }

    public void RefreshHpAndEnery(int hpValue, int energy, int shield)
    {
        mData.RefreshHpAndEnergy(hpValue, energy, shield);
        _hpBar.RefreshHpAndEnergy(hpValue, energy, shield);
    }

    public override void AddToStage(Transform parent)
    {
        Action<EffectBase> OnBornEffectEnd = (eff) => 
        {
            if (eff != null)
                _lstEffects.Remove(eff);
            base.AddToStage(parent);
            mUnitRoot = parent;
            mDefaultPos = parent.localPosition;
            //_modelObject.SetActive(true);
            _container.SetActive(true);

            mUnitRoot.gameObject.SetActive(true);
            SortLayer = (int)(mDefaultPos.y * 10);
            _hpBar = BattleManager.Instance.mBattleUIMgr.CreateFighterHpBar(this);
            _hpBar.UpdatePosition(mDefaultPos);
            _buffMgr = new BuffManager(this);
            PlayAction(ActionName.Idle, true);

            mBlDeath = false;
        };

        _container.layer = GameLayer.NpcLayer;
        _container.SetActive(false);
        if (!string.IsNullOrEmpty(_bornEffect))
        {
            EffectBase eff = EffectMgr.Instance.CreateEffect(_bornEffect, parent.position, OnBornEffectEnd);
            _lstEffects.Add(eff);
        }
        else
        {
            OnBornEffectEnd(null);
        }
    }

    protected override void OnDispose()
    {

        if (_lstDamageTicker != null && _lstDamageTicker.Count > 0)
        {
            for (int i = _lstDamageTicker.Count - 1; i >= 0; i--)
            {
                _lstDamageTicker[i].Dispose();
                _lstDamageTicker.RemoveAt(i);
            }
            _lstDamageTicker.Clear();
        }

        if(_lstShowingBloods != null)
        {
            for (int i = _lstShowingBloods.Count - 1; i >= 0; i--)
                _lstShowingBloods[i].Dispose();
            _lstShowingBloods.Clear();
            _lstShowingBloods = null;
        }
        _lstDamageTicker = null;
        mData = null;
        if (_buffMgr != null)
        {
            _buffMgr.Dispose();
            _buffMgr = null;
        }
        if (_hpBar != null)
        {
            _hpBar.Dispose();
            _hpBar = null;
        }
        base.OnDispose();
    }

    public void AddBuff(BuffDataVO vo)
    {
        if (mData.mCurHp < 0)
            return;
        //Debuger.Log("fighter addBuff, fighter pos:" + mData.PosToString() + ", curHp:" + mData.mCurHp);
        _buffMgr.AddBuff(vo);
    }

    public void RemoveBuff(BuffDataVO vo)
    {
        if (mData.mCurHp < 0)
            return;
        _buffMgr.RemoveBuff(vo);
    }

    public override void UpdatePosition(Vector3 pos)
    {
        base.UpdatePosition(pos);
        if (_hpBar != null)
            _hpBar.UpdatePosition(mUnitRoot.position);
        if (_buffMgr != null)
            _buffMgr.UpdatePosition(mUnitRoot.position);
    }


    private bool _blControl = false;
    public void SetControlStatus(bool blControl)
    {
        if (blControl == _blControl)
            return;
        _blControl = blControl;
        if (_blControl)
        {
            if (mData.mCardConfig.Model != "H1020mr")
                _animator.enabled = false;
            _beHitEffect.SetGrayEffect();
        }
        else
        {
            if (mData == null || mData.mCardConfig.Model != "H1020mr")
                _animator.enabled = true;
            _beHitEffect.SetNormal();
        }
    }

    public void SetAnimatorSpeed(float timeScale = 1f)
    {
        if (_animator == null)
            return;
        _blPlaying = false;
        _animator.state.TimeScale = timeScale;
        if (timeScale == 1f)
            PlayAction(ActionName.Idle, true);
    }

    public override void PlayAction(string action, bool loop = false)
    {
        if (_blControl)
            return;
        base.PlayAction(action, loop);
    }

    public bool FighterAllEffectFinish
    {
        get
        {
            if (mBlDeath)
                return true;
            if (!_blControl && _animator.state.TimeScale > 0.0f)
            {
                if (_animator.AnimationName != ActionName.Idle)
                    return false;
            }
            if (_lstDamageTicker != null && _lstDamageTicker.Count > 0)
                return false;
            if (_lstShowingBloods.Count > 0)
                return false;
            if (_lstEffects.Count > 0)
            {
                for(int i = 0; i < _lstEffects.Count; i++)
                {
                    if (!_lstEffects[i].mBlEffectEnd)
                        return false;
                }
            }
            return true;
        }
    }

    public void SetBuffState(StatusConfig cfg,bool isOn,int value)
    {
        if (isOn)
        {
            _hpBar.OnCreateBuff(cfg,value);
        }
        else
        {
            _hpBar.OnRemoveBuff(cfg,value);
        }
    }
}