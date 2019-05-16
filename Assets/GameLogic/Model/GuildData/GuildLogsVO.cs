using System;
using System.Collections.Generic;
using Msg.ClientMessage;
public class GuildLogsVO : DataBaseVO {
    public string mTimeTitle { get; private set; }
    public int mIdDay { get; private set; }
    public int mIdTime { get; private set; }
    public string mTimeFirst { get; private set; }
    public string mTextBehavior { get; private set; }
    protected override void OnInitData<T>(T value)
    {
        if (value.GetType() == typeof(GuildLog))
        {
            GuildLog gl = value as GuildLog;

            string _timeMonthDay = TimeHelper.GetTime(gl.Time);
            string[] _timeData =  _timeMonthDay.Split();

            string _month = _timeData[0].Split('-')[1];
            string _day = _timeData[0].Split('-')[2];
            mTimeTitle = _month + "-" + _day;
            mIdDay = Convert.ToInt32(_timeData[0].Split('-')[1] + _timeData[0].Split('-')[2]);
            mIdTime = Convert.ToInt32(_timeData[1].Split(':')[0]+ _timeData[1].Split(':')[1]);
            mTimeFirst = _timeData[1];
            mTextBehavior =string.Format(BehaviorDes(gl.Type),gl.PlayerName);
        }
    }
    /// <summary>
    /// 根据类型得到描述
    /// </summary>
    /// <param name="type"></param>
    /// <param name="_textDescription"></param>
    public string BehaviorDes(int type)
    {
        switch (type)
        {
            case 1:
                return LanguageMgr.GetLanguage(520001);
            case 2:
                return LanguageMgr.GetLanguage(520002);
            case 3:
                return LanguageMgr.GetLanguage(520003);
            case 4:
                return LanguageMgr.GetLanguage(520004);
            case 5:
                return LanguageMgr.GetLanguage(520005);
            case 6:
                return LanguageMgr.GetLanguage(520006);
            case 7:
                return LanguageMgr.GetLanguage(520007);
        }
        return "";
    }
}
