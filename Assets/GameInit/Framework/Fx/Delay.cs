public class Delay : RComponent
{
    public float delayTime = 1.0f;
    private uint _key = 0;
    private bool _blRunning = false;

    public void ReturnPool()
    {
        ClearTimer();
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
        _blRunning = false;
    }

    private void OnEnable()
    {
        if (_blRunning)
            return;
        if (gameObject.activeSelf)
            gameObject.SetActive(false);

        ClearTimer();
        _blRunning = true;
        uint time = (uint)(delayTime * 1000);
        _key = TimerHeap.AddTimer(time, 0, OnShow);
    }

    private void OnShow()
    {
        ClearTimer();
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _blRunning = false;
    }

    private void ClearTimer()
    {
        if (_key != 0)
        {
            TimerHeap.DelTimer(_key);
            _key = 0;
        }
    }

    public override void Dispose()
    {
        ClearTimer();
        base.Dispose();
    }
}