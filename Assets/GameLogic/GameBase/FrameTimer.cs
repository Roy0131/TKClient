using System;
using Framework.Core;
using System.Collections.Generic;

public class FrameTimer : IDispose
{
    private int _frames;
    private Action _onTimeEnd;
    public bool mBlEnable { get; private set; }
    private List<int> _intervals = new List<int>();//<int>();
    private int _index = 0;

    public FrameTimer(int frames, Action endMethod)
    {
        _index = 0;
        _frames = frames;
        _onTimeEnd = endMethod;
        mBlEnable = true;
    }

    public FrameTimer(string frames, Action endMethod)
    {
        _onTimeEnd = endMethod;
        Reset(frames);
    }

    public void Update()
    {
        if (!mBlEnable)
            return;
        if(_frames <= 0)
        {
            if(_onTimeEnd != null)
                _onTimeEnd.Invoke();
            if (_index >= _intervals.Count - 1) //_intervals.Count <= 0)
            {
                mBlEnable = false;
            }
            else
            {
                _index++;
                _frames = _intervals[_index]; //.Dequeue();
            }
            return;
        }
		_frames-=(int)UnityEngine.Time.timeScale;
    }

    public void Reset(string frames)
    {
        _index = 0;   
        _intervals.Clear();
        string[] t = frames.Split(',');
        int interval;
        int lastFrame = 0;
        if (t.Length == 0)
        {
            _intervals.Add(0);//.Enqueue(0);
        }
        else
        {
            for (int i = 0; i < t.Length; i++)
            {
                interval = 0;
                int.TryParse(t[i], out interval);
                if (interval != 0)
                {
                    _intervals.Add(interval - lastFrame);
                    lastFrame = interval;
                }
                else
                {
                    _intervals.Add(0);
                    lastFrame = 0;
                }
            }
        }
        _frames = _intervals[_index];
        mBlEnable = true;
    }

    public void Reset(int frame)
    {
        _frames = frame;
        mBlEnable = true;
    }

    public void Dispose()
    {
        _onTimeEnd = null;
        if (_intervals != null)
        {
            _intervals.Clear();
            _intervals = null;
        }
        mBlEnable = false;
    }
}
