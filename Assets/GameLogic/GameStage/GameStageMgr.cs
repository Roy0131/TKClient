using System.Collections.Generic;

public enum StageType
{
    None,
    Login,
    Home,
    Battle,
}

public class GameStageMgr : Singleton<GameStageMgr>
{
    private Dictionary<StageType, BaseStage> _dictAllStages;
    private BaseStage _curStage;
    private StageType _curStageType;

    public void Init()
    {
        _curStage = null;
        _curStageType = StageType.None;
        _dictAllStages = new Dictionary<StageType, BaseStage>();
        _dictAllStages.Add(StageType.Home, new HomeStage());
        _dictAllStages.Add(StageType.Battle, new BattleStage());
        _dictAllStages.Add(StageType.Login, new LoginStage());
    }

    public void ChangeStage(StageType type)
    {
        if (_curStageType == type)
            return;
        if (!_dictAllStages.ContainsKey(type))
        {
            LogHelper.LogError("[stage type:" + type + " can't find, change stage failed!!!]");
            return;
        }
        if (_curStage != null)
            _curStage.Exit();
        _curStage = _dictAllStages[type];
        _curStage.Enter();
        _curStageType = type;
        if (_curStageType == StageType.Battle)
            SoundMgr.Instance.PlayEffectSound("UI_btn_battle");
    }

    public bool CheckInStage(StageType type)
    {
        if (_curStage == null)
            return false;
        return _curStageType == type;
    }

    public T GetStageByType<T>(StageType type) where T : BaseStage
    {
        if (_dictAllStages.ContainsKey(type))
            return (T)_dictAllStages[type];
        return null;
    }

    public void Update()
    {
        if (_curStage == null)
            return;
        _curStage.Update();
    }
}