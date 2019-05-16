using Framework.UI;
using Msg.ClientMessage;
using UnityEngine;
using UnityEngine.UI;

public class RankView : UILoopBaseView<RankItemInfo>
{
    private Text _rank;
    private Text _grade;
    private Text _name;
    private Text _data1;
    private Text _data2;
    private Text _dataText;
    private Image _avatar;
    private GameObject _rankItemObj;
    private GameObject _dataImg;
    private GameObject _rankImg1;
    private GameObject _rankImg2;
    private GameObject _rankImg3;

    private int _curRankType;
    private RankDataVO _curRankDataVO;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _rank = Find<Text>("RankItemObj/RankItem/Rank/Text");
        _grade = Find<Text>("RankItemObj/RankItem/Grade/Text");
        _name = Find<Text>("RankItemObj/RankItem/Name");
        _data1 = Find<Text>("RankItemObj/RankItem/Data1");
        _data2 = Find<Text>("RankItemObj/RankItem/Data2");
        _dataText = Find<Text>("RankItemObj/RankItem/DataText");
        _avatar = Find<Image>("RankItemObj/RankItem/Avatar");
        _rankItemObj = Find("RankItemObj/Panel_Scroll/KnapsackPanel/RankItem");
        _dataImg = Find("RankItemObj/RankItem/DataImg");
        _rankImg1 = Find("RankItemObj/RankItem/Rank/Rank1");
        _rankImg2 = Find("RankItemObj/RankItem/Rank/Rank2");
        _rankImg3 = Find("RankItemObj/RankItem/Rank/Rank3");
        InitScrollRect("RankItemObj/Panel_Scroll");
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        RankDataModel.Instance.AddEvent<int>(RankEvent.RankRefresh, OnRankRefresh);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        RankDataModel.Instance.RemoveEvent<int>(RankEvent.RankRefresh, OnRankRefresh);
    }

    private void OnRankRefresh(int type)
    {
        _curRankType = type;
        OnItemChange();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
    }

    private void OnItem()
    {
        if (_curRankDataVO.mSelfRank != 0)
            _rank.text = _curRankDataVO.mSelfRank.ToString();
        else
            _rank.text = LanguageMgr.GetLanguage(4000058);
        _grade.text = HeroDataModel.Instance.mHeroInfoData.mLevel.ToString();
        _name.text = HeroDataModel.Instance.mHeroInfoData.mHeroName;
        _data1.text = _curRankDataVO.mData;
        _data2.text = _curRankDataVO.mSelfData;
        _dataText.text = _curRankDataVO.mSelfData;
        if (HeroDataModel.Instance.mHeroInfoData.mIcon > 0)
        {
            _avatar.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(HeroDataModel.Instance.mHeroInfoData.mIcon).Icon);
            ObjectHelper.SetSprite(_avatar,_avatar.sprite);
        }
        _dataImg.SetActive(_curRankType == RankTypeConst.ComBat);
        _dataText.gameObject.SetActive(_curRankType == RankTypeConst.ComBat);
        _data1.gameObject.SetActive(_curRankType != RankTypeConst.ComBat);
        _data2.gameObject.SetActive(_curRankType != RankTypeConst.ComBat);
        _rankImg1.gameObject.SetActive(_curRankDataVO.mSelfRank == 1);
        _rankImg2.gameObject.SetActive(_curRankDataVO.mSelfRank == 2);
        _rankImg3.gameObject.SetActive(_curRankDataVO.mSelfRank == 3);
        _rank.gameObject.SetActive(_curRankDataVO.mSelfRank > 3);
    }

    private void OnItemChange()
    {
        _curRankDataVO = RankDataModel.Instance.GetRankType(_curRankType);
        _lstDatas = _curRankDataVO.mListRankItemInfo;
        _loopScrollRect.ClearCells();
        if (_lstDatas.Count == 0)
            return;
        _loopScrollRect.totalCount = _lstDatas.Count;
        _loopScrollRect.RefillCells();
        OnItem();
    }

    protected override UIBaseView CreateItemView()
    {
        RankItemView item = new RankItemView();
        item.SetDisplayObject(GameObject.Instantiate(_rankItemObj));
        return item;
    }

    protected override void SetItemData(UIBaseView view, int idx)
    {
        view.Show(_lstDatas[idx], _curRankDataVO.mData, _curRankDataVO.mDictData, _curRankType);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
