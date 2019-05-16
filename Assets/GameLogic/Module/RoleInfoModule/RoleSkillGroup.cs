using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Framework.UI;

public class RoleSkillGroup : UIBaseView
{
    class SkillItem : UIBaseView
    {
        private bool _blUnlock;
        private Image _skillIcon;
        private Button _button;
        private int _skillID;
        private int _rankCond;
        private ImageGray _imageGray;
        private GameObject _skillImg;
        private Text _skillRank;

        public SkillItem(bool blUnlock)
        {
            _blUnlock = blUnlock;
            _skillID = 0;
        }

		protected override void ParseComponent()
		{
            base.ParseComponent();
            _skillImg = Find("Skill/Img");
            _skillRank = Find<Text>("Skill/Img/Text");
            _skillIcon = Find<Image>("Skill");
            _button = Find<Button>("Skill");
            _imageGray = Find<ImageGray>("Skill");
            MouseEventListener.Get(_imageGray.gameObject).mMouseDown = OnMouseDown;
            MouseEventListener.Get(_imageGray.gameObject).mMouseUp = OnMouseUp;
		}

        private void OnMouseDown(GameObject go)
        {
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(UIEventDefines.ShowSkillTips, _skillID, _rankCond, _blUnlock);
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(UIEventDefines.SkillType, SkillDataVO.mSkillType);
        }

        private void OnMouseUp(GameObject go)
        {
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(UIEventDefines.HideSkillTips);
        }

		protected override void Refresh(params object[] args)
		{
            base.Refresh(args);
            int skillId = int.Parse(args[0].ToString());
            _rankCond = int.Parse(args[1].ToString());
            //if (skillId == _skillID)
            //    return;
            _skillID = skillId;
            SkillConfig config = GameConfigMgr.Instance.GetSkillConfig(skillId);
            if (config == null)
                return;
            _skillRank.text = config.InnerLevel.ToString();
            _skillImg.SetActive(config.InnerLevel > 1);
            _skillIcon.sprite = GameResMgr.Instance.LoadSkillIcon(config.Icon);
            ObjectHelper.SetSprite(_skillIcon,_skillIcon.sprite);
            if (!_blUnlock)
                _imageGray.SetGray();
		}
	}

    private List<SkillItem> _lstSkillItem;
    private GameObject _skillItemObj;
    private string _skillValue;
	protected override void ParseComponent()
	{
        base.ParseComponent();
        _skillItemObj = Find("SkillItem");
    }

    protected override void Refresh(params object[] args)
	{
        base.Refresh(args);
        string skillValue = args[0].ToString();
        //if (string.Equals(skillValue, _skillValue))
        //    return;
		int curRank = int.Parse(args[1].ToString());
        DisposeSkillItem();
        _skillValue = skillValue;
        string[] skills = _skillValue.Split(',');
        if (skills.Length == 0)
            return;
        _lstSkillItem = new List<SkillItem>();
        int rank, skillId;
        SkillItem item;
        for (int i = 0; i < skills.Length; i += 2)
        {
            rank = int.Parse(skills[i]);
            skillId = int.Parse(skills[i + 1]);
            item = new SkillItem(curRank >= rank);
            item.SetDisplayObject(GameObject.Instantiate(_skillItemObj));
            item.Show(skillId, rank);
            item.mRectTransform.SetParent(mRectTransform, false);
            _lstSkillItem.Add(item);
        }
	}

    private void DisposeSkillItem()
    {
        if (_lstSkillItem == null)
            return;
        for (int i = 0; i < _lstSkillItem.Count; i++)
            _lstSkillItem[i].Dispose();
        _lstSkillItem.Clear();
        _lstSkillItem = null;
    }

	public override void Dispose()
	{
        DisposeSkillItem();
        base.Dispose();
	}

}