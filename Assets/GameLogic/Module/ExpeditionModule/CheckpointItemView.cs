using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointItemView : UIBaseView
{
    public ExpeditionConfig mCfg { get; private set; }
    private ImageGray imageGray1;
    private ImageGray imageGray2;
    private ImageGray imageGray3;
    private Button _imgBtn1;
    private Button _imgBtn2;
    private Button _imgBtn3;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        imageGray1 = Find<ImageGray>("Img1");
        imageGray2 = Find<ImageGray>("Img2");
        imageGray3 = Find<ImageGray>("Img3");
        _imgBtn1 = Find<Button>("Img1");
        _imgBtn2 = Find<Button>("Img2");
        _imgBtn3 = Find<Button>("Img3");
        ColliderHelper.SetButtonCollider(_imgBtn1.transform);
        ColliderHelper.SetButtonCollider(_imgBtn2.transform);
        ColliderHelper.SetButtonCollider(_imgBtn3.transform);

        _imgBtn1.onClick.Add(OnBtn);
        _imgBtn2.onClick.Add(OnBtn);
        _imgBtn3.onClick.Add(OnBtn);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        mCfg = args[0] as ExpeditionConfig;
        _imgBtn1.gameObject.SetActive(mCfg.StageType == 1);
        _imgBtn2.gameObject.SetActive(mCfg.StageType == 2);
        _imgBtn3.gameObject.SetActive(mCfg.StageType == 3);
        if (mCfg.ID - 1 < ExpeditionDataModel.Instance.mCurStage)
        {
            imageGray1.SetGray();
            imageGray2.SetGray();
            imageGray3.SetGray();
        }
        else
        {
            imageGray1.SetNormal();
            imageGray2.SetNormal();
            imageGray3.SetNormal();
        }
    }

    private void OnBtn()
    {
        OnStage(mCfg.ID - 1);
    }

    private void OnStage(int index)
    {
        if (index < ExpeditionDataModel.Instance.mCurStage)
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(5003404));
        else if (index == ExpeditionDataModel.Instance.mCurStage)
            ExpeditionDataModel.Instance.ReqExpeditionStageData();
    }

    public override void Hide()
    {
        imageGray1.SetNormal();
        imageGray2.SetNormal();
        imageGray3.SetNormal();
        base.Hide();
    }
}
