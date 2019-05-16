using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RankItemView : UIBaseView
{
    private Text _rank;
    private Text _grade;
    private Text _name;
    private Text _data1;
    private Text _data2;
    private Text _dataText;
    private Image _avatar;
    private Image _rankImg1;
    private Image _rankImg2;
    private Image _rankImg3;
    private Image _rankIcon0;
    private Image _rankIcon1;
    private Image _rankIcon2;
    private Image _rankIcon3;
    private GameObject _dataImg;
    private GameObject _dataObj;

    private RankItemInfo _rankItemInfo;
    private Dictionary<int, string> _dictData;
    private string _data;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _rank = Find<Text>("Rank/Text");
        _grade = Find<Text>("Grade/Text");
        _name = Find<Text>("Name");
        _data1 = Find<Text>("Data1");
        _data2 = Find<Text>("Data2");
        _dataText = Find<Text>("DataText");
        _avatar = Find<Image>("Avatar");
        _rankImg1 = Find<Image>("Rank/Rank1");
        _rankImg2 = Find<Image>("Rank/Rank2");
        _rankImg3 = Find<Image>("Rank/Rank3");
        _rankIcon0 = Find<Image>("IconObj/Icon0");
        _rankIcon1 = Find<Image>("IconObj/Icon1");
        _rankIcon2 = Find<Image>("IconObj/Icon2");
        _rankIcon3 = Find<Image>("IconObj/Icon3");
        _dataImg = Find("DataImg");
        _dataObj = Find("Data1");
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _rankItemInfo = args[0] as RankItemInfo;
        _data = args[1] as string;
        _dictData = args[2] as Dictionary<int, string>;
        OnRankItem();
        _dataImg.SetActive(int.Parse(args[3].ToString()) == RankTypeConst.ComBat);
        _dataText.gameObject.SetActive(int.Parse(args[3].ToString()) == RankTypeConst.ComBat);
        _data1.gameObject.SetActive(int.Parse(args[3].ToString()) != RankTypeConst.ComBat);
        _data2.gameObject.SetActive(int.Parse(args[3].ToString()) != RankTypeConst.ComBat);
    }

    private void OnRankItem()
    {
        _rank.text = _rankItemInfo.Rank.ToString();
        _grade.text = _rankItemInfo.PlayerLevel.ToString();
        _name.text = _rankItemInfo.PlayerName;
        _data1.text = _data;
        _data2.text = _dictData[_rankItemInfo.PlayerId];
        _dataText.text= _dictData[_rankItemInfo.PlayerId];
        if (_rankItemInfo.PlayerHead > 0)
        {
            //Debuger.Log("head:" + _rankItemInfo.PlayerHead);
            _avatar.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(_rankItemInfo.PlayerHead).Icon);
            ObjectHelper.SetSprite(_avatar,_avatar.sprite);
        }
        else
            _avatar.sprite = null;

        _rankImg1.gameObject.SetActive(_rankItemInfo.Rank == 1);
        _rankImg2.gameObject.SetActive(_rankItemInfo.Rank == 2);
        _rankImg3.gameObject.SetActive(_rankItemInfo.Rank == 3);
        _rank.gameObject.SetActive(_rankItemInfo.Rank > 3);
        _rankIcon0.gameObject.SetActive(_rankItemInfo.Rank > 3);
        _rankIcon1.gameObject.SetActive(_rankItemInfo.Rank == 1);
        _rankIcon2.gameObject.SetActive(_rankItemInfo.Rank == 2);
        _rankIcon3.gameObject.SetActive(_rankItemInfo.Rank == 3);
    }
}
