using UnityEngine;
using Spine.Unity;
using System.Collections.Generic;
using Framework.UI;
using System;

public class HeroCallRTLogic : RTLogicBase
{

    private GameObject _roleObject;
    private CardConfig _cardConfig;
    private List<Transform> _lstFighterParents;
    private List<GameObject> _lstFighters;
    private bool _blShow = false;
    private Dictionary<int, CardConfig> _dictHero;

    public HeroCallRTLogic()
        : base(RoleRTType.HeroCall)
    {
    }

    protected override void OnInit()
    {
        base.OnInit();

        _blShow = false;
        _lstFighterParents = new List<Transform>();
        Transform ct;
        GameObject cb;
        for (int i = 1; i <= 2; i++)
        {
            ct = _rtRootObject.transform.Find("p" + i);
            if (ct == null)
            {
                cb = new GameObject("p" + i);
                cb.layer = GameLayer.ModeUILayer;
                ct = cb.transform;
                ct.SetParent(_rtRootObject.transform, false);
                ct.localPosition = new Vector3((i - 1) * 3.2f, 0f, 0f);
                LogHelper.LogWarning("[HeroCallRTLogic.OnInit() => i:" + i + " child not found!!!]");
            }
            _lstFighterParents.Add(ct);
        }
        _lstFighters = new List<GameObject>();
    }

    public override void Show<T>(T data, bool blShowHpbar = false)
    {
        base.Show(data, blShowHpbar);

        RemoveFighters();
        Dictionary<int, string> monster = data as Dictionary<int, string>;


        for (int i = 1; i <= 9; i++)
        {
            if (!monster.ContainsKey(i))
                continue;
            CreateRole(i, monster[i]);
        }

    }
    private void CreateRole(int idx, string name)
    {
        SkeletonAnimation animator;
        Action<GameObject> OnLoaded = (roleObject) =>
        {
            if (roleObject == null)
            {
                LogHelper.LogWarning("[BeforeRTLogic.Show() => monster model:" + name + " not found!!!]");
                return;
            }
            LogHelper.Log(idx.ToString());
            if (idx > _lstFighterParents.Count)
            {
                LogHelper.Log("[BeforeRTLogic.Show() => index:" + idx + " was invalid!!!]");
                string tmpName = roleObject.name.Replace("(Clone)", "");
                RoleResPool.Instance.ReturnRoleObject(tmpName, roleObject);
                return;
            }
            if (roleObject.transform == null)
            {
                LogHelper.Log("idx:" + idx + ",  name:" + name);
                return;
            }
            ObjectHelper.AddChildToParent(roleObject.transform, _lstFighterParents[idx - 1], false);
            animator = roleObject.GetComponent<SkeletonAnimation>();
            animator.AnimationState.SetAnimation(0, ActionName.Idle, true);
            _lstFighters.Add(roleObject);
        };


        RoleResPool.Instance.GetRole(name, OnLoaded);
    }
    public override void Hide()
    {
        base.Hide();
        RemoveFighters();
    }

    private void RemoveFighters()
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

    public override void Dispose()
    {
        base.Dispose();
        if (_lstFighters != null)
        {
            RemoveFighters();
            _lstFighters = null;
        }
        if (_lstFighterParents != null)
        {
            _lstFighterParents.Clear();
            _lstFighterParents = null;
        }
    }
}