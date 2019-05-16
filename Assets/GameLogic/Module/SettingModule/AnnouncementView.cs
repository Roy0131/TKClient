using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class AnnouncementView : UIBaseView
{
    private Text _announcement;
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _announcement = Find<Text>("Panel/Text");
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _announcement.text = (LanguageMgr.curLanguage == SystemLanguage.Chinese || LanguageMgr.curLanguage == SystemLanguage.ChineseSimplified) ? GameEntry.mCNAnnouncement : GameEntry.mENAnnouncement;
    }
}
