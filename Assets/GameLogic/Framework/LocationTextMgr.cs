using UnityEngine;
using System.Collections.Generic;

public class LocationMgr : Singleton<LocationMgr>
{
    private List<LocationText> _lstAllLoctionTxt = new List<LocationText>();

    public void Init()
    {
        _lstAllLoctionTxt = new List<LocationText>();
        GameDriver.Instance.mPluginDispather.AddEvent<LocationText>(LocationText.LocationTextAwake, RegisteText);
        GameDriver.Instance.mPluginDispather.AddEvent<LocationText>(LocationText.LocationTextDestroy, UnRegistText);
    }

    public void SwitchLanguage(SystemLanguage language)
    {
        if (LocalDataMgr.CurLanguage == language)
            return;
        //try
        //{
        //    FileConst.CurLanguage = language;
        //}
        //catch (System.Exception ex)
        //{
        //    Debuger.Log("[locationTextMgr.SwitchLanguage() => old version client...]");
        //}
        //finally
        //{
        LocalDataMgr.CurLanguage = language;
        LanguageMgr.curLanguage = language;
        MainMapMgr.Instance.SetBuildName();
        for (int i = _lstAllLoctionTxt.Count - 1; i >= 0; i--)
        {
            if (_lstAllLoctionTxt[i] == null || _lstAllLoctionTxt[i].gameObject == null)
            {
                _lstAllLoctionTxt.RemoveAt(i);
                continue;
            }
            if (_lstAllLoctionTxt[i].IDInValid())
                continue;
            _lstAllLoctionTxt[i].text = LanguageMgr.GetLanguage(_lstAllLoctionTxt[i].IDLanguage);
        }
        //}
    }

    public void ChangeTextIDLanguage(LocationText text, int languageID)
    {
        text.IDLanguage = languageID;
        if (!_lstAllLoctionTxt.Contains(text))
            _lstAllLoctionTxt.Add(text);
    }

    public void RegisteText(LocationText text)
    {
        if (_lstAllLoctionTxt.Contains(text))
            return;
        if (!text.IDInValid())
            text.text = LanguageMgr.GetLanguage(text.IDLanguage);
        _lstAllLoctionTxt.Add(text);
    }

    public void UnRegistText(LocationText text)
    {
        if (_lstAllLoctionTxt.Contains(text))
            _lstAllLoctionTxt.Remove(text);
    }
}
