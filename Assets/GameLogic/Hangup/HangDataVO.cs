using LitJson;
using UnityEngine;
using System.Collections.Generic;

public class HangDataVO
{
    public List<CardConfig> mlstRoleCards { get; private set; }
    public List<CardConfig> mlstBossCards { get; private set; }
    public Dictionary<int, CardDataVO> mDictMonsters { get; private set; }
    private int _stageID;
    public HangDataVO()
    {
        mlstBossCards = new List<CardConfig>();
        mlstRoleCards = new List<CardConfig>();
        mDictMonsters = new Dictionary<int, CardDataVO>();
    }

    public void Reset()
    {
        mlstRoleCards.Clear();
        mlstBossCards.Clear();
        mDictMonsters.Clear();
    }

    public int StageID
    {
        get { return _stageID; }
        set
        {
            _stageID = value;
            //Debuger.Log("stageid:" + _stageID);
            mlstBossCards.Clear();
            mDictMonsters.Clear();
            StageConfig stageCfg = GameConfigMgr.Instance.GetStageConfig(_stageID);
            JsonData allMonsters = JsonMapper.ToObject(stageCfg.MonsterList);
            JsonData jd;
            CardDataVO vo;
            int monsterId, rank, monsterCardId;
            CardConfig enemyCfg;
            int level;
            int index;
            for (int i = 0; i < allMonsters.Count; i++)
            {
                jd = allMonsters[i];
                monsterId = (int)jd["MonsterID"];
                rank = (int)jd["Rank"];
                level = (int)jd["Level"];
                index = (int)jd["Slot"];
                monsterCardId = monsterId * 100 + rank;
                vo = new CardDataVO(monsterId, rank, level);
                mDictMonsters.Add(index - 1, vo);
                enemyCfg = GameConfigMgr.Instance.GetCardConfig(monsterCardId);
                AddBossCard(enemyCfg);
            }
        }
    }

    public CardConfig GetRandomBossCard()
    {
        if(mlstBossCards.Count == 0)
        {
            LogHelper.LogError("[HangDataVO.GetRandomBossCard() => boss data count was zero!!!]");
            return null;
        }
        int idx = Random.Range(0, mlstBossCards.Count);
        return mlstBossCards[idx];
    }

    public void AddBossCard(params CardConfig[] args)
    {
        if (args != null)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (mlstBossCards.Contains(args[i]))
                    continue;
                mlstBossCards.Add(args[i]);
            }
        }
    }

    public void RemoveBossCard(params CardConfig[] args)
    {
        if (args != null)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (!mlstRoleCards.Contains(args[i]))
                    continue;
                mlstRoleCards.Remove(args[i]);
            }
        }
    }

    public void AddRole(params CardConfig[] args)
    {
        if(args != null)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (mlstRoleCards.Contains(args[i]))
                    continue;
                mlstRoleCards.Add(args[i]);
            }
        }
    }

    public void RemoveRole(params CardConfig[] args)
    {
        if (args != null)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (!mlstRoleCards.Contains(args[i]))
                    continue;
                mlstRoleCards.Remove(args[i]);
            }
        }
    }
}