using Framework.UI;
using UnityEngine.UI;

public class CTowerRewardTipsView : UIBaseView {

    private Button _rewardTipsWindowBack;
    private ItemConfig _itemCfg;
    private Text _name;
    private Image _imageIcon;
    private Text _textDes01;
    private Text _textDes02;

    //public CTowerRewardTipsView()
    //{
    //}

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _rewardTipsWindowBack = Find<Button>("BlackBack");
        _name = Find<Text>("TextName");
        _imageIcon = Find<Image>("ImageIconBack/ImageIcon");
        _textDes01 = Find<Text>("TextDescribe01");
        _textDes02 = Find<Text>("TextDescribe02");

        _rewardTipsWindowBack.onClick.Add(OnClose);
    }

    private void OnClose()
    {
        Hide();
    }
    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        //SetData();
    }
    //private void SetData()
    //{
    //    //_imageIcon.sprite = GameResMgr.Instance.LoadItemIcon(_itemCfg.Icon);
    //    //_name.text = GameResMgr.Instance.load
    //}
}
