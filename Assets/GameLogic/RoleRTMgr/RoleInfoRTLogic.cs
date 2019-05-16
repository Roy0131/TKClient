using UnityEngine;
using Spine.Unity;
using System;

public class RoleInfoRTLogic : RTLogicBase
{

    private GameObject _roleObject;
    private CardConfig _cardConfig;
    private bool _blShow = false;

    public RoleInfoRTLogic()
        : base(RoleRTType.RoleInfo)
    {
    }

    protected override void OnInit()
    {
        base.OnInit();
        _blShow = false;
    }

    public override void Show<T>(T data, bool blShowHpbar = false)
    {
        base.Show(data, blShowHpbar);

        if (!_blShow)
        {
            _rtRootObject.SetActive(true);
            _blShow = true;
        }
        CardConfig config = data as CardConfig;
        if (_cardConfig == config)
            return;
        if (_roleObject != null)
        {
            RoleResPool.Instance.ReturnRoleObject(_cardConfig.Model, _roleObject);
            _roleObject = null;
        }
        _cardConfig = config;

        Action<GameObject> OnRoleLoaded = (roleObject) =>
        {
            if (_roleObject != null)
                RoleResPool.Instance.ReturnRoleObject(_cardConfig.Model, _roleObject);
            _roleObject = roleObject;

            if (_roleObject == null)
            {
                LogHelper.LogWarning("model name:" + _cardConfig.Model + ", can't found!!!");
                return;
            }
            _roleObject.transform.SetParent(_rtRootObject.transform, false);
            _roleObject.transform.localPosition = Vector3.zero;
            _roleObject.layer = GameLayer.ModeUILayer;
            _roleObject.SetActive(true);

            SkeletonAnimation animator = _roleObject.GetComponent<SkeletonAnimation>();
            animator.state.SetAnimation(0, ActionName.Idle, true);
        };

        RoleResPool.Instance.GetRole(_cardConfig.Model, OnRoleLoaded);
    }

    public override void Hide()
    {
        _blShow = false;
        base.Hide();
    }

    public override void Dispose()
    {
        if(_roleObject != null)
        {
            RoleResPool.Instance.ReturnRoleObject(_cardConfig.Model, _roleObject);
            _roleObject = null;
            _cardConfig = null;
        }
        base.Dispose();
    }
}