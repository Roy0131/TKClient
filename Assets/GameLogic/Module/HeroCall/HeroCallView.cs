using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroCallView : UIBaseView
{
    private Button _btnCall;
    private Button _btnLeft;
    private Button _btnRight;
    private List<ItemInfo> _RewardItemInfo;
    private Dictionary<int, int> _dictRandomRewards;
    private ItemView _itemView;
    private Text _name;

    private GameObject _doors;
    private GameObject _doorsIns;

    private GameObject[] sprites;
    private int halfSize;
    private Vector2 screenCenterPos;
    private float startAngle = -90;//中间卡牌的角度
    private float deltaAngle = 60;//相邻卡牌的角度差值
    //private float moveSpeed = 0;//移动动画的速度
    private float moveTime = 0.3f;//动画移动时间
    private float deltaScale = 0.6f;
    private float startScale = 1.2f;
    private float deltaAlpha = 0.6f;
    private float startAlpha = 1.0f;
    private Vector3 center = new Vector3(0, 0, 0);//椭圆中心点
    private float A = 400;//long axis
    private float B = 100;//short axis
    private int cardcount;
    private float height = 50.0f;

    private int _selNum = 3;

    private bool isOpen = true;
    private bool isRefresh = false;

    private int _downX = 0;

    private UIEffectView _effect01;
    private UIEffectView _effect02;
    private UIEffectView _effect03;
    private UIEffectView _effect04;
    private UIEffectView _effect05;
    private UIEffectView _effect06;
    private UIEffectView _effectTmp;

    private Dictionary<int, UIEffectView> _dictEffect = new Dictionary<int, UIEffectView>();

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _btnLeft = Find<Button>("Button1");
        _btnRight = Find<Button>("Button2");
        _btnCall = Find<Button>("ButtonSummon");
        _name = Find<Text>("Img/TextTitle");
        _doors = Find("Doors");
        _btnCall.onClick.Add(OnReq);
        _btnLeft.onClick.Add(RightBtnClick);
        _btnRight.onClick.Add(OnLeftBtnClick);

        ColliderHelper.SetButtonCollider(_btnLeft.transform, 90, 100);
        ColliderHelper.SetButtonCollider(_btnRight.transform, 90, 100);

        _effect01 = CreateUIEffect(Find("Doors/Image1/fx_ui_zhaohuan_wangling"), UILayerSort.WindowSortBeginner+2);
        _effect02 = CreateUIEffect(Find("Doors/Image2/fx_ui_zhaohuan_fanpai"), UILayerSort.WindowSortBeginner+2);
        _effect03 = CreateUIEffect(Find("Doors/Image3/fx_ui_zhaohuan_hero"), UILayerSort.WindowSortBeginner+2);
        _effect04 = CreateUIEffect(Find("Doors/Image4/fx_ui_zhaohuan_dongfang"), UILayerSort.WindowSortBeginner+2);
        _effect05 = CreateUIEffect(Find("Doors/Image5/fx_ui_zhaohuan_chuanshuo"), UILayerSort.WindowSortBeginner+2);
        _effect06 = CreateUIEffect(Find("Doors/Image6/fx_ui_zhaohuan_jixie"), UILayerSort.WindowSortBeginner+2);

        _dictEffect.Add(0, _effect01);
        _dictEffect.Add(1, _effect02);
        _dictEffect.Add(2, _effect03);
        _dictEffect.Add(3, _effect04);
        _dictEffect.Add(4, _effect05);
        _dictEffect.Add(5, _effect06);

        _effect04.PlayEffect();

        OnInitPos();

    }

    private void OnSwitchFxState(int value)
    {
        _name.text = GameConst.HeroCallIndex(_selNum);
        for (int i = 0; i < 6; i++)
        {
            if (i != value)
                _dictEffect[i].StopEffect();
            else
                _dictEffect[i].PlayEffect();
        }
    }
    protected override void AddEvent()
    {
        base.AddEvent();
        RecruitDataModel.Instance.AddEvent<int, List<int>, bool>(RecruitEvent.DrawCard, OnInitData);
        InputController.Instance.AddInputEvent(InputEventType.MouseDown, OnMoveDown);
        InputController.Instance.AddInputEvent(InputEventType.MouseUp, OnMoveUp);
        InputController.Instance.AddInputEvent(InputEventType.MouseDrag, OnMoveDrag);
    }
    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        RecruitDataModel.Instance.RemoveEvent<int, List<int>, bool>(RecruitEvent.DrawCard, OnInitData);
        InputController.Instance.RemoveInputEvent(InputEventType.MouseDown, OnMoveDown);
        InputController.Instance.RemoveInputEvent(InputEventType.MouseUp, OnMoveUp);
        InputController.Instance.RemoveInputEvent(InputEventType.MouseDrag, OnMoveDrag);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _name.text = GameConst.HeroCallIndex(_selNum);
    }

    private void OnMoveDown(Vector2 pos)
    {
        _downX = (int)pos.x;
    }

    private void OnMoveUp(Vector2 pos)
    {
        pos = Input.mousePosition;
        if ((int)pos.x - _downX > 100 && isRefresh || (int)pos.x - _downX < -100 && isRefresh)
        {
            if ((int)pos.x - _downX > 0)
                RightBtnClick();
            else
                OnLeftBtnClick();
            isRefresh = false;
        }
    }

    private void OnMoveDrag(Vector2 pos)
    {
        isRefresh = true;
    }

    private void OnReq()
    {
        if (BagDataModel.Instance.GetItemCountById(SpecialItemID.FateKey) == 0)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000130));
            return;
        }
        int id = GameConfigMgr.Instance.GetExtractConfig(100 + _selNum + 1).Id;

        LogHelper.Log("[HeroCallView.OnReq() => id:" + id + "]");
        HeroCallModel.Instance.ReqHeroCall(id);
    }
    private void OnInitData(int drawId, List<int> tabId, bool isFreeDraw)
    {
        //任务奖励显示
        _RewardItemInfo = new List<ItemInfo>();
        _dictRandomRewards = new Dictionary<int, int>();
        CreatRewardItem(RecruitDataModel.Instance.rewardId);
    }
    /// <summary>
    /// 创建奖励
    /// </summary>
    /// <param name="num"></param>
    private void CreatRewardItem(List<int> lstReward)
    {
        List<ItemInfo> _lstitem = new List<ItemInfo>();
        for (int i = 0; i < lstReward.Count; i += 2)
        {
            _dictRandomRewards.Add(lstReward[i], lstReward[i + 1]);
        }
        foreach (KeyValuePair<int, int> kv in _dictRandomRewards)
        {
            ItemInfo info = new ItemInfo();
            info.Id = kv.Key;
            info.Value = kv.Value;
            _lstitem.Add(info);
        }

        GetItemTipMgr.Instance.ShowItemResult(_lstitem);
    }
    private void OnInitPos()
    {
        //_doorsIns = GameObject.Instantiate(_doors);
        //_doorsIns.transform.SetParent(this.mTransform, false);
        //_doorsIns.SetActive(true);

        screenCenterPos = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        cardcount = _doors.transform.childCount;
        if (halfSize % 2 != 0)
            halfSize = (cardcount - 1) / 2;
        else
            halfSize = cardcount / 2;
        sprites = new GameObject[cardcount];
        for (int i = 0; i < cardcount; i++)
        {
            sprites[i] = _doors.transform.GetChild(i).gameObject;
            setPosition(i, false);
            setDeeps(i);
        }
        isOpen = false;
    }
    private void setPosition(int index, bool userTweener = true)
    {
        //计算每一张卡片在椭圆上相对中间卡牌的角度
        float angle = 0;
        float scaleSize = 0;
        float alpha = 0;
        if (index < halfSize)
        {
            angle = startAngle - (halfSize - index) * deltaAngle;
            scaleSize = Mathf.Pow(deltaScale, Mathf.Abs(halfSize - index));
            alpha = Mathf.Pow(deltaAlpha, Mathf.Abs(halfSize - index));
        }
        else if (index > halfSize)
        {

            angle = startAngle + (index - halfSize) * deltaAngle;
            scaleSize = Mathf.Pow(deltaScale, Mathf.Abs(halfSize - index));
            alpha = Mathf.Pow(deltaAlpha, Mathf.Abs(halfSize - index));
        }
        else
        {
            angle = startAngle;
            scaleSize = startScale;
            alpha = startAlpha;

        }

        Image image = sprites[index].GetComponent<Image>();

        //通过卡牌的角度，计算对应的位置
        float xpos = A * Mathf.Cos((angle / 180) * Mathf.PI);
        float ypos = B * Mathf.Sin((angle / 180) * Mathf.PI) + height;
        if (isOpen)
            image.rectTransform.localScale = new Vector2(scaleSize, scaleSize);
        image.color = new Vector4(1, 1, 1, alpha);
        Vector2 pos = new Vector2(xpos, ypos);

        if (!userTweener)
        {
            //.rectTransform.DOLocalMove(new Vector2(pos.x, pos.y), 0f).SetEase(Ease.Linear);
            DGHelper.DoLocalMove(image.transform, new Vector2(pos.x, pos.y), 0f);
        }
        else
        {
            //sprites[index].GetComponent<Image>().rectTransform.DOScale(new Vector2(scaleSize, scaleSize), moveTime).SetEase(Ease.Linear);
            //sprites[index].GetComponent<Image>().rectTransform.DOLocalMove(new Vector2(pos.x, pos.y), moveTime).SetEase(Ease.Linear);
            DGHelper.DoScale(image.rectTransform, new Vector2(scaleSize, scaleSize), moveTime);
            DGHelper.DoLocalMove(image.rectTransform, new Vector2(pos.x, pos.y), moveTime);
        }

    }
    /// <summary>
    /// 计算每一张卡片的层级
    /// </summary>
    /// <param name="index">Index.</param>
    private void setDeeps(int index)
    {
        int deep = 0;
        if (index < halfSize)
        {
            deep = index;
        }
        else if (deep > halfSize)
        {
            deep = sprites.Length - (index + 1);
        }
        else
        {
            deep = halfSize;
        }

        sprites[index].GetComponent<RectTransform>().SetSiblingIndex(deep);
    }
    /// <summary>
    /// 左侧按钮点击，向左移动
    /// </summary>
    public void OnLeftBtnClick()
    {
        int length = sprites.Length;

        GameObject temp = sprites[0];
        for (int i = 0; i < length; i++)
        {
            if (i == length - 1)
                sprites[i] = temp;
            else
                sprites[i] = sprites[i + 1];
        }

        for (int i = 0; i < length; i++)
        {
            setPosition(i);
            setDeeps(i);
        }
        _selNum++;
        if (_selNum > 5)
            _selNum = 0;
        OnSwitchFxState(_selNum);
        _btnRight.interactable = false;
        DelayCall(moveTime, () => _btnRight.interactable = true);
    }

    /// <summary>
    /// 右侧按钮点击，向右移动
    /// </summary>
    public void RightBtnClick()
    {
        int length = sprites.Length;

        GameObject temp = sprites[length - 1];
        for (int i = length - 1; i >= 0; i--)
        {
            if (i == 0)
                sprites[i] = temp;
            else
                sprites[i] = sprites[i - 1];
        }
        for (int i = 0; i < length; i++)
        {
            setPosition(i);
            setDeeps(i);
        }
        _selNum--;
        if (_selNum < 0)
            _selNum = 5;
        OnSwitchFxState(_selNum);
        _btnLeft.interactable = false;
        DelayCall(moveTime, () => _btnLeft.interactable = true);
    }
}

