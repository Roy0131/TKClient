using Msg.ClientMessage;
using System.Collections.Generic;

public enum PlayerInfoType
{
    Normal,
    FriendPlayer,
    FriendOther,
    GuildMember,
}

public class PlayerVO  : DataBaseVO
{
    public int mPlayerId { get; private set; }
    public string mPlayerName { get; private set; }
    public int mPlayerLevel { get; private set; }
    public int mBattlePower { get; private set; }
    public int mPlayerIcon { get; private set; }
    public int mGuildId { get; private set; }
    public string mGuildName { get; private set; }
    public PlayerInfoType mType { get; private set; }
    public GuildOfficeType mGuildOfficeType { get; set; }

    private IDictionary<int, PlayerTeamRole> _dictRoles;

    public PlayerVO(int playerId, PlayerInfoType type = PlayerInfoType.Normal)
    {
        mPlayerId = playerId;
        mType = type;
    }

    protected override void OnInitData<T>(T value)
    {
        S2CArenaPlayerDefenseTeamResponse data = value as S2CArenaPlayerDefenseTeamResponse;
        mPlayerName = data.PlayerName;
        mBattlePower = data.Power;
        mPlayerIcon = data.PlayerHead;
        mPlayerLevel = data.PlayerLevel;
        mGuildId = data.GuildId;
        mGuildName = data.GuildName;
        if (_dictRoles == null)
            _dictRoles = new Dictionary<int, PlayerTeamRole>();
        _dictRoles.Clear();
        for (int i = 0; i < data.DefenseTeam.Count; i++)
        {
            if (data.DefenseTeam[i] != null)
                _dictRoles.Add(data.DefenseTeam[i].Pos, data.DefenseTeam[i]);
        }
        //_dictRoles = data.DefenseTeam;
    }

    public PlayerTeamRole GetPlayerTeamRole(int posIndex)
    {
        if (_dictRoles == null || !_dictRoles.ContainsKey(posIndex))
            return null;
        return _dictRoles[posIndex];
    }
}