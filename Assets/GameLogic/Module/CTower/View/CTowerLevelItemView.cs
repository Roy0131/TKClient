using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class CTowerLevelItemView : UIBaseView
{

    //显示状态
    enum ItemState
    {
        NoBattle,
        Battle,
        Battled,
        //On =1,//显示可以战斗状态
        //Off = 2,//显示不可以战斗状态
    }
    private GameObject _beforeBattleWindow;
    private int floorIDis;

    private GameObject _levelPic01;
    private Text _floorNum01;
    private GameObject _levelPic02;
    private Text _floorNum02;

    private ItemState _itemState01;
    private ItemState _itemState02;
    private Text _tipsText;

    public Vector2 vecTrans;
    private GameObject _point;

    private UIEffectView _effect1;
    private UIEffectView _effect2;
    public CTowerLevelItemView(int index)
    {
        floorIDis = index;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _levelPic01 = Find("ImageLevel01");
        _floorNum01 = Find<Text>("ImageLevel01/level01");
        _levelPic02 = Find("ImageLevel02");
        _floorNum02 = Find<Text>("ImageLevel02/level02");
        _tipsText = Find<Text>("TextTips");

        _beforeBattleWindow = Find("BeforeBattle");
        _point = Find("Point");
        
        _levelPic01.GetComponent<Button>().onClick.Add(DIsResult01);
        _levelPic02.GetComponent<Button>().onClick.Add(DIsResult02);

        _effect1 = CreateUIEffect(Find("fx_ui_tiangongta"), UILayerSort.WindowSortBeginner + 1);
        _effect2 = CreateUIEffect(Find("fx_ui_tiangongta1"), UILayerSort.WindowSortBeginner + 1);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        ChangeNum();
        ChangeState();
        PicDIs();

       // CreateRole();
    }
    /// <summary>
    /// 爬塔层数文字显示
    /// </summary>
    private void ChangeNum()
    {
        _floorNum01.text = floorIDis.ToString();
        _floorNum02.text = (floorIDis + 1).ToString();
    }

    /// <summary>
    /// 判断显示状态
    /// </summary>
    private void ChangeState()
    {
        int floor = CTowerDataModel.Instance.currTowerID + 1;
        _itemState01 = SetState(floorIDis, floor);
        _itemState02 = SetState(floorIDis+1, floor);
    }
    /// <summary>
    /// 设置state
    /// </summary>
    /// <param name="state"></param>
    private ItemState SetState(int num,int floor)
    {
        if (num > floor)
        {
           return ItemState.NoBattle;
        }
        else if (num < floor)
        {
            return  ItemState.Battled;
        }
        else
        {
            return  ItemState.Battle;
           
        }
    }
    /// <summary>
    /// 点击按钮是进入战斗或者弹出tips
    /// </summary>
    private void DIsResult01()
    {
        DIsResult(_itemState01);
    }
    private void DIsResult02()
    {
        DIsResult(_itemState02);
    }

    private void DIsResult(ItemState _state)
    {
        if (_state == ItemState.Battle)
        {
          
            BeforeBattle();
            return;
        }
        else if (_state == ItemState.Battled)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000123));
            //Debuger.Log("已经通关");
        }
        else
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000124));
            //Debuger.Log("前面关卡尚未通关不能进行爬塔");
        }
    }
    /// <summary>
    /// 图片显示
    /// </summary>
    private void PicDIs()
    {
        Sprite img01 = GameResMgr.Instance.LoadItemIcon("levelicon/panel_ctower_01");//已打过状态图标
        Sprite img02 = GameResMgr.Instance.LoadItemIcon("levelicon/panel_ctower_02");//未打状态图标
        Sprite img03 = GameResMgr.Instance.LoadItemIcon("levelicon/panel_ctower_03");//开启状态图标

        _effect1.StopEffect();
        _effect2.StopEffect();
        if (_itemState01 == ItemState.Battled)
        {
            _levelPic01.GetComponent<Image>().sprite = img01;
        }
        else if(_itemState01 == ItemState.NoBattle)
        {
            _levelPic01.GetComponent<Image>().sprite = img02;
        }
        else
        {
            _levelPic01.GetComponent<Image>().sprite = img03;
            _point.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            _effect1.PlayEffect();
            _point.SetActive(true);
        }

        if (_itemState02 == ItemState.Battled)
        {
            _levelPic02.GetComponent<Image>().sprite = img01;
        }
        else if (_itemState02 == ItemState.NoBattle)
        {
            _levelPic02.GetComponent<Image>().sprite = img02;
        }
        else
        {
            _levelPic02.GetComponent<Image>().sprite = img03;
            _point.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f,267f);
            _effect2.PlayEffect();
            _point.SetActive(true);
        }
    }

    /// <summary>
    /// 战前准备面板
    /// </summary>
    private void BeforeBattle()
    {
        if (BagDataModel.Instance.GetItemCountById(SpecialItemID.CTowerTicket) != 0)
        {
            GameUIMgr.Instance.OpenModule(ModuleID.BeforeBattle);
        }
        else
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000056));
            //Debuger.Log("体力不足，不能进行爬塔");
        }
    }
}
