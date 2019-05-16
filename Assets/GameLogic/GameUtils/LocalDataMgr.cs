using UnityEngine;
using Msg.ClientMessage;
using System.Collections.Generic;
using System;

public enum TeamType : ushort
{
    Defense = 2,
    Hangeup = 3,
    Arena,
    Ctower,
    ActiveCopy,
    ExploreTask,
    ExploreStoryTask,
    FriendBattle,
    FriendBoss,
    FriendBossAssist,
    GuildBoss,
    Expedition,
}

public class LocalDataMgr : Singleton<LocalDataMgr>
{
    class LocalDataKeys
    {
        public const string PlayerAccount = "PlayerAccout";
        public const string PlayerId = "playerId";
        public const string PasswordKey = "passwordKey";
        public const string LoginChannelKey = "loginChannelKey";

        public const string CampaignIds = "CampaignIds";
        public const string ArenaCancelBattle = "arenaCancelBattle";
        public const string MusicSele = "musicSele";
        public const string SoundSele = "soundSele";

        public const string BattleTeam = "battleTeam";

        public const string FriendAssistCardKey = "friendAssistCardKey";

        public const string NewBieGuideIDKey = "newBieGuideIDKey";
        public const string NewBieGuideStepIDKey = "newBieGuideStepIDKey";

        public const string CurSpeedLevel = "curSpeedLevel";
        public const string LanguageKey = "languageKey";

        public const string ArtifactSele = "artifactSele";
    }

    public static void Init()
    {
        OnInitArtifactSelect();
        _playerAccount = PlayerPrefs.GetString(LocalDataKeys.PlayerAccount, SystemInfo.deviceUniqueIdentifier);
        _playerId = PlayerPrefs.GetInt(LocalDataKeys.PlayerId, 0);

        _password = PlayerPrefs.GetString(LocalDataKeys.PasswordKey, "");
        _loginChannel = PlayerPrefs.GetInt(LocalDataKeys.LoginChannelKey, 0);

        _campIdsValue = PlayerPrefs.GetString(LocalDataKeys.CampaignIds, "1");
        _arenaCancelBattleAlert = PlayerPrefs.GetInt(LocalDataKeys.ArenaCancelBattle, 0);
        _musicSele = PlayerPrefs.GetInt(LocalDataKeys.MusicSele, 0);
        _soundSele = PlayerPrefs.GetInt(LocalDataKeys.SoundSele, 0);

        _friendAssistCardId = PlayerPrefs.GetInt(LocalDataKeys.FriendAssistCardKey, 0);

        _guideID = PlayerPrefs.GetInt(LocalDataKeys.NewBieGuideIDKey, 1);
        _guideStepID = PlayerPrefs.GetInt(LocalDataKeys.NewBieGuideStepIDKey, 1);
        _speedLevel = PlayerPrefs.GetInt(LocalDataKeys.CurSpeedLevel, 1);
       // _artifactId = PlayerPrefs.GetInt(LocalDataKeys.ArtifactSele, 0);

#if UNITY_IPHONE
        SystemLanguage lang = Application.systemLanguage;  
        int tmpLanguage = PlayerPrefs.GetInt(LocalDataKeys.LanguageKey, (int)lang);
        _curLanguage = (SystemLanguage)tmpLanguage;
#elif UNITY_ANDROID || UNITY_EDITOR
        int tmpLanguage = PlayerPrefs.GetInt(LocalDataKeys.LanguageKey, (int)Application.systemLanguage);
        _curLanguage = (SystemLanguage)tmpLanguage;
#endif

        if (_guideStepID < 1)
            _guideStepID = 1;
        if (_guideID < 1)
            _guideID = 1;
    }

#region language

    private static SystemLanguage _curLanguage;
    public static SystemLanguage CurLanguage
    {
        get { return _curLanguage; }
        set
        {
            if (_curLanguage == value)
                return;
            _curLanguage = value;
            int language = (int)_curLanguage;
            PlayerPrefs.SetInt(LocalDataKeys.LanguageKey, language);
            PlayerPrefs.SetInt("languageKey", language); //兼容老版本母包逻辑
            PlayerPrefs.Save();
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(HeroEvent.SwitchLanguage);
        }
    }

    public static bool IsChinese
    {
        get { return _curLanguage == SystemLanguage.ChineseSimplified || _curLanguage == SystemLanguage.Chinese || _curLanguage == SystemLanguage.ChineseTraditional; }
    }
#endregion


#region newbie guide local data
    private static int _guideID;
    public static int NewBieGuideID
    {
        get { return _guideID; }
        set
        {
            if (_guideID == value)
                return;
            _guideID = value;
            PlayerPrefs.SetInt(LocalDataKeys.NewBieGuideIDKey, _guideID);
            PlayerPrefs.Save();
        }
    }

    private static int _guideStepID;
    public static int NewBieGuildStepID
    {
        get { return _guideStepID; }
        set
        {
            if (_guideStepID == value)
                return;
            _guideStepID = value;
            PlayerPrefs.SetInt(LocalDataKeys.NewBieGuideStepIDKey, _guideStepID);
            PlayerPrefs.Save();
        }
    }
#endregion

#region login data
    private static int _loginChannel;
    public static int LoginChannel
    {
        get { return _loginChannel; }
        set
        {
            if (_loginChannel == value)
                return;
            _loginChannel = value;
            PlayerPrefs.SetInt(LocalDataKeys.LoginChannelKey, _loginChannel);
            PlayerPrefs.Save();
        }
    }

    private static string _password;
    public static string Password
    {
        get { return _password; }
        set
        {
            _password = value;
            PlayerPrefs.SetString(LocalDataKeys.PasswordKey, _password);
            PlayerPrefs.Save();
        }
    }

    private static string _playerAccount;
    public static string PlayerAccount
    {
        get { return _playerAccount; }
        set
        {
            if (_playerAccount == value)
                return;
            DeleteLocalCache();
            _playerAccount = value;
            PlayerPrefs.SetString(LocalDataKeys.PlayerAccount, _playerAccount);
            PlayerPrefs.Save();
        }
    }

    private static int _playerId;
    public static int PlayerId
    {
        get { return _playerId; }
        set
        {
            if (_playerId == value)
                return;
            DeleteLocalCache();
            _playerId = value;
            PlayerPrefs.SetInt(LocalDataKeys.PlayerId, _playerId);
            PlayerPrefs.Save();
        }
    }

#endregion

#region battle team cache

    private static Dictionary<TeamType, IList<int>> _dictTeamCardId;
    public static void InitBattleTeam(IList<TeamData> value)
    {
        if (_dictTeamCardId != null)
            return;
        Dictionary<TeamType, IList<int>> dictTmp = new Dictionary<TeamType, IList<int>>();
        int i;
        for (i = 0; i < value.Count; i++)
            dictTmp.Add((TeamType)value[i].TeamType, value[i].TeamMembers);
        _dictTeamCardId = new Dictionary<TeamType, IList<int>>();
        ParseCacheTeam(TeamType.Defense, dictTmp[TeamType.Defense]);
        ParseCacheTeam(TeamType.Hangeup, dictTmp[TeamType.Hangeup]);
        for (i = 4; i <= 12; i++)
            ParseCacheTeam((TeamType)i);
    }

    private static void ParseCacheTeam(TeamType type, IList<int> members = null)
    {
        string caches = PlayerPrefs.GetString(LocalDataKeys.BattleTeam + (int)type, null);
        if (!string.IsNullOrEmpty(caches))
        {
            members = new List<int>();
            string[] tmp = caches.Split(',');
            for (int i = 0; i < tmp.Length; i++)
                members.Add(int.Parse(tmp[i]));
        }
        if (members != null)
            _dictTeamCardId.Add(type, members);
    }

    public static void AddBattleTeam(TeamType type, IList<int> battleCardIds)
    {
        IList<int> oldValues = new List<int>();
        if (_dictTeamCardId.ContainsKey(type))
            oldValues = _dictTeamCardId[type];
        bool blChanged = oldValues.Count != battleCardIds.Count;
        if (!blChanged)
        {
            int len = oldValues.Count;
            for (int i = 0; i < len; i++)
            {
                if (oldValues[i] != battleCardIds[i])
                {
                    blChanged = true;
                    break;
                }
            }
        }
        if (!blChanged)
            return;
        _dictTeamCardId[type] = battleCardIds;
        string cacheDatas = battleCardIds[0].ToString();
        for (int i = 1; i < battleCardIds.Count; i++)
            cacheDatas += ("," + battleCardIds[i]);
        string key = LocalDataKeys.BattleTeam + (int)type;
        PlayerPrefs.SetString(key, cacheDatas);
        PlayerPrefs.Save();
    }

    public static IList<int> GetBattleTeamCards(TeamType type)
    {
        if (_dictTeamCardId.ContainsKey(type))
            return _dictTeamCardId[type];
        return null;
    }

    public static void RemoveTeamCard(TeamType type,List<int> listId)
    {
        if (_dictTeamCardId.ContainsKey(type))
        {
            for (int i = _dictTeamCardId[type].Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < listId.Count; j++)
                {
                    if (_dictTeamCardId[type][i] == listId[j])
                        _dictTeamCardId[type][i] = 0;
                }
            }
        }
    }

    public static bool CheckCardInBattle(int cardId)
    {
        if (_friendAssistCardId == cardId)
            return true;
        IList<int> members;
        for (int i = 2; i <= 3; i++)
        {
            members = GetBattleTeamCards((TeamType)i);
            if (members.Contains(cardId))
                return true;
        }
        return false;
    }

    private static int _friendAssistCardId;
    public static int FriendAssistCardId
    {
        get { return _friendAssistCardId; }
        set
        {
            if (_friendAssistCardId == value)
                return;
            _friendAssistCardId = value;
            PlayerPrefs.SetInt(LocalDataKeys.FriendAssistCardKey, _friendAssistCardId);
            PlayerPrefs.Save();
        }
    }

#endregion

#region campaign logic
    private static string _campIdsValue;
    public static bool CheckCampFirstBattle(int campId)
    {
        return _campIdsValue.Contains(campId.ToString());
    }

    public static void SaveCampaignID(int campId)
    {
        if (string.IsNullOrEmpty(_campIdsValue))
            _campIdsValue = campId.ToString();
        else
            _campIdsValue += ("," + campId.ToString());
        PlayerPrefs.SetString(LocalDataKeys.CampaignIds, _campIdsValue);
        PlayerPrefs.Save();
    }
#endregion

    private static int _arenaCancelBattleAlert;
    public static bool ArenaCancelBattleAlert
    {
        get
        {
            return _arenaCancelBattleAlert == 0;
        }
        set
        {
            _arenaCancelBattleAlert = value ? 0 : 1;
            PlayerPrefs.SetInt(LocalDataKeys.ArenaCancelBattle, _arenaCancelBattleAlert);
            PlayerPrefs.Save();
        }
    }

    private static int _musicSele;
    public static bool IsMusic
    {
        get
        {
            return _musicSele == 0;
        }
        set
        {
            _musicSele = value ? 0 : 1;
            PlayerPrefs.SetInt(LocalDataKeys.MusicSele, _musicSele);
            PlayerPrefs.Save();
        }
    }

    private static int _soundSele;
    public static bool IsSound
    {
        get
        {
            return _soundSele == 0;
        }
        set
        {
            _soundSele = value ? 0 : 1;
            PlayerPrefs.SetInt(LocalDataKeys.SoundSele, _soundSele);
            PlayerPrefs.Save();
        }
    }

    private static int _speedLevel;
    public static int SpeedLevel
    {
        get { return _speedLevel; }
        set
        {
            if (_speedLevel == value)
                return;
            _speedLevel = value;
            PlayerPrefs.SetInt(LocalDataKeys.CurSpeedLevel, _speedLevel);
            PlayerPrefs.Save();
        }
    }

//    private static int _artifactId;
//    public static int ArtifactId
//    {
//        get { return _artifactId; }
//        set
//        {
//            if (_artifactId == value)
//                return;
//            _artifactId = value;
//            PlayerPrefs.SetInt(LocalDataKeys.ArtifactSele, _artifactId);
//            PlayerPrefs.Save();
//        }
//    }

    private static Dictionary<TeamType, IList<int>> _dictArtifactSelect;
    public static void OnInitArtifactSelect()
    {
        if (_dictArtifactSelect == null)
            _dictArtifactSelect = new Dictionary<TeamType, IList<int>>();
        IList<int> mlst ;//= new List<int>();
        //foreach (TeamType artifact in Enum.GetValues(typeof(TeamType)))
        TeamType type;
        for (int i = 2; i <= 12; i++)
            //ParseCacheTeam((TeamType)i);
        {
            int artifactId = PlayerPrefs.GetInt(LocalDataKeys.ArtifactSele + i, 0);
            mlst = new List<int>();
            mlst.Add(artifactId);
            type = (TeamType) i;
            if (_dictArtifactSelect.ContainsKey(type))
                _dictArtifactSelect[type] = mlst; //artifactId;
            else
                _dictArtifactSelect.Add(type, mlst);
        }
    }

    public static void AddArtifactSelect(TeamType type, int artifactId)
    {
        IList<int> lst = new List<int>();
        lst.Add(artifactId);
        if (_dictArtifactSelect.ContainsKey(type))
            _dictArtifactSelect[type] = lst; //artifactId;
        else
            _dictArtifactSelect.Add(type, lst);//artifactId);
        string key = LocalDataKeys.ArtifactSele + (int)type;
        PlayerPrefs.SetInt(key, artifactId);
        PlayerPrefs.Save();
    }

    public static int GetArtifactSele(TeamType type)
    {
        if (_dictArtifactSelect.ContainsKey(type))
            return _dictArtifactSelect[type][0];
        return 0;
    }


    public static void DeleteLocalCache()
    {
        PlayerPrefs.DeleteKey(LocalDataKeys.CampaignIds);
        PlayerPrefs.DeleteKey(LocalDataKeys.ArenaCancelBattle);
        PlayerPrefs.DeleteKey(LocalDataKeys.MusicSele);
        PlayerPrefs.DeleteKey(LocalDataKeys.SoundSele);
        PlayerPrefs.DeleteKey(LocalDataKeys.FriendAssistCardKey);
        PlayerPrefs.DeleteKey(LocalDataKeys.NewBieGuideIDKey);
        PlayerPrefs.DeleteKey(LocalDataKeys.NewBieGuideStepIDKey);
        PlayerPrefs.DeleteKey(LocalDataKeys.CurSpeedLevel);
        //ChatModel.Instance.DeleteChatCacheData();
        for (int j = 2; j <= 12; j++)
            PlayerPrefs.DeleteKey(LocalDataKeys.BattleTeam + j);
        if (_dictTeamCardId != null)
        {
            _dictTeamCardId.Clear();
            _dictTeamCardId = null;
        }
        foreach (var item in _dictArtifactSelect)
            PlayerPrefs.DeleteKey(LocalDataKeys.ArtifactSele + (int)item.Key);
        if (_dictArtifactSelect != null)
        {
            _dictArtifactSelect.Clear();
            _dictArtifactSelect = null;
        }
        Init();
    }


    public static Dictionary<int, OrderCacheData> mDictOrderDatas = new Dictionary<int, OrderCacheData>();
    private static int _orderKey = 10000;

    public static bool ParseOrderCache()
    {
        string orders = PlayerPrefs.GetString("ibordercache");
        //LogHelper.Log("[LocalDataMgr.ParseOrderCache() => read local order info data, value:" + orders + "]");
        if (!string.IsNullOrEmpty(orders))
        {
            int orderId = 0;
            string[] tmp = orders.Split('|');
            if (tmp.Length > 0)
            {
                OrderCacheData orderData;
                for (int i = 0; i < tmp.Length; i++)
                {
                    orderData = new OrderCacheData();
                    bool blInvalid = orderData.SetData(tmp[i]);
                    if (!blInvalid)
                        continue;
                    orderId = orderData.mCacheOrderID;//int.Parse(orderData.mCacheOrderID);
                    _orderKey = Mathf.Max(orderId, _orderKey);
                    mDictOrderDatas.Add(orderData.mCacheOrderID, orderData);
                }
                return mDictOrderDatas.Count > 0;
            }
        }
        return false;
    }

    public static void RemoveAllOrderData()
    {
        if (mDictOrderDatas.Count > 0)
            mDictOrderDatas.Clear();
        SaveOrderToCache();
    }
    public static void RemoveOrderByKey(int key)
    {
        if (!mDictOrderDatas.ContainsKey(key))
            return;
        mDictOrderDatas.Remove(key);
        SaveOrderToCache();
    }

    private static void SaveOrderToCache()
    {
        string value = "";
        if (mDictOrderDatas.Count > 0)
        {
            Dictionary<int, OrderCacheData>.KeyCollection keyColl = mDictOrderDatas.Keys;
            foreach (int key in keyColl)
            {
                if (string.IsNullOrEmpty(value))
                    value = mDictOrderDatas[key].ToString();
                else
                    value += ("|" + mDictOrderDatas[key].ToString());
            }
        }
        PlayerPrefs.SetString("ibordercache", value);
    }

    public static int AddOrderInfo(string payload, string value, int channel, string bundleid)
    {
        _orderKey += 1;
        OrderCacheData orderData = new OrderCacheData();
        orderData.mPayLoad = payload;
        orderData.mOrderData = value;
        orderData.mOrderChannel = channel;
        orderData.mCacheOrderID = _orderKey;
        orderData.mBundleId = bundleid;
        mDictOrderDatas.Add(orderData.mCacheOrderID, orderData);
        SaveOrderToCache();
        //LogHelper.LogWarning("[LocalDataMgr.AddOrderInfo() => sava order info, _orderKey:"+ _orderKey + ", payload:"+ payload + "]");
        return orderData.mCacheOrderID;
    }
}

public class OrderCacheData
{
    public int mCacheOrderID;
    public int mOrderChannel;
    public string mOrderData;
    public string mBundleId;
    public string mPayLoad;

    private string _dataString;

    public bool SetData(string value)
    {
        string[] tmps = value.Split(':');
        if (tmps.Length != 5)
            return false;
        mCacheOrderID = int.Parse(tmps[0]);
        mOrderChannel = int.Parse(tmps[1]);
        mOrderData = tmps[2];
        mBundleId = tmps[3];
        mPayLoad = tmps[4];
        _dataString = value;
        return true;
    }

    public override string ToString()
    {
        if (string.IsNullOrEmpty(_dataString))
            _dataString = mCacheOrderID + ":" + mOrderChannel + ":" + mOrderData + ":" + mBundleId + ":" + mPayLoad;
        return _dataString;
    }
}