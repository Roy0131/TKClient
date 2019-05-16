using System;
using System.Collections.Generic;
using Msg.ClientMessage;

public class ArenaRewardVO : DataBaseVO
{
    private int _minRank;
    private int _maxRank;
    private ArenaRewardType _type;
    public ArenaDivisionConfig config { get; private set; }

    public List<ItemInfo> mlstReward;
    public ArenaRewardVO(ArenaRewardType type)
    {
        _type = type;
    }

    protected override void OnInitData<T>(T value)
    {
        if (_type == ArenaRewardType.RankReward)
        {
            config = value as ArenaDivisionConfig;
            _rewardIndex = config.Division;
            _minRank = config.DivisionScoreMin;
            _maxRank = config.DivisionScoreMax;
            ParseReward(config.RewardList);
            _rewardTitle = LanguageMgr.GetLanguage(config.Name);
        }
        else
        {
            ArenaRankingBonusConfig cfg = value as ArenaRankingBonusConfig;
            _rewardIndex = cfg.Index;
            _minRank = cfg.RankingMin;
            _maxRank = cfg.RankingMax;
            ParseReward(_type == ArenaRewardType.DailyReward ? cfg.DayRewardList : cfg.SeasonRewardList);
            if (_minRank == _maxRank)
                _rewardTitle = _minRank.ToString();
            else
                _rewardTitle = _minRank + "-" + _maxRank;
        }
    }

    private void ParseReward(string rewards)
    {
        mlstReward = new List<ItemInfo>();
        string[] values = rewards.Split(',');
        if (values.Length % 2 != 0)
        {
            LogHelper.LogError("[ArenaRewardVO.ParseReward() => reward format error!!!]");
            return;
        }
        ItemInfo item;
        for (int i = 0; i < values.Length; i += 2)
        {
            item = new ItemInfo();
            item.Id = int.Parse(values[i]);
            item.Value = int.Parse(values[i + 1]);
            mlstReward.Add(item);
        }
    }

    public bool InRange(int rank)
    {
        return rank >= _minRank && rank <= _maxRank;
    }

    private string _rewardTitle;
    public string RewardTitle
    {
        get { return _rewardTitle; }
    }

    private int _rewardIndex;
    public int RewardIndex
    {
        get { return _rewardIndex; }
    }
}