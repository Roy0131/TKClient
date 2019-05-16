using Msg.ClientMessage;
using System.Collections.Generic;


public class TalentTypeConst
{
    public const int Basis = 1;//基础天赋
    public const int Guild = 2;//联盟天赋
    public const int Not = 3;//暂无天赋
}

public class TalentAttTypeConst
{
    public const int All = 0;//所有角色通用属性
    public const int Attack = 1;//攻击角色属性
    public const int Defense = 2;//防御角色属性
    public const int Skill = 3;//技巧角色属性
}



public class TalentDataModel : ModelDataBase<TalentDataModel>
{
    private Dictionary<int, Dictionary<int, TalentVO>> _dictTalents;//所有天赋
    private Dictionary<int, Dictionary<int, int>> _dictAttri;
    private Dictionary<int, int> _dictTalentNum;

    protected override void OnInit()
    {
        _dictTalents = new Dictionary<int, Dictionary<int, TalentVO>>();
        _dictAttri = new Dictionary<int, Dictionary<int, int>>();
        _dictTalentNum = new Dictionary<int, int>();
        ParseTalent(TalentTypeConst.Basis);
        ParseTalent(TalentTypeConst.Guild);
        base.OnInit();
    }
    private void ParseTalent( int type )
    {
        int start;
        int end;
        if (type == TalentTypeConst.Basis)
        {
            start = 1;
            end = 7;
        }
        else
        {
            start = 11;
            end = 20;
        }
        TalentVO vo;
        Dictionary<int, TalentVO> tmpDict = new Dictionary<int, TalentVO>();
        for (int i = start; i <= end; i++)
        {
            vo = new TalentVO(i);
            tmpDict.Add(i, vo);
        }
        _dictTalents.Add(type, tmpDict);
    }

    private void OnTalentData(S2CTalentListResponse value)
    {
        Dictionary<int, TalentVO> tmpDict;
        int baseTalentID;
        for (int i = 0; i < value.Talents.Count; i++)
        {
            baseTalentID = value.Talents[i].Id;
            tmpDict = _dictTalents[GameConfigMgr.Instance.GetTalentConfig(value.Talents[i].Id * 100 + value.Talents[i].Level).PageLabel];
            if (tmpDict.ContainsKey(baseTalentID))
                tmpDict[baseTalentID].InitData(value.Talents[i]);

            if (GameConfigMgr.Instance.GetTalentConfig(value.Talents[i].Id * 100 + value.Talents[i].Level).PageLabel == TalentTypeConst.Basis)
            {
                if (_dictTalentNum.ContainsKey(TalentTypeConst.Basis))
                    _dictTalentNum[TalentTypeConst.Basis] = 1;
                else
                    _dictTalentNum.Add(TalentTypeConst.Basis, 1);
            }
            else
            {
                if (_dictTalentNum.ContainsKey(TalentTypeConst.Guild))
                    _dictTalentNum[TalentTypeConst.Guild] = 1;
                else
                    _dictTalentNum.Add(TalentTypeConst.Guild, 1);
            }
        }
        OnTalentAtt();
        Instance.DispathEvent(TalentEvent.TalentDate);
    }

    private void OnTalentUp(S2CTalentUpResponse value)
    {
        int talentType = GameConfigMgr.Instance.GetTalentConfig(value.TalentId * 100 + value.Level).PageLabel;
        int start;
        int end;
        if (talentType == TalentTypeConst.Basis)
        {
            start = 1;
            end = 7;
            if (_dictTalentNum.ContainsKey(TalentTypeConst.Basis))
                _dictTalentNum[TalentTypeConst.Basis] = 1;
            else
                _dictTalentNum.Add(TalentTypeConst.Basis, 1);
        }
        else
        {
            start = 11;
            end = 20;
            if (_dictTalentNum.ContainsKey(TalentTypeConst.Guild))
                _dictTalentNum[TalentTypeConst.Guild] = 1;
            else
                _dictTalentNum.Add(TalentTypeConst.Guild, 1);
        }
        for (int i = start; i <= end; i++)
        {
            if (_dictTalents[talentType][i].mTalentBaseID == value.TalentId)
                _dictTalents[talentType][i].OnTalentLevelUp(value.Level);
        }
        OnTalentAtt();
        Instance.DispathEvent(TalentEvent.TalentUp);
    }

    private void OnTalentReset(S2CTalentResetResponse value)
    {
        _dictTalentNum[value.Tag] = 0;
        foreach (TalentVO vo in _dictTalents[value.Tag].Values)
            vo.OnReset(1);
        List<int> listItem = new List<int>();
        listItem.AddRange(value.ReturnItems);
        OnTalentAtt();
        Instance.DispathEvent(TalentEvent.TalentReset, listItem);
    }

    public TalentVO GetTalentVO(int talentType, int baseID)
    {
        if (_dictTalents.ContainsKey(talentType))
        {
            for (int i = 0; i < _dictTalents[talentType].Count; i++)
            {
                if (_dictTalents[talentType].ContainsKey(baseID))
                    return _dictTalents[talentType][baseID];
            }
        }
        return null;
    }

    public int GetTalentNum(int talentType)
    {
        if (_dictTalentNum.ContainsKey(talentType))
            return _dictTalentNum[talentType];
        return 0;
    }

    public static void DoTalentData(S2CTalentListResponse value)
    {
        Instance.OnTalentData(value);
    }

    public static void DoTalentUp(S2CTalentUpResponse value)
    {
        Instance.OnTalentUp(value);
    }

    public static void DoTalentReset(S2CTalentResetResponse value)
    {
        Instance.OnTalentReset(value);
    }

    private void OnTalentAtt()
    {
        Dictionary<int, int> dictAtt = new Dictionary<int, int>();
        foreach (Dictionary<int, TalentVO> dictVO in _dictTalents.Values)
        {
            foreach (TalentVO vo in dictVO.Values)
            {
                if (vo.mBlLearned)
                {
                    TalentConfig cfg = GameConfigMgr.Instance.GetTalentConfig(vo.mTalentBaseID * 100 + vo.mTalentLevel);
                    string[] talentEff = cfg.TalentEffectCond.Split('|');
                    string[] talentAtt = cfg.TalentAttr.Split(',');
                    if (talentEff.Length < 2 && talentAtt.Length >= 2 && talentAtt.Length % 2 == 0)
                    {
                        dictAtt.Clear();
                        for (int i = 0; i < talentAtt.Length; i += 2)
                        {
                            if (_dictAttri.ContainsKey(TalentAttTypeConst.All))
                            {
                                if (_dictAttri[TalentAttTypeConst.All].ContainsKey(int.Parse(talentAtt[i])))
                                {
                                    _dictAttri[TalentAttTypeConst.All][int.Parse(talentAtt[i])] += int.Parse(talentAtt[i + 1]);
                                }
                                else
                                {
                                    dictAtt = _dictAttri[TalentAttTypeConst.All];
                                    dictAtt.Add(int.Parse(talentAtt[i]), int.Parse(talentAtt[i + 1]));
                                    _dictAttri[TalentAttTypeConst.All] = dictAtt;
                                }
                            }
                            else
                            {
                                dictAtt.Add(int.Parse(talentAtt[i]), int.Parse(talentAtt[i + 1]));
                                _dictAttri.Add(TalentAttTypeConst.All, dictAtt);
                            }
                        }
                    }
                    else if (talentEff.Length >= 2 && talentEff.Length % 2 == 0 && talentAtt.Length >= 2 && talentAtt.Length % 2 == 0 && int.Parse(talentEff[0]) == 10)
                    {
                        if (int.Parse(talentEff[1]) == TalentAttTypeConst.Attack)
                        {
                            dictAtt.Clear();
                            for (int i = 0; i < talentAtt.Length; i += 2)
                            {
                                if (_dictAttri.ContainsKey(TalentAttTypeConst.Attack))
                                {
                                    if (_dictAttri[TalentAttTypeConst.Attack].ContainsKey(int.Parse(talentAtt[i])))
                                    {
                                        _dictAttri[TalentAttTypeConst.Attack][int.Parse(talentAtt[i])] += int.Parse(talentAtt[i + 1]);
                                    }
                                    else
                                    {
                                        dictAtt = _dictAttri[TalentAttTypeConst.Attack];
                                        dictAtt.Add(int.Parse(talentAtt[i]), int.Parse(talentAtt[i + 1]));
                                        _dictAttri[TalentAttTypeConst.Attack] = dictAtt;
                                    }
                                }
                                else
                                {
                                    dictAtt.Add(int.Parse(talentAtt[i]), int.Parse(talentAtt[i + 1]));
                                    _dictAttri.Add(TalentAttTypeConst.Attack, dictAtt);
                                }
                            }
                        }
                        if (int.Parse(talentEff[1]) == TalentAttTypeConst.Defense)
                        {
                            dictAtt.Clear();
                            for (int i = 0; i < talentAtt.Length; i += 2)
                            {
                                if (_dictAttri.ContainsKey(TalentAttTypeConst.Defense))
                                {
                                    if (_dictAttri[TalentAttTypeConst.Defense].ContainsKey(int.Parse(talentAtt[i])))
                                    {
                                        _dictAttri[TalentAttTypeConst.Defense][int.Parse(talentAtt[i])] += int.Parse(talentAtt[i + 1]);
                                    }
                                    else
                                    {
                                        dictAtt = _dictAttri[TalentAttTypeConst.Defense];
                                        dictAtt.Add(int.Parse(talentAtt[i]), int.Parse(talentAtt[i + 1]));
                                        _dictAttri[TalentAttTypeConst.Defense] = dictAtt;
                                    }
                                }
                                else
                                {
                                    dictAtt.Add(int.Parse(talentAtt[i]), int.Parse(talentAtt[i + 1]));
                                    _dictAttri.Add(TalentAttTypeConst.Defense, dictAtt);
                                }
                            }
                        }
                        if (int.Parse(talentEff[1]) == TalentAttTypeConst.Skill)
                        {
                            dictAtt.Clear();
                            for (int i = 0; i < talentAtt.Length; i += 2)
                            {
                                if (_dictAttri.ContainsKey(TalentAttTypeConst.Skill))
                                {
                                    if (_dictAttri[TalentAttTypeConst.Skill].ContainsKey(int.Parse(talentAtt[i])))
                                    {
                                        _dictAttri[TalentAttTypeConst.Skill][int.Parse(talentAtt[i])] += int.Parse(talentAtt[i + 1]);
                                    }
                                    else
                                    {
                                        dictAtt = _dictAttri[TalentAttTypeConst.Skill];
                                        dictAtt.Add(int.Parse(talentAtt[i]), int.Parse(talentAtt[i + 1]));
                                        _dictAttri[TalentAttTypeConst.Skill] = dictAtt;
                                    }
                                }
                                else
                                {
                                    dictAtt.Add(int.Parse(talentAtt[i]), int.Parse(talentAtt[i + 1]));
                                    _dictAttri.Add(TalentAttTypeConst.Skill, dictAtt);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public int GetTalentAttNumByTypeId(int attType, int attId)
    {
        if (_dictAttri.ContainsKey(attType))
        {
            if (_dictAttri[attType].ContainsKey(attId))
            {
                return _dictAttri[attType][attId];
            }
        }
        return 0;
    }

    protected override void DoClearData()
    {
        base.DoClearData();
        if (_dictTalents != null)
            _dictTalents.Clear();
        if (_dictAttri != null)
            _dictAttri.Clear();
        if (_dictTalentNum != null)
            _dictTalentNum.Clear();
        ParseTalent(TalentTypeConst.Basis);
        ParseTalent(TalentTypeConst.Guild);
    }
}
