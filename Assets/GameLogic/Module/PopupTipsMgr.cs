using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupTipsMgr : Singleton<PopupTipsMgr>
{
    #region init TipsView
    private Queue<TipsView> _uiPools = new Queue<TipsView>();
    private Queue<string> _tipsPool = new Queue<string>();
    private TipsView _curShowTips = null;
    #endregion

    private TipsView GetTipsView()
    {
        TipsView view;
        if (_uiPools.Count > 0)
        {
            view = _uiPools.Dequeue();
        }
        else
        {
            view = new TipsView();
            view.SetDisplayObject(GameResMgr.Instance.LoadUIObjectSync(SingletonResName.UITips));
        }
        GameUIMgr.Instance.AddObjectToTopRoot(view.mRectTransform);
        view.mRectTransform.localPosition = Vector2.zero;
        return view;
    }

    public void ShowTips(string content)
    {
        if (_curShowTips != null)
        {
            if (content == _curShowTips.mContent)
                return;
            if (_tipsPool.Contains(content))
                return;
            _tipsPool.Enqueue(content);
        }
        else
        {
            Show(content);
        }
    }

    public void ShowTips(int languageID, params object[] args)
    {
        string value = LanguageMgr.GetLanguage(languageID, args);
        if (string.IsNullOrWhiteSpace(value))
            return;
        ShowTips(value);
    }

    private void Show(string value)
    {
        _curShowTips = GetTipsView();
        _curShowTips.Show(value);
    }

    public void ReturnView(TipsView view)
    {
        _curShowTips = null;
        _uiPools.Enqueue(view);
        view.Hide();
        if (_tipsPool.Count <= 0)
            return;
        Show(_tipsPool.Dequeue());
    }
}

public class TipsView : UIBaseView
{
    private static Vector2 _targetPos = new Vector2(0, 100f);

    private Text _text;
    private Color _defaultColor;
    private Image _backGround;

    public string mContent { get; private set; }
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _text = Find<Text>("Label");
        _backGround = Find<Image>("BackGround");
        _defaultColor = _text.color;
        
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        mContent = args[0].ToString();
        _text.text = mContent;
        _text.color = _defaultColor;
        _backGround.color = new Color(0.2f, 0.15f, 0.3f, 0.6f);
        //mRectTransform.DOAnchorPos(_targetPos, 0.8f).onComplete = OnMoveEnd;
        DGHelper.DoAnchorPos(mRectTransform, _targetPos, 0.3f, 0, OnMoveEnd);
    }

    private void OnMoveEnd()
    {
        DGHelper.DoTextFade(_text, 0f, 1f, 0, DoAlphaEnd);
        DGHelper.DoImageFade(_backGround, 0f, 1f, 0, DoAlphaEnd);
    }

    private void DoAlphaEnd()
    {
        DGHelper.DoKill(mRectTransform);
        DGHelper.DoKill(_text);
        DGHelper.DoKill(_backGround);
        PopupTipsMgr.Instance.ReturnView(this);
    }
}