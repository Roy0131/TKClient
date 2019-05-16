using Msg.ClientMessage;
using System.Collections.Generic;

public class RedPointDataModel : ModelDataBase<RedPointDataModel>
{
    private Dictionary<RedPointEnum, bool> _dictRedStates;
    private List<int> _lstBitCode = new List<int>() { 0x01, 0x02, 0x04 };
    private uint _reqTimer = 0;

    protected override void OnInit()
    {
        _dictRedStates = new Dictionary<RedPointEnum, bool>();
        base.OnInit();
    }

    public void SetDynamicChildNodeState(RedPointEnum parentID, int childID, bool value)
    {
        RedPointTipsMgr.Instance.UpdateDynamicChildState(parentID, childID, value);
    }

    public void SetRedPointDataState(RedPointEnum redPointID, bool value)
    {
        if (_dictRedStates.ContainsKey(redPointID))
            _dictRedStates[redPointID] = value;
        else
            _dictRedStates.Add(redPointID, value);
        RedPointTipsMgr.Instance.UpdateRedPointState(redPointID, value);
    }

    public void ReqRedStates(params RedPointEnum[] args)
    {
        GameNetMgr.Instance.mGameServer.ReqRedPointState(args);
    }

    private void TimerReqRedData()
    {
        ReqRedStates();
    }

    private void OnRedPointData(S2CRedPointStatesResponse value)
    {
        if (_dictRedStates == null)
            _dictRedStates = new Dictionary<RedPointEnum, bool>();
        RedPointEnum redPoint;
        int idRoot;
        bool blValue;
        RedPointEnum parentRedID;
        for (int i = 1; i < value.States.Count; i++)
        {
            idRoot = i;
            parentRedID = RedPointHelper.GetRedPointEnum(idRoot);
            if (parentRedID == RedPointEnum.None)
                continue;
            blValue = (value.States[i] & 0x01) > 0;
            for (int j = 0; j < _lstBitCode.Count; j++)
            {
                idRoot = i * 100 + j + 1;
                redPoint = RedPointHelper.GetRedPointEnum(idRoot);
                if (redPoint == RedPointEnum.None)
                    continue;
                _dictRedStates[redPoint] = (value.States[i] & _lstBitCode[j]) > 0;
                blValue |= _dictRedStates[redPoint];
                SetRedPointDataState(redPoint, _dictRedStates[redPoint]);
            }
            _dictRedStates[parentRedID] = blValue;
            SetRedPointDataState(parentRedID, blValue);
        }
        if (_reqTimer != 0)
        {
            TimerHeap.DelTimer(_reqTimer);
            _reqTimer = 0;
        }
        _reqTimer = TimerHeap.AddTimer(120 * 1000, 0, TimerReqRedData);
    }

    public bool GetRedPointStatus(RedPointEnum redPoint)
    {
        if (_dictRedStates != null && _dictRedStates.ContainsKey(redPoint))
            return _dictRedStates[redPoint];
        return false;
    }

    public static void DoRedPointData(S2CRedPointStatesResponse value)
    {
        Instance.OnRedPointData(value);
    }
}