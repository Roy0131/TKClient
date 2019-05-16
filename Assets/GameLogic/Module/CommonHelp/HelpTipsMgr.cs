using UnityEngine;

public class HelpTipsMgr : Singleton<HelpTipsMgr>
{

    private HelpTipsView _helpTipsView;
    public int descrptionID;

    private void InitHelpTips()
    {
        if (_helpTipsView == null)
            GameResMgr.Instance.LoadUIObjectAsync(SingletonResName.UIHelpTips, OnInitHelpView);
        else
            _helpTipsView.Show();
    }

    private void OnInitHelpView(GameObject uiObject)
    {
        _helpTipsView = new HelpTipsView();
        _helpTipsView.SetDisplayObject(uiObject);
        _helpTipsView.Show();
        GameUIMgr.Instance.AddObjectToTopRoot(_helpTipsView.mRectTransform);
    }

    public void ShowTIps(int _descrptionID)
    {
        descrptionID = _descrptionID;
        InitHelpTips();
    }
    public void HideTips()
    {
        if (_helpTipsView == null)
            return;
        _helpTipsView.Hide();
    }

}
