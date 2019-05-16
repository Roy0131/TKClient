using Framework.UI;
using System.Collections.Generic;
using UnityEngine;

public class ServerView : UILoopBaseView<ServerDataVO>
{
    private List<ServerItemView> _listServerItemView;
    private GameObject _serverObj;
    private RectTransform _parent;
    protected override void ParseComponent()
    {
        base.ParseComponent();
        InitScrollRect("Panel_Scroll");
        _serverObj = Find("ServerObj");
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);

        _lstDatas = ServerDataModel.Instance.mListServerDataVO;
        if (_lstDatas.Count == 0)
            return;
        _lstDatas.Sort(OnSort);
        _loopScrollRect.ClearCells();
        _loopScrollRect.totalCount = _lstDatas.Count;
        _loopScrollRect.RefillCells();
    }

    protected override UIBaseView CreateItemView()
    {
        ServerItemView item = new ServerItemView();
        item.SetDisplayObject(GameObject.Instantiate(_serverObj));
        return item;
    }

    private int OnSort(ServerDataVO v1, ServerDataVO v2)
    {
        if (v1.mServerId == LoginHelper.ServerID)
            return -1;
        else if (v2.mServerId == LoginHelper.ServerID)
            return 1;
        else if (v1.mPlayerLevel > 0 && v2.mPlayerLevel > 0)
            return v1.mServerId > v2.mServerId ? -1 : 1;
        else if (v1.mPlayerLevel > 0 && v2.mPlayerLevel == 0)
            return -1;
        else if (v1.mPlayerLevel == 0 && v2.mPlayerLevel > 0)
            return 1;
        else
            return v1.mServerId > v2.mServerId ? -1 : 1;
    }
}
