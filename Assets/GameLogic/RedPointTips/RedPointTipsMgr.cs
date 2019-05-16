using System.Xml;
using UnityEngine;
using System.Collections.Generic;
public class RedPointTipsMgr : Singleton<RedPointTipsMgr>
{
    private Dictionary<RedPointEnum, RedPointNode> _dictRedNodes;

    public void Init()
    {
        _dictRedNodes = new Dictionary<RedPointEnum, RedPointNode>();
        CreateRedPointTree();
    }

    private void CreateRedPointTree()
    {
        XmlDocument xmlDoc = GameResMgr.Instance.LoadXml("RedPointTreeConfig");
        if (xmlDoc == null)
        {
            LogHelper.LogError("[RedPointTipsMgr.CreateRedPointTree() => redpoint tree config not found!!!]");
            return;
        }
        XmlNodeList parentNodes = xmlDoc.SelectNodes("root/parent");
        XmlElement pEle;

        foreach (XmlNode pNode in parentNodes)
        {
            if (pNode is XmlComment)
                continue;
            pEle = pNode as XmlElement;
            CreateNodeTreeByXml(pEle);
        }
    }

    private void CreateNodeTreeByXml(XmlElement xmlEle, RedPointNode parent = null)
    {
        int tmpID = int.Parse(xmlEle.GetAttribute("id"));
        RedPointEnum redPointID = RedPointHelper.GetRedPointEnum(tmpID);
        if (_dictRedNodes.ContainsKey(redPointID))
        {
            LogHelper.LogError("[RedPointTipsMgr.CreateNodeTreeByXml() => red point id:" + tmpID + " create repeated!!!]");
            return;
        }
        if (redPointID == RedPointEnum.None)
        {
            LogHelper.LogError("[RedPointTipsMgr.CreateNodeTreeByXml() => redpoint config tree id not registe!!]");
            return;
        }
        RedPointNode node = new RedPointNode(redPointID);
        _dictRedNodes.Add(redPointID, node);
        if (parent != null)
            parent.AddChildren(node);
        bool blChild = int.Parse(xmlEle.GetAttribute("hasChild")) > 0;  
        if (blChild)
        {
            XmlNodeList nodeList = xmlEle.ChildNodes;
            foreach (XmlNode xmlNode in nodeList)
            {
                if (xmlNode is XmlComment)
                    continue;
                CreateNodeTreeByXml(xmlNode as XmlElement, node);
            }
        }
    }

    public void DynamicCreateChildNode(RedPointEnum parentID, int childID, bool blShow = false)
    {
        if (!_dictRedNodes.ContainsKey(parentID))
        {
            LogHelper.LogError("[RedPointTipsMgr.DynamicCreateChild() => parent id:" + parentID + " not found!!!]");
            return;
        }
        _dictRedNodes[parentID].DynamicCreateChildNode(childID, blShow);
    }

    public void UpdateDynamicChildState(RedPointEnum parentID, int childID, bool blShow)
    {
        if (_dictRedNodes.ContainsKey(parentID))
            _dictRedNodes[parentID].DynamicChildNodeUpdate(childID, blShow);
    }

    public void UpdateRedPointState(RedPointEnum redPointID, bool blShow)
    {
        if (_dictRedNodes.ContainsKey(redPointID))
            _dictRedNodes[redPointID].mBlRedShow = blShow;
    }

    public void RedPointBindObject(RedPointEnum redType, GameObject redObject)
    {
        if (_dictRedNodes.ContainsKey(redType))
            _dictRedNodes[redType].BindRedObject(redObject);
    }

    public void ChildNodeBindObject(int child, RedPointEnum parentID, GameObject redObject)
    {
        if (!_dictRedNodes.ContainsKey(parentID))
        {
            LogHelper.LogError("[RedPointTipsMgr.DynamicBindChildNode() => dynamic binding child failed, parent:" + parentID + " not found!!!]");
            return;
        }
        _dictRedNodes[parentID].DynamicBindChildObject(child, redObject);
    }

    public void RedPointUnBindObject(RedPointEnum redPointID, GameObject redObject)
    {
        if (_dictRedNodes.ContainsKey(redPointID))
            _dictRedNodes[redPointID].UnBindRedObject(redObject);
    }

    public void ChildNodeUnBindObject(int child, RedPointEnum parentID, GameObject redObject)
    {
        if (!_dictRedNodes.ContainsKey(parentID))
        {
            LogHelper.LogError("[RedPointTipsMgr.DynamicUnBindChildNode() => dynamic unbinding child failed, parent:" + parentID + " not found!!!]");
            return;
        }
        _dictRedNodes[parentID].DynamicUnBindChildObject(child, redObject);
    }
}