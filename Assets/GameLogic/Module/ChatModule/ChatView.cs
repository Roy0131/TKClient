using Framework.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class ChatView : UIBaseView
{
    private Button _btnSend;
    private int _channel;
    private InputField _inputField;
    private ScrollRect _scrollRect;

    private GameObject _heroChatObj;
    private GameObject _otherChatObj;
    private GameObject _heroLevel;
    private GameObject _otherLevel;
    private RectTransform _content;
    private int _maxLimit = 100;
    private List<ChatItemView> _lstChatItems;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _btnSend = Find<Button>("ChatBtn/ButtonSend");
        _inputField = Find<InputField>("ChatBtn/InputField");
        _scrollRect = Find<ScrollRect>("ScrollView");
        _content = Find<RectTransform>("ScrollView/Content");

        _heroChatObj = Find("ChatItemMine");
        _heroLevel = Find("ChatItemMine/uiCardItem/ImageLevelBack");
        _otherChatObj = Find("ChatItemOther");
        _otherLevel = Find("ChatItemOther/uiCardItem/ImageLevelBack");

        _lstChatItems = new List<ChatItemView>();
        _btnSend.onClick.Add(SendText);
        _inputField.onValidateInput = SetInput;
        //_inputField.onValueChanged.AddListener(SetInput);
    }

    public void SetChannel(int channel)
    {
        if (_channel != channel)
            SetTDis(channel);
        _channel = channel;
        ChatModel.Instance.ReqChatMsgPull(_channel);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        SetChannel(int.Parse(args[0].ToString()));
        OnChatDataRefresh(_channel);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        ChatModel.Instance.AddEvent<int>(ChatEvent.ChatRefresh, OnChatDataRefresh);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        ChatModel.Instance.RemoveEvent<int>(ChatEvent.ChatRefresh, OnChatDataRefresh);
    }

    private void OnChatDataRefresh(int channel)
    {
        RedPointEnum redPointID = RedPointHelper.GetRedPointEnum((int)RedPointEnum.Chat * 100 + channel);
        RedPointDataModel.Instance.SetRedPointDataState(redPointID, false);
        RemoveChatItemViews();
        List<ChatItemDataVO> lst = ChatModel.Instance.GetChannelChatDatas(_channel);
        if (lst == null || lst.Count == 0)
            return;
        ChatItemView chatItem;
        int type;
        if (lst.Count > 4)
            _content.pivot = new Vector2(0.5f, 0.0f);
        else
            _content.pivot = new Vector2(0.5f, 1.0f);
        for (int i = 0; i < lst.Count; i++)
        {
            type = lst[i].mPlayerId == HeroDataModel.Instance.mHeroPlayerId ? 1 : 2;
            chatItem = GetChatItemByType(type);
            chatItem.mRectTransform.SetParent(_scrollRect.content, false);
            chatItem.Show(lst[i]);
            //chatItem.mDisplayObject.name = lst[i].mContent;
            _lstChatItems.Add(chatItem);
        }
        _scrollRect.scrollSensitivity = 1;
    }

    private void RemoveChatItemViews()
    {
        if (_lstChatItems != null)
        {
            for (int i = 0; i < _lstChatItems.Count; i++)
                RetChatItemToPool(_lstChatItems[i]);
            _lstChatItems.Clear();
        }
    }

    #region chat item view pool
    private Dictionary<int, Queue<ChatItemView>> _dictChatItemPool = new Dictionary<int, Queue<ChatItemView>>();
    private ChatItemView GetChatItemByType(int type)
    {
        Queue<ChatItemView> queue = null;
        if (_dictChatItemPool.ContainsKey(type))
            queue = _dictChatItemPool[type];
        if (queue != null && queue.Count > 0)
            return queue.Dequeue();
        ChatItemView chatItem = new ChatItemView();
        GameObject obj = type == 1 ? _heroChatObj : _otherChatObj;
        chatItem.SetDisplayObject(GameObject.Instantiate(obj));
        return chatItem;
    }

    private void RetChatItemToPool(ChatItemView item)
    {
        Queue<ChatItemView> queue = null;
        int type = item.mBlHeroChat ? 1 : 2;
        if (!_dictChatItemPool.ContainsKey(type))
        {
            queue = new Queue<ChatItemView>();
            _dictChatItemPool.Add(type, queue);
        }
        else
        {
            queue = _dictChatItemPool[type];
        }
        item.Hide();
        item.mRectTransform.SetParent(_scrollRect.transform, false);
        queue.Enqueue(item);
    }
    #endregion

    public override void Dispose()
    {
        RemoveChatItemViews();
        if (_dictChatItemPool != null)
        {
            Queue<ChatItemView> queue;
            foreach (var kv in _dictChatItemPool)
            {
                queue = kv.Value;
                if (queue == null)
                    continue;
                while (queue.Count > 0)
                    queue.Dequeue().Dispose();
            }
            _dictChatItemPool.Clear();
            _dictChatItemPool = null;
        }
        _lstChatItems = null;
        base.Dispose();
    }

    private void SendText()
    {
        if (string.IsNullOrWhiteSpace(_inputField.text))
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000052));
            return;
        }

        LogHelper.Log(System.Text.Encoding.Unicode.GetBytes(_inputField.text).Length);
        if (System.Text.Encoding.Default.GetBytes(_inputField.text).Length > 200)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000112));
            return;
        }
        _inputField.text.Trim();
        string s = Regex.Replace(_inputField.text, @"\s+", " ").Trim();
        LogHelper.Log(s);
        ChatModel.Instance.ReqChat(_channel, s);
        _inputField.text = "";
    }

    private void SetTDis(int channel)
    {
        switch (channel)
        {
            case ChatChannelConst.World:
            case ChatChannelConst.Guild:
                Find("ChatBtn").SetActive(true);
                Find("ImageBack01").SetActive(true);
                Find("ImageBack02").SetActive(false);
                break;
            case ChatChannelConst.Recruit:
                Find("ChatBtn").SetActive(false);
                Find("ImageBack01").SetActive(false);
                Find("ImageBack02").SetActive(true);
                break;
        }
    }

    private char SetInput(string text, int charIndex, char addedChar)
    {
        if (System.Text.Encoding.UTF8.GetBytes(text + addedChar).Length > _maxLimit)
        {
            return '\0'; 
        }
        return addedChar;
    }
}
