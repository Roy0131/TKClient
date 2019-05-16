using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroGuildLogView : UIBaseView
{
    private Button _btnClose;
    private string _textDescription;
    private string _timeTitle;
    private string _timeFirst;
    private GameObject _objTextItem;
    private Dictionary<string, List<GuildLogsVO>> _mlstGuildLogs;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _btnClose = Find<Button>("CloseBtn");
        _objTextItem = Find("TextItem");
        _btnClose.onClick.Add(OnClose);
        //InitScrollRect("ScrollView");

        ColliderHelper.SetButtonCollider(_btnClose.transform);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        GuildDataModel.Instance.ReqGuildLogs();
    }
    protected override void AddEvent()
    {
        base.AddEvent();
        GuildDataModel.Instance.AddEvent(GuildEvent.GuildLogsRefresh, OnRefreshLogsData);
    }
    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GuildDataModel.Instance.RemoveEvent(GuildEvent.GuildLogsRefresh, OnRefreshLogsData);
    }
    private void OnRefreshLogsData()
    {
        LogHelper.Log("显示log");
        ClearGrid(Find("ScrollView/Content"));
        //ClearGrid(Find("ScrollView/Content"));
        _mlstGuildLogs = new Dictionary<string, List<GuildLogsVO>>();
       _mlstGuildLogs = GuildDataModel.Instance.mdictGuildLogDatas;
        foreach (KeyValuePair<string, List<GuildLogsVO>> ky in _mlstGuildLogs)
        {
            _timeTitle = ky.Key;
            GameObject obj = GameObject.Instantiate(_objTextItem);
            RectTransform objTextTitle = obj.transform.Find("TextTitle").GetComponent<RectTransform>();
            RectTransform objTextDes = obj.transform.Find("TextTitle/TextDes").GetComponent<RectTransform>();
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(objTextTitle.sizeDelta.x, objTextDes.sizeDelta.y * ky.Value.Count+ objTextTitle.sizeDelta.y);
            obj.SetActive(true);
            obj.transform.Find("TextTitle").GetComponent<Text>().text = _timeTitle;
            obj.transform.SetParent(Find("ScrollView/Content").transform, false);

            LogHelper.Log(ky.Value.Count);
            for (int i = 0; i < ky.Value.Count; i++)
            {
                GameObject obj01 =GameObject.Instantiate( obj.transform.Find("TextTitle/TextDes").gameObject);
                obj01.SetActive(true);
                obj01.GetComponent<Text>().text = ky.Value[i].mTimeFirst+"   "+ ky.Value[i].mTextBehavior;
                obj01.transform.SetParent(obj.transform.Find("TextTitle"), false);
            }
        }

        //_loopScrollRect.ClearCells();
        //_loopScrollRect.totalCount = _lstDatas.Count;
        //_loopScrollRect.RefillCells();
    }
    private void ClearGrid(GameObject obj)
    {
        if (obj.transform.childCount != 0)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                GameObject.Destroy(obj.transform.GetChild(i).gameObject);
            }
           
        }
    }
    //protected override UIBaseView CreateItemView()
    //{
    //    GuildDonateItem item = new GuildDonateItem();
    //    item.SetDisplayObject(GameObject.Instantiate(Find("TextItem")));
    //    return item;
    //}
    private void OnClose()
    {
        Hide();
    }
}
