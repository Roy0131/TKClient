using Framework.UI;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene
{
    private List<Fighter> _lstHeroFighters;
    private List<Fighter> _lstTargetFighters;

    private List<Vector3> _lstRightCommonSeatPos;
    private List<Vector3> _lstLeftCommonSeatPos;

    private List<Transform> _lstLeftSeatRoot;
    private List<Transform> _lstRightSeatRoot;

    public Transform mBulletRoot { get; private set; }

    private SpriteRenderer _mapRender;

    private CameraShake _cameraShake;

    public BattleScene()
    {
        Transform battleRoot = GameObject.Find("BattleRoot").transform;
        Transform leftFighterRoot = battleRoot.Find("SeatsPosRoot/LeftRoot");
        Transform rightFighterRoot = battleRoot.Find("SeatsPosRoot/RightRoot");
        mBulletRoot = battleRoot.Find("BulletRoot");
        _mapRender = GameObject.Find("Map").GetComponent<SpriteRenderer>();

        Camera cam = GameObject.Find("BattleCamera").GetComponent<Camera>();
        cam.orthographicSize = GameUIMgr.Instance.blPadMode ? 5.2f : 3.9f;

        _lstRightCommonSeatPos = new List<Vector3>();
        _lstLeftCommonSeatPos = new List<Vector3>();
        int i;
        for (i = 0; i < 3; i++)
        {
            _lstLeftCommonSeatPos.Add(leftFighterRoot.Find("c" + (i + 1)).position);
            _lstRightCommonSeatPos.Add(rightFighterRoot.Find("c" + (i + 1)).position);
        }

        _lstLeftSeatRoot = new List<Transform>();
        _lstRightSeatRoot = new List<Transform>();
        for (i = 0; i < 9; i++)
        {
            _lstLeftSeatRoot.Add(leftFighterRoot.Find("p" + (i + 1)).transform);
            _lstRightSeatRoot.Add(rightFighterRoot.Find("p" + (i + 1)).transform);
        }
        _cameraShake = new CameraShake();//battleRoot.gameObject.AddComponent<CameraShake>();
        InitFighters();
    }

    public void ShakeCamera()
    {
        if (_cameraShake == null)
            return;
        _cameraShake.StartShake();
    }

    private void InitFighters()
    {
        _lstHeroFighters = new List<Fighter>();
        _lstTargetFighters = new List<Fighter>();
        CreateFighter(BattleDataModel.Instance.mlstHeroFighterDatas);
        CreateFighter(BattleDataModel.Instance.mlstTargetFighterDatas);

        string mapName = "Map_Battle_jdc";
        string bgSound = "Music_ZD";
        StageConfig stageConfig = null;
        switch(BattleDataModel.Instance.mBattleType)
        {
            case BattleType.Campaign:
                CampaignConfig campaignConfig = GameConfigMgr.Instance.GetCampaignByCampaignId(BattleDataModel.Instance.mBattleParam);
                stageConfig = GameConfigMgr.Instance.GetStageConfig(campaignConfig.StageID);
                break;
            case BattleType.CTower:
                TowerConfig cfg = GameConfigMgr.Instance.GetTowerConfig(BattleDataModel.Instance.mBattleParam);
                stageConfig = GameConfigMgr.Instance.GetStageConfig(cfg.StageID);
                break;
            case BattleType.ActivityCopy:
                stageConfig = GameConfigMgr.Instance.GetStageConfig(ActivityCopyDataModel.Instance.stageId);
                break;
            case BattleType.ExploreTask:
            case BattleType.ExploreStoryTask:
                stageConfig = GameConfigMgr.Instance.GetStageConfig(ExploreDataModel.Instance.mStageId);
                break;
            case BattleType.FriendBoss:
                stageConfig = GameConfigMgr.Instance.GetStageConfig(FriendDataModel.Instance.stageId);
                break;
            case BattleType.GuildBoss:
                stageConfig = GameConfigMgr.Instance.GetStageConfig(GuildBossDataModel.Instance.stageId);
                break;
            case BattleType.Expedition:
                mapName = GameConfigMgr.Instance.GetExpeditionConfig(ExpeditionDataModel.Instance.mCurCfgId + 1).BackGroundMap;
                break;
        }
        if (stageConfig != null)
            mapName = stageConfig.BackGroundMap;
        GameResMgr.Instance.LoadMapImage(mapName, (spr) =>
            {
                _mapRender.sprite = spr;
                float cameraHeight = Camera.main.orthographicSize * 2;
                Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
                Vector2 spriteSize = _mapRender.sprite.bounds.size;

                Vector2 scale = _mapRender.transform.localScale;
                if (cameraSize.x >= cameraSize.y)
                { 
                    scale *= cameraSize.x / spriteSize.x;
                }
                else
                { 
                    scale *= cameraSize.y / spriteSize.y;
                }
                _mapRender.transform.localScale = scale;
            }
        );
        SoundMgr.Instance.PlayBackGroundSound(bgSound);
    }

    public List<Fighter> CreateFighter(List<FighterDataVO> value, string bornEffect = null)
    {
        bool blHero;
        Fighter fighter;
        Transform parent;
        List<Fighter> lst = new List<Fighter>();
        for(int i = 0; i < value.Count; i++)
        {
            blHero = BattleDataModel.Instance.IsHeroFighter(value[i].mSide);
            fighter = new Fighter(bornEffect);
            fighter.InitData(value[i]);
            if (blHero)
            {
                parent = _lstLeftSeatRoot[value[i].mSeatIndex];
                _lstHeroFighters.Add(fighter);
            }
            else
            {
                parent = _lstRightSeatRoot[value[i].mSeatIndex];
                _lstTargetFighters.Add(fighter);
            }
            fighter.AddToStage(parent);
            lst.Add(fighter);
        }
        return lst;
    }

    public void RemoveFighter(Fighter fighter)
    {
        bool blHero = BattleDataModel.Instance.IsHeroFighter(fighter.mData.mSide);
        if (blHero)
            _lstHeroFighters.Remove(fighter);
        else
            _lstTargetFighters.Remove(fighter);
        fighter.Dispose();
    }

    public Fighter GetFighterBySeatIndex(int seatIndex, bool blHero)
    {
        List<Fighter> lst = blHero ? _lstHeroFighters : _lstTargetFighters;
        for (int i = 0; i < lst.Count; i++)
        {
            if (lst[i].mData.mSeatIndex == seatIndex)
                return lst[i];
        }
        return null;
    }

    public Fighter GetFighterBySeatIndex(int side, int seatIndex)
    {
        if (seatIndex < 0)
            return null; //神器,无实例Fighter对象
        bool blHero = BattleDataModel.Instance.IsHeroFighter(side);

        return GetFighterBySeatIndex(seatIndex, blHero);
    }

	//JZH重写MoveToSeatPosition,直接根据INDEX取位置
	public Vector3 GetMoveToSeatPosition(int mSide, int mSeatIndex)
    {
		int index = mSeatIndex % 3;
		if (BattleDataModel.Instance.IsHeroFighter(mSide))
            return _lstLeftCommonSeatPos[index];
        else
            return _lstRightCommonSeatPos[index];
    }

    public Vector3 GetSkillTargetPosByRangeType(int side, int seatIndex, int rangeType)
    {
        bool blHero = BattleDataModel.Instance.IsHeroFighter(side);
        int index = 0;//
		index = GetSkillTargetIndex( seatIndex, rangeType);
        return blHero ? _lstLeftSeatRoot[index].position : _lstRightSeatRoot[index].position;
    }
    
	public int GetSkillTargetIndex( int seatIndex, int rangeType) //JZH：新增加一个取INDEX函数
	{
		int index = seatIndex;//
		if (rangeType == SkillRangeType.Col)
			index = (int)(seatIndex / 3) * 3 + 1;
		else if (rangeType == SkillRangeType.All)
			index = 4;
		return index;
	}

    public void Update()
    {
        int i = 0;
        for (i = 0; i < _lstHeroFighters.Count; i++)
            _lstHeroFighters[i].Update();
        for (i = 0; i < _lstTargetFighters.Count; i++)
            _lstTargetFighters[i].Update();
    }

    public void Dispose()
    {
        int i = 0;
        if (_lstHeroFighters != null)
        {
            for (i = _lstHeroFighters.Count - 1; i >= 0; i--)
            {
                _lstHeroFighters[i].Dispose();
                _lstHeroFighters[i] = null;
            }
            _lstHeroFighters.Clear();
            _lstHeroFighters = null;
        }
        if (_lstTargetFighters != null)
        {
            for (i = _lstTargetFighters.Count - 1; i >= 0; i--)
            {
                _lstTargetFighters[i].Dispose();
                _lstTargetFighters[i] = null;
            }
            _lstTargetFighters.Clear();
            _lstTargetFighters = null;
        }
        if (_lstRightSeatRoot != null)
        {
            _lstRightSeatRoot.Clear();
            _lstRightSeatRoot = null;
        }
        if (_lstLeftSeatRoot != null)
        {
            _lstLeftSeatRoot.Clear();
            _lstLeftSeatRoot = null;
        }
        if (_lstRightCommonSeatPos != null)
        {
            _lstRightCommonSeatPos.Clear();
            _lstRightCommonSeatPos = null;
        }
        if (_lstLeftCommonSeatPos != null)
        {
            _lstLeftCommonSeatPos.Clear();
            _lstLeftCommonSeatPos = null;
        }
        if (_cameraShake != null)
        {
            _cameraShake.Dispose();
            _cameraShake = null;
        }
        mBulletRoot = null;
    }
}
