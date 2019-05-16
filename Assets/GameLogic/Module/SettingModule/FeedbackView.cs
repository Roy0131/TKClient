using Framework.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackView : UIBaseView
{
    private Text _sendName;
    private Button _send;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _sendName = Find<Text>("Send/Text");
        _send = Find<Button>("Send");

        _send.onClick.Add(OnMail);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _sendName.text = LanguageMgr.GetLanguage(5002418);
    }

    private void OnMail()
    {
        string email = "idlebattle@moyuplay.com"; //Email
        string time = TimeHelper.GetTime(HeroDataModel.Instance.mSystemTime);
        string playerName = HeroDataModel.Instance.mHeroInfoData.mHeroName;
        int playerId = HeroDataModel.Instance.mHeroPlayerId;
        int serverId = LoginHelper.ServerID;
        int playerLevel = HeroDataModel.Instance.mHeroInfoData.mLevel;
        int vipLevel = HeroDataModel.Instance.mHeroInfoData.mVipLevel;
        string language = "";
        if (LocalDataMgr.IsChinese)
            language = "中文";
        else
            language = "英文";
        string version = GameConst.Version;
        Uri uri = new Uri(string.Format("mailto:{0}?subject={1}&body={2}", email, "反馈",
            "\n\n\n\n\n\n\n\n\n\n--------------------以下内容请勿修改和删除--------------------\n发送时间:" + time + "\n游戏名:末世战场：放置英雄\n角色名:" + playerName +
            "\nUID:" + playerId + "\n所在服务器:" + serverId + "\n角色等级:" + playerLevel + "\nVIP等级:" + vipLevel + "\n游戏语言:" + language + "\n游戏版本:" + version));
        Application.OpenURL(uri.AbsoluteUri);
    }
}
