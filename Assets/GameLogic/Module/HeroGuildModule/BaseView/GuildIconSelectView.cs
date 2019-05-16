using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Framework.UI;
using System;

public class GuildIconSelectView : UIBaseView
{
    private Action<GuildMarkConfig> _onMethod;
    private bool _blInited = false;
    private Button _disBtn;

    public GuildIconSelectView(Action<GuildMarkConfig> onMethod)
    {
        _onMethod = onMethod;
    }
    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _disBtn = Find<Button>("ImageBack");
        _disBtn.onClick.Add(Hide);
        ColliderHelper.SetButtonCollider(_disBtn.transform);
        if (!_blInited)
        {
            Dictionary<int, GuildMarkConfig>.ValueCollection vall = GuildMarkConfig.Get().Values;
            GameObject logoItem = Find("LogoItem");
            Transform logoParent = Find<Transform>("ScrollView/Content");
            Image image;
            Button btn;
            GameObject imageItem;
            foreach (GuildMarkConfig config in vall)
            {
                imageItem = GameObject.Instantiate(logoItem);
                imageItem.transform.SetParent(logoParent, false);
                image = imageItem.transform.Find("Logoicon").GetComponent<Image>();
                imageItem.SetActive(true);
                image.sprite = GameResMgr.Instance.LoadGuildIcon(config.Icon);
                ObjectHelper.SetSprite(image,image.sprite);
                btn = imageItem.GetComponent<Button>();
                btn.onClick.Add(() => { OnChooseLogo(config); });
            }
            _blInited = true;
        }
    }

    private void OnChooseLogo(GuildMarkConfig config)
    {
        if (_onMethod != null)
        {
            _onMethod.Invoke(config);
        }
    }

    public override void Dispose()
    {
        _blInited = false;
        _onMethod = null;
        base.Dispose();
    }
}