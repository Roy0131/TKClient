using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseStage 
{
    public StageType mStageType { get; protected set; }
    protected AsyncOperation _asyncOpera;
    protected bool _blLoadingScene;
    protected string _sceneName;

    public BaseStage(StageType type)
    {
        mStageType = type;
        _asyncOpera = null;
    }

    public void Enter()
    {
        OnEnter();
    }

    public void Update()
    {
        OnUpdate();
    }

    public void Exit()
    {
        OnExit();
    }

    protected virtual void OnEnter()
    {
        if (!string.IsNullOrEmpty(_sceneName))
        {
            _asyncOpera = SceneManager.LoadSceneAsync(_sceneName);
            _blLoadingScene = true;
        }
        else
        {
            LoadSceneFinish();
        }
    }

    protected virtual void LoadSceneFinish()
    {
    }

    protected virtual void OnUpdate()
    {
        if (_asyncOpera != null)
        {
            if (_blLoadingScene)
            {
                if (_asyncOpera.isDone)
                {
                    _blLoadingScene = false;
                    _asyncOpera = null;
                    LoadSceneFinish();
                }
            }
        }
    }

    protected virtual void OnExit()
    {
        _asyncOpera = null;
        _blLoadingScene = false;
        GameUIMgr.Instance.CloseAllModule();
        SoundMgr.Instance.StopAllSound();
    }
}