
public class GameEventMgr : Singleton<GameEventMgr>
{
    public static readonly string GEventEnterBattle = "gEventEnterBattle";
    public static readonly string GEventClearAllData = "gEventClearAllData";

    public REventDispatcher mUIEvtDispatcher { get; private set; }
    public REventDispatcher mBattleDispatcher { get; private set; }
    public REventDispatcher mGlobalDispatcher { get; private set; }
    public REventDispatcher mGuideDispatcher { get; private set; }

    public void Init()
    {
        if (_blInited)
            return;
        mUIEvtDispatcher = new REventDispatcher();
        mBattleDispatcher = new REventDispatcher();
        mGlobalDispatcher = new REventDispatcher();
        mGuideDispatcher = new REventDispatcher();
        _blInited = true;
    }
}