using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class BuffTipsView : UIBaseView
{
    public bool mBlMoveEnd { get; private set; }
    protected Text _text;
    protected override void ParseComponent()
    {
        base.ParseComponent();

        _text = Find<Text>("TextGreen");
    }

    public override void Show(params object[] args)
    {
        base.Show(args);
        StatusConfig config = args[0] as StatusConfig;
        _text.text = LanguageMgr.GetLanguage(config.BuffTextID);
    }

    public void PlayAnimation(Vector3 pos)
    {
        mRectTransform.anchoredPosition = pos;
        _text.gameObject.SetActive(true);

        mBlMoveEnd = false;

        DGHelper.DoKill(mRectTransform);
        DGHelper.DoKill(_text);
        
        Vector2 targetAchPos = new Vector2(pos.x, pos.y + 60f);

        //OutQuad
        DGHelper.DoAnchorPos(mRectTransform, targetAchPos, 0.5f, 0, OnMoveEnd);
    }

    private void OnMoveEnd()
    {
        DGHelper.DoKill(mRectTransform);
        DGHelper.DoKill(_text);
        mBlMoveEnd = true;
    }

    public override void Hide()
    {
        _text.gameObject.SetActive(false);
        base.Hide();
    }
}