using UnityEngine;
using System.Collections.Generic;
using System;

public class RoleResPool : Singleton<RoleResPool>
{
    private Dictionary<string, Queue<GameObject>> _dictRolePool;
    private Transform _resPoolRoot;

    public void Init(Transform root)
    {
        _resPoolRoot = root;
        _resPoolRoot.localPosition = new Vector3(0, 10000, 0);
        _dictRolePool = new Dictionary<string, Queue<GameObject>>();
    }

    public void GetRole(string roleName, Action<GameObject> OnLoaded)
    {
        Queue<GameObject> queue;
        GameObject roleObj = null;
        if(_dictRolePool.ContainsKey(roleName))
        {
            queue = _dictRolePool[roleName];
            if (queue.Count > 0)
                roleObj = queue.Dequeue();
        }

        if (roleObj != null)
            OnLoaded(roleObj);
        else
            GameResMgr.Instance.LoadRole(roleName, OnLoaded);
    }

    public void ReturnRoleObject(string roleName, GameObject roleObj)
    {
		Queue<GameObject> queue;
        if(_dictRolePool.ContainsKey(roleName))
        {
            queue = _dictRolePool[roleName];
            if(queue.Count >= 5)
            {
                GameObject.Destroy(roleObj);
                return;
            }
        }
        else
        {
            queue = new Queue<GameObject>();
            _dictRolePool.Add(roleName, queue);
        }
        queue.Enqueue(roleObj);
        roleObj.transform.SetParent(_resPoolRoot, false);
        roleObj.layer = _resPoolRoot.gameObject.layer;
    }
}