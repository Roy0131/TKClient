using Msg.ClientMessage;

public class PlayerInfoDataModel : ModelDataBase<PlayerInfoDataModel>
{
    private PlayerVO _playerVO;

    public void ShowPlayerInfo(PlayerVO vo)
    {
        _playerVO = vo;
        GameNetMgr.Instance.mGameServer.ReqPlayerDefenseTeam(_playerVO.mPlayerId);
    }

    public void ShowPlayerInfo(int playerId, PlayerInfoType type = PlayerInfoType.Normal)
    {
        ShowPlayerInfo(new PlayerVO(playerId, type));
    }

    public static void DoPlayerDefenseTeam(S2CArenaPlayerDefenseTeamResponse value)
    {
        if (Instance._playerVO == null)
            return;
        Instance._playerVO.InitData(value);
        PlayerInfoMgr.Instance.ShowPlayerInfo(Instance._playerVO);
        Instance._playerVO = null;
    }
}