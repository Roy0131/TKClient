using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class ArenaRecordView : UIBaseView
{
    private Button _closeBtn;
    private RectTransform _recordItemRoot;
    private GameObject _recordItemObj;
    private Transform _root;
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _root = Find<Transform>("Root");
        _closeBtn = Find<Button>("Root/BtnClose");
        ColliderHelper.SetButtonCollider(_closeBtn.transform);
        _recordItemObj = Find("Root/RecordRoot/Content/Item");
        _recordItemRoot = Find<RectTransform>("Root/RecordRoot/Content");

        _closeBtn.onClick.Add(Hide);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);

        List<BattleRecordData> lstRecord = ArenaDataModel.Instance.mlstRecordData;
        UIBaseView recordView;
        int i;
        for (i = 0; i < lstRecord.Count; i++)
        {
            if (i <= _childrenViews.Count - 1)
            {
                recordView = _childrenViews[i];
            }
            else
            {
                recordView = new ArenaRecordItem();
                recordView.SetDisplayObject(GameObject.Instantiate(_recordItemObj));
                recordView.mRectTransform.SetParent(_recordItemRoot, false);
                AddChildren(recordView);
            }
            recordView.Show(lstRecord[i]);
        }

        for (i = lstRecord.Count; i < _childrenViews.Count; i++)
            _childrenViews[i].Hide();
    }
    protected override void OnShowViewAnimation()
    {
        base.OnShowViewAnimation();
        ObjectHelper.PopAnimationLiner(_root);
    }
}