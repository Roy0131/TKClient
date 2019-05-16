using Framework.UI;
using UnityEngine;

public class ArtifactRankItemView : UIBaseView
{
    private int _id;
    private int _curId;
    private GameObject _icon;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _icon = Find("icon");
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _id = int.Parse(args[0].ToString());
        _curId = int.Parse(args[1].ToString());
        _icon.SetActive(_id <= _curId);
    }
}
