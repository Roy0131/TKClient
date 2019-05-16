using System;
using UnityEngine;

public class HangupBullet : SceneRenderUnit
{
    public HangupFighter targeter;
    public HangupFighter attacker;

    private BulletType _bulletType;
    private SkillConfig _skillConfig;
    private Vector3 _direction;
    private int _flBulletAliveTime;
    private float _flBulletSpeed;
    private bool _blBulletEnd;
    public HangupBullet()
        : base(BattleUnitType.HangupBullet)
    {

    }

    #region Init Logic
    protected override void OnInitData<T>(T data)
    {
        _skillConfig = data as SkillConfig;
        _bulletType = (BulletType)_skillConfig.BulletType;
        if (_bulletType != BulletType.FiexedEffect)
            _modelName = _skillConfig.BulletAnim.Trim();
        _flBulletAliveTime = _skillConfig.BulletContinueTime;
        _flBulletSpeed = _skillConfig.BulletSpeed;
        base.OnInitData(data);
        //if (_modelObject == null)
        //_container = new GameObject("");
        //_container.name = "FiexedEffect";
        _container.layer = GameLayer.ModeUILayer;
        mUnitRoot = _container.transform;
        _blStart = false;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        SortLayer = -1;
        if (_container.transform.parent != null && !_blStart)
            OnStart();
    }

    protected override void LoadModel()
    {
        if (string.IsNullOrEmpty(_modelName))
            return;
        _container.name = _modelName;
        GameResMgr.Instance.LoadEffect(_modelName, OnModelLoaded);
    }
    #endregion

    private void OnStart()
    {
        _blBulletEnd = false;
        _blStart = true;
        switch (_bulletType)
        {
            case BulletType.Bomb:
            case BulletType.Linear:
                OnLinearBulletStart();
                break;
            case BulletType.LineEffect:
                OnLineEffectBullet();
                break;
            case BulletType.SelfEffect:
            case BulletType.FiexedEffect:
                OnSelfEffectStart();
                break;
        }
    }

    #region line effect bullet logic
    private void OnLineEffectBullet()
    {
        _direction = (targeter.mHitWorldPosition - attacker.FireDummyWorldPosition).normalized;
        float temp = Mathf.Atan2(_direction.y, _direction.x);
        float flDegress = temp * 180f / (float)Math.PI;
        mUnitRoot.transform.localEulerAngles = Vector3.zero;
        mUnitRoot.transform.Rotate(0f, 0f, flDegress);

        float value = Vector3.Distance(targeter.mHitWorldPosition, attacker.FireDummyWorldPosition);
        Transform line = _modelObject.transform.Find("line");
        line.localScale = new Vector3(value, line.localScale.y, line.localScale.z);

        DoEffectBulletHit();
    }

    #endregion

    #region Linear bullet logic
    private void OnLinearBulletStart()
    {
        _direction = (targeter.mHitWorldPosition - attacker.FireDummyWorldPosition).normalized;
        float temp = Mathf.Atan2(_direction.y, _direction.x);
        float flDegress = temp * 180f / (float)Math.PI;
        mUnitRoot.transform.localEulerAngles = Vector3.zero;
        mUnitRoot.transform.Rotate(0f, 0f, flDegress);
        _blMoveEnd = false;
    }

    private float _frameSpeed;
    private bool _blMoveEnd = false;
    private void DoLinearBulletUpdate()
    {
        if (_blMoveEnd)
            return;
        _frameSpeed = _flBulletSpeed * Time.deltaTime;
        mUnitRoot.position += _direction * _frameSpeed;
        float distance = Vector3.Distance(mUnitRoot.position, targeter.mHitWorldPosition);
        if (distance <= _frameSpeed)
        {
            mUnitRoot.position = targeter.mHitWorldPosition;
            _blMoveEnd = true;
            DoTargetBehit(); 
        }   
    }
    #endregion

    #region self effect bullet
    private FrameTicker _damageTicker;
    private void OnSelfEffectStart()
    {
        string[] hitTimes = _skillConfig.HitShowTime.Split(',');
        _damageTicker = new FrameTicker(float.Parse(hitTimes[0]) / 1000f, DoEffectBulletHit);
    }

    private void OnSelfEffectUpdate()
    {
        if (_damageTicker != null && _damageTicker.mBlEnable)
            _damageTicker.Update();
    }

    #endregion

    private void DoEffectBulletHit()
    {
        targeter.DoBehit(_skillConfig.ChaHitEffect, _skillConfig.ChaHitSound);
        if (!string.IsNullOrEmpty(_skillConfig.BulletHitEffect))
        {
            OnCreateHitEffect();
        }
        else
        {
            if (_skillConfig.BulletContinueTime == 0)
                OnBulletEnd();
        }
    }

    private uint _hitEffectKey = 0;
    private void DoTargetBehit()
    {
        targeter.DoBehit(_skillConfig.ChaHitEffect, _skillConfig.ChaHitSound);
        if(string.IsNullOrEmpty(_skillConfig.BulletHitEffect))
			OnBulletEnd();
        else
            OnCreateHitEffect();
    }

    private GameObject _hitEffectObject;
    private void OnCreateHitEffect()
    {
        if (_skillConfig.BulletContinueTime == 0)
            _container.gameObject.SetActive(false);
        if (!string.IsNullOrEmpty(_skillConfig.BulletHitSound))
            SoundMgr.Instance.PlayEffectSound(_skillConfig.BulletHitSound);
        EffectConfig cfg = GameConfigMgr.Instance.GetEffectConfig(_skillConfig.BulletHitEffect);
        if (cfg == null)
        {
            OnBulletEnd();
            return;
        }

        Action<GameObject> OnHitEffectLoaded = (modelObj) =>
        {
            _hitEffectObject = modelObj;
            if (_hitEffectObject == null)
            {
                LogHelper.LogWarning("[HangupBullet.OnCreateHitEffect() => bullet effect res load error, res name:" + _skillConfig.BulletHitEffect + "]");
                OnBulletEffectEnd();
                return;
            }
            _hitEffectObject.transform.SetParent(HangUpMgr.Instance.BulletRoot, false);
            _hitEffectObject.transform.position = targeter.mUnitRoot.position;
            _hitEffectKey = TimerHeap.AddTimer((uint)cfg.Duration, 0, OnBulletEffectEnd);
            if (_bulletType == BulletType.FiexedEffect)
            {
                Vector3 scale = attacker.BlHero ? Vector3.one : new Vector3(-1f, 1f, 1f);
                _hitEffectObject.transform.localScale = scale;
            }

            Transform objTransform = _hitEffectObject.transform;
            Renderer[] rootRenders = objTransform.GetComponents<Renderer>();
            Renderer[] childRenders = objTransform.GetComponentsInChildren<Renderer>();

            int rootLen = rootRenders == null ? 0 : rootRenders.Length;
            int childLen = childRenders == null ? 0 : childRenders.Length;

            Renderer[] renders = new Renderer[rootLen + childLen];
            if (rootLen > 0)
                rootRenders.CopyTo(renders, 0);
            if (childLen > 0)
                childRenders.CopyTo(renders, rootLen);

            for (int i = 0; i < renders.Length; i++)
            {
                renders[i].gameObject.layer = GameLayer.ModeUILayer;
                renders[i].sortingOrder = _sortLayer + 1;
            }
        };

        GameResMgr.Instance.LoadEffect(_skillConfig.BulletHitEffect, OnHitEffectLoaded);
    }

    private void OnBulletEffectEnd()
    {
        TimerHeap.DelTimer(_hitEffectKey);
        GameObject.Destroy(_hitEffectObject);
        _hitEffectObject = null;
        _hitEffectKey = 0;
        OnBulletEnd();
    }

    protected override void OnUpdate()
    {
        if (_blBulletEnd)
            return;
        base.OnUpdate();
        switch (_bulletType)
        {
            case BulletType.Bomb:
            case BulletType.Linear:
                DoLinearBulletUpdate();
                break;
            case BulletType.FiexedEffect:
            case BulletType.SelfEffect:
                OnSelfEffectUpdate();
                break;
        }
        if (_skillConfig.BulletContinueTime != 0)
        {
            if (_flBulletAliveTime <= 0)
            {
                OnBulletEnd();
                return;
            }
            _flBulletAliveTime--;
        }
    }

    private void OnBulletEnd()
    {
        _blBulletEnd = true;
        _container.SetActive(false);
    }

    private bool _blStart = false;
    
    public override void AddToStage(Transform parent)
    {
        base.AddToStage(parent);
        mUnitRoot.position = _bulletType != BulletType.FiexedEffect ? attacker.FireDummyWorldPosition : attacker.mUnitRoot.position;
        if (_modelObject != null && !_blStart)
            OnStart();
    }

	protected override void OnDisposeModel()
	{
        if (_modelObject != null)
        {
            GameObject.Destroy(_modelObject);
            _modelObject = null;
        }
        if (_container != null)
        {
            GameObject.Destroy(_container);
            _container = null;
        }
	}

	protected override void OnDispose()
    {
        if (_damageTicker != null)
        {
            _damageTicker.Dispose();
            _damageTicker = null;
        }
        if (_hitEffectKey != 0)
        {
            TimerHeap.DelTimer(_hitEffectKey);
            _hitEffectKey = 0;
        }
        _skillConfig = null;
        targeter = null;
        attacker = null;
        base.OnDispose();
    }

    public bool BulletFinish
    {
        get
        {
            return _blBulletEnd;
        }
    }
}