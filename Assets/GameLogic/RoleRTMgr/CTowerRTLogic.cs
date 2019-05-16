using Spine.Unity;
using UnityEngine;
using System.Collections.Generic;
using Framework.UI;
using System;

public class CTowerRTLogic : RTLogicBase
{
    private List<GameObject> _lstFighters;
    private HpBarView _hpbar;
    public CTowerRTLogic()
        : base(RoleRTType.CTower)
    {

    }

    protected override void OnInit()
    {
        base.OnInit();

        _rtRootObject.transform.localScale = new Vector3(-1f, 1f, 1f);
        _lstFighters = new List<GameObject>();
    }


    public override void Show<T>(T data, bool blShowHpbar = false)
    {
        base.Show(data, blShowHpbar);
        ClearFighter();
        Dictionary<int, string> dict = data as Dictionary<int, string>;

        for (int i = 0; i < 12; i++)
        {
            if (!dict.ContainsKey(i) || string.IsNullOrEmpty(dict[i]))
                continue;
            CreateRole(i, dict[i]);
        }
    }
    
    private void CreateRole(int idx, string name)
    {
        SkeletonAnimation animator;
        Action<GameObject> OnLoad = (roleObject) =>
        {
            if (roleObject == null)
            {
                LogHelper.LogWarning("[CTowerRTLogic.Show() => monster model:" + name + " not found!!!]");
                return;
            }

            ObjectHelper.AddChildToParent(roleObject.transform, _rtRootObject.transform, false);
            LogHelper.Log("name:" + name);

            roleObject.transform.localPosition = new Vector3(-5.5f, idx * 2.9f, 0f);
            animator = roleObject.GetComponent<SkeletonAnimation>();
            animator.AnimationState.SetAnimation(0, ActionName.Idle, true);
            _lstFighters.Add(roleObject);
        };

        RoleResPool.Instance.GetRole(name, OnLoad);
    }

    private void ClearFighter()
    {
        if (_lstFighters == null)
            return;
        string name;
        for (int i = 0; i < _lstFighters.Count; i++)
        {
            name = _lstFighters[i].name.Replace("(Clone)", "");
            RoleResPool.Instance.ReturnRoleObject(name, _lstFighters[i]);
        }
        _lstFighters.Clear();
    }
    
    public override void Hide()
    {
        base.Hide();
        ClearFighter();
    }

    public override void Dispose()
    {
        base.Dispose();
        if (_lstFighters != null)
        {
            ClearFighter();
            _lstFighters = null;
        }
    }
}