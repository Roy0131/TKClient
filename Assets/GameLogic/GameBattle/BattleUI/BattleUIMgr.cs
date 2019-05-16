using System.Collections.Generic;
using UnityEngine;
public class BattleUIMgr
{
    
    public HpBarView CreateFighterHpBar(Fighter fighter)
    {
        HpBarView hpBar = new HpBarView(fighter.mData);
        GameObject hpObject = GameResMgr.Instance.LoadUIObjectSync(SingletonResName.UIHpBar);//ResPoolMgr.Instance.GetBattleUnitObject(BattleUnitType.BattleUI, BattleResName.RES_HPBAR);
        hpBar.SetDisplayObject(hpObject);
        hpBar.Show();
        GameUIMgr.Instance.AddBattleUIToStage(hpBar);
        return hpBar;
    }

    public static HpBarView CreateUIFighterHpbar(int hpPercent, int level, int cardType)
    {
        HpBarView hpbar = new HpBarView();
        GameObject hpObject = GameResMgr.Instance.LoadUIObjectSync(SingletonResName.UIHpBar);//ResPoolMgr.Instance.GetBattleUnitObject(BattleUnitType.BattleUI, BattleResName.RES_HPBAR);
        hpbar.SetDisplayObject(hpObject);
        hpbar.mDisplayObject.SetActive(true);
        hpbar.ShowUIHpbar(hpPercent, level, cardType);
        return hpbar;
    }

    #region create blood view
    private Queue<BloodView> _bloodViewQueue = new Queue<BloodView>();
    public BloodView ShowBlood(Fighter fighter, FighterDamageDataVO value)
    {
        BloodView view = null;
        if (_bloodViewQueue.Count > 0)
            view = _bloodViewQueue.Dequeue();
        if (view == null)
        {
            view = new BloodView();
            view.SetDisplayObject(GameResMgr.Instance.LoadUIObjectSync(SingletonResName.UIBlood));//ResPoolMgr.Instance.GetBattleUnitObject(BattleUnitType.BattleUI, BattleResName.RES_UIBLOOD));
            GameUIMgr.Instance.AddBattleUIToStage(view);
        }
        view.InitData(fighter, value);
        return view;
    }

    public void ReturnBloodView(BloodView view)
    {
        view.Hide();
        _bloodViewQueue.Enqueue(view);
    }
    #endregion

    #region create buff tips
    private Queue<BuffTipsView> _buffTipsPools = new Queue<BuffTipsView>();
    public BuffTipsView CreateBuffTips(StatusConfig cfg)
    {
        BuffTipsView view;
        if (_buffTipsPools.Count > 0)
        {
            view = _buffTipsPools.Dequeue();
        }
        else
        {
            view = new BuffTipsView();
            view.SetDisplayObject(GameResMgr.Instance.LoadUIObjectSync(SingletonResName.UIBuffTips));// ResPoolMgr.Instance.GetBattleUnitObject(BattleUnitType.BattleUI, BattleResName.RES_BUFFTIPS));
            GameUIMgr.Instance.AddBattleUIToStage(view);
        }
        view.Show(cfg);
        return view;
    }

    public void ReturnBuffTips(BuffTipsView view)
    {
        view.Hide();
        _buffTipsPools.Enqueue(view);
    }
    #endregion

    public void Dispose()
    {
        if (_bloodViewQueue != null)
        {
            int len = _bloodViewQueue.Count;
            while (len > 0)
            {
                _bloodViewQueue.Dequeue().Dispose();
                len--;
            }
            _bloodViewQueue.Clear();
            _bloodViewQueue = null;
        }

        if (_buffTipsPools != null)
        {
            while (_buffTipsPools.Count > 0)
                _buffTipsPools.Dequeue().Dispose();
            _buffTipsPools.Clear();
            _buffTipsPools = null;
        }
    }
}