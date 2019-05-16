using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactAttView : UIBaseView
{
    private Text _attName;
    private Button _disBtn;
    private List<ArtifactAllAttItemView> _listAllAttItemView;
    private GameObject _attItem;
    private RectTransform _attParent;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _attName = Find<Text>("Name");
        _disBtn = Find<Button>("DisBtn");
        _attParent = Find<RectTransform>("Atts");
        _attItem = Find("Atts/AttItem");

        _disBtn.onClick.Add(Hide);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _attName.text = LanguageMgr.GetLanguage(400004);
        Dictionary<int, int> dictAllAtt = new Dictionary<int, int>();
        foreach (var item in ArtifactDataModel.Instance.mAllArtifactAtt.Values)
        {
            for (int i = 0; i < item.Count; i++)
            {
                if (dictAllAtt.ContainsKey(item[i].Id))
                    dictAllAtt[item[i].Id] += item[i].Value;
                else
                    dictAllAtt.Add(item[i].Id, item[i].Value);
            }
        }
        OnAttitemClear();
        _listAllAttItemView = new List<ArtifactAllAttItemView>();
        foreach (var item in dictAllAtt)
        {
            GameObject obj = GameObject.Instantiate(_attItem);
            obj.transform.SetParent(_attParent, false);
            ArtifactAllAttItemView attItemView = new ArtifactAllAttItemView();
            attItemView.SetDisplayObject(obj);
            attItemView.Show(item.Key, item.Value);
            _listAllAttItemView.Add(attItemView);
        }
    }

    private void OnAttitemClear()
    {
        if (_listAllAttItemView != null)
        {
            for (int i = 0; i < _listAllAttItemView.Count; i++)
                _listAllAttItemView[i].Dispose();
            _listAllAttItemView.Clear();
            _listAllAttItemView = null;
        }
    }
}
