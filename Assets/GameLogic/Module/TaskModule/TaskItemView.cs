using Framework.UI;
using Msg.ClientMessage;
using UnityEngine;
using UnityEngine.UI;

public class TaskItemView : UIBaseView
{
    private Text _allText;
    private Text _fillText;
    private Image _fillImg;
    private Button _jumpBtn;
    private Button _drawBtn;
    private GameObject _jumpObj;
    private GameObject _drawObj;
    private RectTransform _Parent;

    private TaskData _taskData;
    private ItemView _view;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _allText = Find<Text>("AllText");
        _fillText = Find<Text>("FillImg/Text");
        _fillImg = Find<Image>("FillImg/Fill");
        _jumpBtn = Find<Button>("JumpBtn");
        _drawBtn = Find<Button>("DrawBtn");
        _Parent = Find<RectTransform>("RewardGrod");
        _jumpObj = Find("JumpBtn");
        _drawObj = Find("DrawBtn");

        _jumpBtn.onClick.Add(OnJump);
        _drawBtn.onClick.Add(OnDraw);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _taskData = args[0] as TaskData;
        OnTaskItem();
    }

    private void OnTaskItem()
    {
        MissionConfig cfg = GameConfigMgr.Instance.GetMissionConfig(_taskData.Id);
        string[] rewards = cfg.Reward.Split(',');
        if (rewards.Length % 2 != 0)
            return;
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        for (int i = 0; i < rewards.Length; i += 2)
        {
            ItemInfo itemInfo = new ItemInfo();
            itemInfo.Id = int.Parse(rewards[i]);
            itemInfo.Value = int.Parse(rewards[i + 1]);
            if (GameConfigMgr.Instance.GetItemConfig(itemInfo.Id).ItemType == 2)
                _view = ItemFactory.Instance.CreateItemView(itemInfo, ItemViewType.EquipHeroItem);
            else
                _view = ItemFactory.Instance.CreateItemView(itemInfo, ItemViewType.HeroItem);
            _view.mRectTransform.SetParent(_Parent, false);
        }
        if (cfg.CompleteNum > 0)
        {
            if (cfg.EventParam > 0)
                _allText.text = LanguageMgr.GetLanguage(cfg.Title, cfg.CompleteNum, cfg.EventParam);
            else
                _allText.text = LanguageMgr.GetLanguage(cfg.Title, cfg.CompleteNum);
        }
        else
        {
            if (cfg.EventParam > 0)
                _allText.text = LanguageMgr.GetLanguage(cfg.Title, cfg.EventParam);
            else
                _allText.text = LanguageMgr.GetLanguage(cfg.Title);
        }
        if (cfg.CompleteNum == 0)
        {
            _fillText.text = (_taskData.Value + "/" + 1);
            _fillImg.fillAmount = (float)_taskData.Value / (float)1;
        }
        else
        {
            if (_taskData.Value >= cfg.CompleteNum)
                _fillText.text = cfg.CompleteNum + "/" + cfg.CompleteNum;
            else
                _fillText.text = _taskData.Value + "/" + cfg.CompleteNum;
            _fillImg.fillAmount = (float)_taskData.Value / (float)cfg.CompleteNum;
        }
        _jumpObj.SetActive(_taskData.State == 0 && cfg.Type == TaskTypeConst.DAILYTask);
        _drawObj.SetActive(_taskData.State != 0 && cfg.Type == TaskTypeConst.DAILYTask || cfg.Type == TaskTypeConst.ACHIEVETask);
        if (_taskData.State == 1)
            _drawBtn.interactable = true;
        else
            _drawBtn.interactable = false;
    }

    private void OnJump()
    {
        JumpModule.JumpType((JumpType)GameConfigMgr.Instance.GetMissionConfig(_taskData.Id).Hyperlink);
    }

    private void OnDraw()
    {
        GameNetMgr.Instance.mGameServer.ReqTaskReward(_taskData.Id);
    }

    public override void Dispose()
    {
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        _view = null;
        base.Dispose();
    }
}
