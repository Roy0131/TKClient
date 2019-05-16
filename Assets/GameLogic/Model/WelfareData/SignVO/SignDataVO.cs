using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg.ClientMessage;

public class SignDataVO : DataBaseVO
{
    public List<SignConfig> mListSignConfig { get; private set; }
    public int mMinIndex { get; private set; }
    public int mMaxIndex { get; private set; }
    public int mSignTime { get; private set; }
    private int _curGroup = 0;

    protected override void OnInitData<T>(T value)
    {
        S2CSignDataResponse req = value as S2CSignDataResponse;
        mMinIndex = req.TakeAwardIndex;
        mMaxIndex = req.SignedIndex;
        mSignTime = req.NextSignRemainSeconds + (int)Time.realtimeSinceStartup;
        OnListConfig(mMinIndex);
    }

    private void OnListConfig(int id)
    {
        SignConfig cfg = GameConfigMgr.Instance.GetSignConfig(id + 1);
        int group = cfg.Group;
        if (_curGroup != group)
        {
            if (mListSignConfig != null)
                mListSignConfig.Clear();
            mListSignConfig = new List<SignConfig>();
            Dictionary<int, SignConfig> AllDatas = SignConfig.Get();
            foreach (SignConfig Config in AllDatas.Values)
            {
                if (Config.Group == group)
                    mListSignConfig.Add(Config);
            }
            mListSignConfig.Sort((x, y) => x.TotalIndex.CompareTo(y.TotalIndex));
        }
        _curGroup = group;
    }

    public int SignTime
    {
        get { return mSignTime - (int)Time.realtimeSinceStartup; }
    }

    public void OnAward(int indexs)
    {
        mMinIndex = indexs;
        OnListConfig(mMinIndex);
    }
}
