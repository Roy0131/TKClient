using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactView : UIBaseView
{
    private Button _leftBtn;
    private Button _rightBtn;
    private List<ArtifactItemView> _listArtifactItemView;

    private int _bookmarkNum;
    private int _bookmark;
    private int bossNum;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _leftBtn = Find<Button>("LeftBtn");
        _rightBtn = Find<Button>("RightBtn");

        _listArtifactItemView = new List<ArtifactItemView>();
        for (int i = 0; i < 3; i++)
        {
            GameObject obj = Find("ScrollView/Content/Item" + (i + 1));
            ArtifactItemView artifactItemView = new ArtifactItemView();
            artifactItemView.SetDisplayObject(obj);
            _listArtifactItemView.Add(artifactItemView);
        }

        _leftBtn.onClick.Add(OnLeft);
        _rightBtn.onClick.Add(OnRight);
    }

    private void OnMarkNum()
    {
        bossNum = ArtifactDataModel.Instance.mListArtifactVO.Count;
        if (bossNum % 3 == 0)
            _bookmarkNum = bossNum / 3;
        else
            _bookmarkNum = (bossNum / 3) + 1;
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        OnMarkNum();
        _bookmark = int.Parse(args[0].ToString());
        OnBookmark(_bookmark);
    }

    private void OnBookmark(int book)
    {
        _leftBtn.gameObject.SetActive(book > 0);
        _rightBtn.gameObject.SetActive(book < (_bookmarkNum - 1));

        OnBossChange();
    }

    private void OnBossChange()
    {
        for (int i = 0; i < _listArtifactItemView.Count; i++)
        {
            if (_bookmark == _bookmarkNum - 1 && i >= bossNum % 3 && bossNum % 3 != 0)
            {
                _listArtifactItemView[i].Hide();
            }
            else
            {
                ArtifactDataVO vo = new ArtifactDataVO();
                vo = ArtifactDataModel.Instance.OnArtifactVO(_bookmark * 3 + i + 1);
                _listArtifactItemView[i].Show(vo);
            }
        }
    }

    private void OnLeft()
    {
        _bookmark -= 1;
        OnBookmark(_bookmark);
    }

    private void OnRight()
    {
        _bookmark += 1;
        OnBookmark(_bookmark);
    }
}
