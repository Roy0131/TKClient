using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchLanguageView : UIBaseView
{
    private Button _disBtn;
    private List<LanguageItemView> _listLanguageItemViews;
    private GameObject _languageObj;
    private RectTransform _parent;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _disBtn = Find<Button>("DisBtn");
        _parent = Find<RectTransform>("Panel_Scroll/KnapsackPanel");
        _languageObj = Find("Panel_Scroll/KnapsackPanel/LanguageItem");

        _disBtn.onClick.Add(OnDisBtn);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        OnLanguageItemClear();
        _listLanguageItemViews = new List<LanguageItemView>();
        List<SystemLanguage> allLanguages = GameConst.AllLanguages;
        List<string> languageTitles = GameConst.AllLanguageTitles;
        for (int i = 0; i < languageTitles.Count; i++)
        {
            GameObject obj = GameObject.Instantiate(_languageObj);
            obj.transform.SetParent(_parent, false);
            LanguageItemView languageItemView = new LanguageItemView(allLanguages[i]);
            languageItemView.SetDisplayObject(obj);
            languageItemView.Show(languageTitles[i]);
            _listLanguageItemViews.Add(languageItemView);
        }
    }

    private void OnLanguageItemClear()
    {
        if (_listLanguageItemViews != null)
        {
            for (int i = _listLanguageItemViews.Count - 1; i >= 0; i--)
                _listLanguageItemViews[i].Dispose();
            _listLanguageItemViews.Clear();
            _listLanguageItemViews = null;
        }
    }

    private void OnDisBtn()
    {
        Hide();
    }
}
