using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactPreView : UIBaseView
{
    private ArtifactDataVO _artifactDataVO;
    private Button _disBtn;
    private Text _detailName;
    private Text _grade;
    private Text _rankName;
    private List<ArtifactRankItemView> _listRankItemView;
    private List<ArtifactAttItemView> _listAttItemView;
    private Text _skillName;
    private Text _skillDetail;
    private Image _skillIcon;
    private GameObject _rankGrid;
    private RectTransform _parent;
    private GameObject _attItem;
    private RectTransform _attParent;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _disBtn = Find<Button>("DisBtn");
        _detailName = Find<Text>("PreView/Detail");
        _grade = Find<Text>("PreView/Grade");
        _rankName = Find<Text>("PreView/Rank/Name");
        _skillName = Find<Text>("PreView/SkillImg/Name");
        _skillDetail = Find<Text>("PreView/SkillExplain");
        _skillIcon = Find<Image>("PreView/SkillImg/SkillIcon");
        _rankGrid = Find("PreView/Rank/RankGrid/item");
        _parent = Find<RectTransform>("PreView/Rank/RankGrid");
        _attItem = Find("PreView/AttImg/Item");
        _attParent = Find<RectTransform>("PreView/AttImg");

        _disBtn.onClick.Add(Hide);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _artifactDataVO = args[0] as ArtifactDataVO;
        _detailName.text = LanguageMgr.GetLanguage(_artifactDataVO.mArtifactNameId);
        _grade.text = "Lv：" + _artifactDataVO.mMaxLevel + "/" + _artifactDataVO.mMaxLevel;
        _rankName.text = LanguageMgr.GetLanguage(5002705);
        OnRankitemClear();
        _listRankItemView = new List<ArtifactRankItemView>();
        for (int i = 1; i < _artifactDataVO.mMaxRank; i++)
        {
            GameObject obj = GameObject.Instantiate(_rankGrid);
            obj.transform.SetParent(_parent, false);
            ArtifactRankItemView rankItemView = new ArtifactRankItemView();
            rankItemView.SetDisplayObject(obj);
            rankItemView.Show(i, _artifactDataVO.mMaxRank);
            _listRankItemView.Add(rankItemView);
        }
        OnAttitemClear();
        _listAttItemView = new List<ArtifactAttItemView>();
        for (int i = 0; i < _artifactDataVO.mPreViewAtt.Count; i++)
        {
            GameObject obj = GameObject.Instantiate(_attItem);
            obj.transform.SetParent(_attParent, false);
            ArtifactAttItemView attItemView = new ArtifactAttItemView();
            attItemView.SetDisplayObject(obj);
            attItemView.Show(_artifactDataVO.mPreViewAtt[i]);
            _listAttItemView.Add(attItemView);
        }
        SkillConfig config = GameConfigMgr.Instance.GetSkillConfig(_artifactDataVO.mPreViewSkillId);
        _skillName.text = "Lv." + config.InnerLevel + " " + LanguageMgr.GetLanguage(config.NameID);
        string[] skillArgs = config.ShowParam.Split(',');
        _skillDetail.text = LanguageMgr.GetLanguage(config.DescrptionID, skillArgs);
        _skillIcon.sprite = GameResMgr.Instance.LoadSkillIcon(config.Icon);
    }

    private void OnRankitemClear()
    {
        if (_listRankItemView != null)
        {
            for (int i = 0; i < _listRankItemView.Count; i++)
                _listRankItemView[i].Dispose();
            _listRankItemView.Clear();
            _listRankItemView = null;
        }
    }

    private void OnAttitemClear()
    {
        if (_listAttItemView != null)
        {
            for (int i = 0; i < _listAttItemView.Count; i++)
                _listAttItemView[i].Dispose();
            _listAttItemView.Clear();
            _listAttItemView = null;
        }
    }

    public override void Dispose()
    {
        OnRankitemClear();
        OnAttitemClear();
        base.Dispose();
    }
}
