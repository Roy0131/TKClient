using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class PlayerAvatarItemView : UIBaseView
{
    private Image _avatarImg;
    private Button _btn;
    private GameObject _selected;
    public int mAvatarId { get; private set; }
    private bool _blSelected;
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _avatarImg = Find<Image>("Img");
        _btn = Find<Button>("Img");
        _selected = Find("Sele");
        _btn.onClick.Add(OnBtn);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        mAvatarId = int.Parse(args[0].ToString());
        if (mAvatarId == HeroDataModel.Instance.mHeroInfoData.mIcon)
            BlSelected = true;
        else
            BlSelected = false;
        _avatarImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(mAvatarId).Icon);
        ObjectHelper.SetSprite(_avatarImg,_avatarImg.sprite);
        if (GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(mAvatarId).Icon) == null)
        {
            LogHelper.Log(GameConfigMgr.Instance.GetItemConfig(mAvatarId).Icon);
        }
    }

    private void OnBtn()
    {
        GameNetMgr.Instance.mGameServer.ReqPlayerChangHead(mAvatarId);
    }

    public bool BlSelected
    {
        get { return _blSelected; }
        set
        {
            if (_blSelected == value)
                return;
            _blSelected = value;
            _selected.SetActive(_blSelected == true);
        }
    }
}
