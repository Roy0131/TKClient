using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExploreGroupView : UIBaseView
{
    #region

    private ExploreDataVO _exploreDataVo;
    private Text _textTime;
    private List<CardDataVO> _lstDatas; //账号拥有的所有的卡牌
    private List<CardDataVO> _lstAllCard; //满足条件数的卡牌排序
    private GameObject _imageHero;
    private GameObject _imageHeroGrid;
    private GameObject _imageConditionBack;

    private GameObject _imageConditionGrid;

    private GameObject _imageRewardGrid;

    private ExploreHeroBagView _exploreHeroBagView;

    private Button _btnExploreAKey;
    private Button _btnExploreStart;

    private Transform _trans;
    private bool _isStory = true;

    private List<CardDataVO> _cardDataSelRole;

    private List<string> _lstNeed;
    //private List<string> _tempNeed;

    private bool _isOneKey;



    private Dictionary<CardDataVO, int> _dictNum;

    private bool _camsSel;
    private bool _typsSel;
    private bool _starSel;

    private bool _count = false;
    private int _countNum = 0;

    private int _needRoleNum;

    private List<CardView> _lstCardView;

    private bool _canExplore = false;

    private List<string> _tmpNeed;

    #endregion

    #region

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _textTime = Find<Text>("ImageStart/Text");

        _imageHero = Find("ImageHeroBack");
        _imageHeroGrid = Find("GridHero");

        _imageConditionBack = Find("ImageConditionBack");
        _imageConditionGrid = Find("GridCondition");

        _imageRewardGrid = Find("GridReward");

        _btnExploreAKey = Find<Button>("ButtonAKey");
        _btnExploreStart = Find<Button>("ButtonStart");

        _exploreHeroBagView = new ExploreHeroBagView();
        _exploreHeroBagView.SetDisplayObject(Find("HeroBag/uiExploreHerobag"));

        _btnExploreAKey.onClick.Add(ExploreOneKey);
        _btnExploreStart.onClick.Add(ExploreStart);
    }

    #endregion

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _exploreDataVo = args[0] as ExploreDataVO;
        _isStory = (bool)args[1];
        _cardDataSelRole = new List<CardDataVO>();
        _dictNum = new Dictionary<CardDataVO, int>();
        _lstCardView = new List<CardView>();
        _tmpNeed = new List<string>();
        //_tempNeed = new List<string>();
        InitData();
        SetCardConditionlst();//按照满足条数排好顺序

    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(ExploreEvent.ExploreCloseBag, CloseHeroBag);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<ExploreEventVO>(ExploreEvent.ExploreHeroCard, OnClick);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(ExploreEvent.ExploreCloseBag, CloseHeroBag);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<ExploreEventVO>(ExploreEvent.ExploreHeroCard, OnClick);
    }

    private void InitData()
    {
        _camsSel = false;
        _typsSel = false;
        _starSel = false;

        _isOneKey = false;
        _textTime.text = TimeHelper.GetCountTime(_exploreDataVo.mRemainSeconds);
        //创建需要的角色的背景
        SearchTaskConfig searchCfg = GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId);
        if (_imageHeroGrid.transform.childCount != 0)
            for (int i = 0; i < _imageHeroGrid.transform.childCount; i++)
                Object.Destroy(_imageHeroGrid.transform.GetChild(i).gameObject);
        for (int i = 0; i < searchCfg.CardNum; i++)
        {
            GameObject obj = Object.Instantiate(_imageHero);
            obj.transform.SetParent(_imageHeroGrid.transform, false);
            obj.SetActive(true);
            int j = i;
            obj.transform.Find("Collider").GetComponent<Button>().onClick.Add(() => { OpenHeroBag(j); });
        }

        //创建显示的条件
        if (_imageConditionGrid.transform.childCount != 0)
            for (int i = 0; i < _imageConditionGrid.transform.childCount; i++)
                Object.Destroy(_imageConditionGrid.transform.GetChild(i).gameObject);
        _lstNeed = new List<string>();
        if (_exploreDataVo.mRoleCampsCanSel != null)
            for (int i = 0; i < _exploreDataVo.mRoleCampsCanSel.Count; i++)
            {
                GameObject obj = Object.Instantiate(_imageConditionBack);
                obj.name = "camp" + _exploreDataVo.mRoleCampsCanSel[i];
                obj.transform.Find("ImageCondition").gameObject.GetComponent<Image>().sprite =
                    GameResMgr.Instance.LoadCampIcon( _exploreDataVo.mRoleCampsCanSel[i]);
                obj.transform.SetParent(_imageConditionGrid.transform, false);
                obj.gameObject.name = "camp" + _exploreDataVo.mRoleCampsCanSel[i];
                obj.SetActive(true);
                _lstNeed.Add("camp" + _exploreDataVo.mRoleCampsCanSel[i]);
            }

        if (_exploreDataVo.mRoleTypesCanSel != null)
            for (int i = 0; i < _exploreDataVo.mRoleTypesCanSel.Count; i++)
            {
                GameObject obj = Object.Instantiate(_imageConditionBack);
                obj.name = "cardtype" + _exploreDataVo.mRoleTypesCanSel[i];
                obj.transform.Find("ImageCondition").gameObject.GetComponent<Image>().sprite =
                    GameResMgr.Instance.LoadTypeIcon(_exploreDataVo.mRoleTypesCanSel[i]);
                obj.transform.SetParent(_imageConditionGrid.transform, false);
                obj.gameObject.name = "cardtype" + _exploreDataVo.mRoleTypesCanSel[i];
                obj.SetActive(true);
                _lstNeed.Add("cardtype" + _exploreDataVo.mRoleTypesCanSel[i]);
            }

        if (GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardStarCond != 0)
        {
            GameObject obj = Object.Instantiate(_imageConditionBack);
            obj.name = GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardStarCond.ToString();
            obj.transform.Find("ImageCondition").gameObject.GetComponent<Image>().sprite =
                GameResMgr.Instance.LoadItemIcon("levelicon/button_star_0" + GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardStarCond);
            obj.transform.SetParent(_imageConditionGrid.transform, false);
            obj.gameObject.name = "button_star_0" + GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardStarCond;
            obj.SetActive(true);
            _lstNeed.Add("button_star_0" + GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardStarCond.ToString());
        }

        //奖励显示
        DiposeChildren();
        if (_exploreDataVo.mRandomRewards != null)
            for (int i = 0; i < _exploreDataVo.mRandomRewards.Count / 2; i++)
            {
                ItemInfo item = new ItemInfo
                {
                    Id = _exploreDataVo.mRandomRewards[i * 2],
                    Value = _exploreDataVo.mRandomRewards[i * 2 + 1]
                };
                ItemView itemView = ItemFactory.Instance.CreateItemView(item, ItemViewType.RewardItem);
                itemView.mRectTransform.SetParent(_imageRewardGrid.transform, false);
                AddChildren(itemView);
            }

        //_countNum = _lstNeed.Count;//需要满足的条件的数量
        //_tempNeed.AddRange(_lstNeed);
        _tmpNeed.AddRange(_lstNeed);
        _needRoleNum = searchCfg.CardNum;//需要的角色的数量
        //英雄角色排序
        HeroSort();
        //按钮颜色切换显示
        _btnExploreStart.interactable = false;
    }

    private void HeroSort()
    {
        _lstDatas = new List<CardDataVO>();
        for (int i = 0; i < HeroDataModel.Instance.mAllCards.Count; i++) _lstDatas.Add(HeroDataModel.Instance.mAllCards[i]);
        //排序
        _lstDatas.Sort((x, y) =>
        {
            if (GameConfigMgr.Instance.GetCardConfig(x.mCardCfgId).Rarity.CompareTo(GameConfigMgr.Instance.GetCardConfig(y.mCardCfgId).Rarity) != 0)
                return -GameConfigMgr.Instance.GetCardConfig(x.mCardCfgId).Rarity.CompareTo(GameConfigMgr.Instance.GetCardConfig(y.mCardCfgId).Rarity);
            if (x.mCardLevel.CompareTo(y.mCardLevel) != 0)
                return -x.mCardLevel.CompareTo(y.mCardLevel);
            if (x.mCardRank.CompareTo(y.mCardRank) != 0)
                return -x.mCardRank.CompareTo(y.mCardRank);
            if (x.mCardCfgId.CompareTo(y.mCardCfgId) != 0)
                return -x.mCardCfgId.CompareTo(y.mCardCfgId);
            return 1;
        });
    }

    /// <summary>
    ///     点击按钮显示或者关闭英雄背包
    /// </summary>
    private void OpenHeroBag(int id)
    {
        _exploreHeroBagView.Show(_lstDatas, _exploreDataVo, id, _dictNum);
    }

    private void CloseHeroBag()
    {
        _exploreHeroBagView.Hide();
    }

    private void OnClick(ExploreEventVO vo)
    {
        ClickData(vo.mCardDataVO, vo.mHeroCardID);
    }

    private void ClickData(CardDataVO vo, int id)
    {
        _needRoleNum--;
        CardView cardView = CardViewFactory.Instance.CreateCardView(vo, CardViewType.Common, ClickRemove);
        cardView.mRectTransform.SetParent(_imageHeroGrid.transform.GetChild(id), false);
        _lstCardView.Add(cardView);
        _dictNum.Add(vo, id);
        if (!_cardDataSelRole.Contains(vo)) _cardDataSelRole.Add(vo);
        //满足条件打对勾
        GetHeroData(vo);


        if (_needRoleNum == 0)
        {
            if (_tmpNeed.Count == 0)
            {
                _btnExploreStart.interactable = true;
            }
        }

        //for (int i = 0; i < _tmpNeed.Count; i++)
        //{
        //    Debuger.Log(_tmpNeed[i]);
        //}
    }

    private void ClickRemove(CardView item)
    {
        item.Dispose();
        _needRoleNum++;

        _lstCardView.Remove(item);
        _cardDataSelRole.Remove(item.mCardDataVO);
        _isOneKey = false;
        _dictNum.Remove(item.mCardDataVO);
        _btnExploreStart.interactable = false;

        OnGetRemoveData(_cardDataSelRole);
    }

    /// <summary>
    ///     判断是否符合上阵条件
    /// </summary>
    /// <returns></returns>
    private void GetHeroData(CardDataVO vo)
    {
        //遍历条件去判断英雄是否符合阵营要求
        if (_exploreDataVo.mRoleCampsCanSel != null && !_camsSel)
        {
            //int valueCamps = _exploreDataVo.mRoleCampsCanSel.Count;
            for (int j = 0; j < _exploreDataVo.mRoleCampsCanSel.Count; j++)
            {
                if (vo.mCardConfig.Camp == _exploreDataVo.mRoleCampsCanSel[j])
                {
                    //_countNum--;
                    //_countNum--;
                    //valueCamps--;
                    //if (valueCamps == 0) _camsSel = true;
                    if (_tmpNeed.Contains("camp" + vo.mCardConfig.Camp)) _tmpNeed.Remove("camp" + vo.mCardConfig.Camp);
                    _imageConditionGrid.transform.Find("camp" + vo.mCardConfig.Camp).Find("Image").gameObject.SetActive(true);
                }
                else
                {
                    //Debuger.Log("不满足阵营要求");
                    //_camsSel = false;
                }
            }

        }

        //遍历条件去判断英雄是否符合类型要求
        if (_exploreDataVo.mRoleTypesCanSel != null && !_typsSel)
        {
            //int valueTyps = _exploreDataVo.mRoleTypesCanSel.Count;
            for (int j = 0; j < _exploreDataVo.mRoleTypesCanSel.Count; j++)
            {
                if (vo.mCardConfig.Type == _exploreDataVo.mRoleTypesCanSel[j])
                {
                    //_countNum--;
                    //valueTyps--;
                    //if (valueTyps == 0) _typsSel = true;
                    if (_tmpNeed.Contains("cardtype" + vo.mCardConfig.Type)) _tmpNeed.Remove("cardtype" + vo.mCardConfig.Type);
                    _imageConditionGrid.transform.Find("cardtype" + vo.mCardConfig.Type).Find("Image").gameObject.SetActive(true);
                }
                else
                {
                    //Debuger.Log("不满足阵营要求");
                    //_typsSel = false;

                }
            }
        }
        //遍历条件去判断英雄是否符合星级要求
        if (GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardStarCond != 0 && !_starSel)
        {
            if (vo.mCardConfig.Rarity >=
                GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardStarCond)
            {
                //_countNum--;
                //break;
                //_starSel = true;
                if (_tmpNeed.Contains("button_star_0" + GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardStarCond))
                    _tmpNeed.Remove("button_star_0" + GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardStarCond);
                _imageConditionGrid.transform.Find("button_star_0" + GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardStarCond).Find("Image").gameObject.SetActive(true);
            }
            else
            {
                //Debuger.Log("不满足星级需求");
                //_starSel = false;
            }
        }

    }

    /// <summary>
    ///点击下阵
    /// </summary>
    /// <param name="lstVo"></param>
    private void OnGetRemoveData(List<CardDataVO> lstVo)
    {
        #region
        /*
        if (lstVo.Count == 0)
        {
            //_camsSel = false;
            //_typsSel = false;
            //_starSel = false;

            //if (_imageConditionGrid.transform.childCount != 0)
            //{
            //    for (int i = 0; i < _imageConditionGrid.transform.childCount; i++)
            //    {
            //        if (_imageConditionGrid.transform.GetChild(i).Find("Image").gameObject.activeSelf)
            //        {
            //            //_countNum++;
            //            _imageConditionGrid.transform.GetChild(i).Find("Image").gameObject.SetActive(false);
            //        }
            //    }
            //}

        }
        else
        {

            //for (int i = 0; i < lstVo.Count; i++)
            //{
            //遍历条件去判断英雄是否符合阵营要求
            if (_exploreDataVo.mRoleCampsCanSel != null)
            {
                int campsValue = 1;
                int index = 0;
                for (int i = 0; i < lstVo.Count; i++)
                {
                    for (int j = 0; j < _exploreDataVo.mRoleCampsCanSel.Count; j++)
                    {
                        //有一个满足阵营就ok
                        if (lstVo[i].mCardConfig.Camp == _exploreDataVo.mRoleCampsCanSel[j])
                        {
                            campsValue--;
                            index = j;
                            break;
                        }

                    }
                    //_countNum++;
                    GameObject campsObj = _imageConditionGrid.transform.Find("camp" + _exploreDataVo.mRoleCampsCanSel[index]).Find("Image").gameObject;
                    if (campsValue == 1)
                    {
                        if (campsObj.activeSelf)
                        {
                            campsObj.SetActive(false);
                            _camsSel = false;
                        }
                    }
                    else
                    {
                        campsObj.SetActive(true);
                        _camsSel = true;
                    }
                }


            }

            //遍历条件去判断英雄是否符合类型要求
            if (_exploreDataVo.mRoleTypesCanSel != null)
            {
                int typsValue = 1;
                int index = 0;
                for (int i = 0; i < lstVo.Count; i++)
                {
                    for (int j = 0; j < _exploreDataVo.mRoleTypesCanSel.Count; j++)
                    {
                        //有一个满足类型就ok
                        if (lstVo[i].mCardConfig.Type == _exploreDataVo.mRoleTypesCanSel[j])
                        {
                            typsValue--;
                            if (typsValue == 0) index = j;
                            break;
                        }
                    }
                    //_countNum++;
                    GameObject typesObj = _imageConditionGrid.transform.Find("cardtype" + _exploreDataVo.mRoleTypesCanSel[index]).Find("Image").gameObject;
                    if (typsValue == 1)
                    {
                        if (typesObj.activeSelf)
                        {
                            typesObj.SetActive(false);
                            _typsSel = false;
                        }

                    }
                    else
                    {
                        typesObj.SetActive(true);
                        _typsSel = true;
                    }
                }
            }


            //遍历条件去判断英雄是否符合星级要求
            if (GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardStarCond != 0)
            {
                int typsValue = 1;
                for (int i = 0; i < lstVo.Count; i++)
                {
                    //有一个满足星级就ok
                    if (lstVo[i].mCardConfig.Rarity >=
                        GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardStarCond)
                    {
                        typsValue--;
                        break;
                    }
                    else
                    {
                        //_countNum++;

                        GameObject starObj = _imageConditionGrid.transform.Find("button_star_0" + GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardStarCond).Find("Image").gameObject;
                        if (typsValue == 1)
                        {

                            if (starObj.activeSelf)
                            {
                              
                                starObj.SetActive(false);
                                _starSel = false;
                            }

                        }
                        else
                        {
                            starObj.SetActive(true);
                            _starSel = true;
                        }
                    }
                }
            }
            //}
        }
        */
        #endregion
        List<string> tempLst = new List<string>();
        tempLst.AddRange(_lstNeed);

        if (lstVo.Count != 0)
        {
            for (int i = 0; i < tempLst.Count; i++)
            {
                for (int j = 0; j < lstVo.Count; j++)
                {
                    for (int k = 0; k < lstVo[j].mConditionLst.Count; k++)
                    {
                        tempLst.Remove(lstVo[j].mConditionLst[k]);
                    }
                }
            }

            for (int i = 0; i < tempLst.Count; i++)
            {
                _imageConditionGrid.transform.Find(tempLst[i]).Find("Image").gameObject.SetActive(false);
                if (!_tmpNeed.Contains(tempLst[i])) _tmpNeed.Add(tempLst[i]);
            }
        }
        else
        {
            for (int i = 0; i < tempLst.Count; i++)
            {
                _imageConditionGrid.transform.Find(tempLst[i]).Find("Image").gameObject.SetActive(false);
                if (!_tmpNeed.Contains(tempLst[i])) _tmpNeed.Add(tempLst[i]);
            }
        }
    }
    /// <summary>
    ///     开始探索
    /// </summary>
    private void ExploreStart()
    {
        if (_btnExploreStart.interactable)
        {
            List<int> roleIds = new List<int>();
            for (int i = 0; i < _cardDataSelRole.Count; i++)
                roleIds.Add(_cardDataSelRole[i].mCardID);
            GameNetMgr.Instance.mGameServer.ReqExploresSelRole(_exploreDataVo.mId, _isStory, roleIds);
            GameNetMgr.Instance.mGameServer.ReqExploreStart(_exploreDataVo.mId, _isStory);
        }
    }

    /// <summary>
    ///     一键上阵
    /// </summary>
    private void ExploreOneKey()
    {
        if (!_isOneKey)
        {
            //去除之前上阵的角色
            if (_lstCardView.Count != 0)
            {
                for (int i = 0; i < _lstCardView.Count; i++)
                {
                    ClickRemove(_lstCardView[0]);
                    i--;
                }
            }

            _needRoleNum = GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardNum;

            _isOneKey = true;
            int index = 0;
            if (_lstAllCard.Count == 0)
            {
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000106));
                return;
            }
            List<string> _tempNeed = new List<string>();
            _tempNeed.AddRange(_lstNeed);

            if (_lstAllCard[0].mConditionLst.Count != 0)
            {
                ClickData(_lstAllCard[0], index);
                index++;
            }
            else
            {
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000106));
                return;
            }

            for (int i = 0; i < _lstAllCard[0].mConditionLst.Count; i++)
            {
                _tempNeed.Remove(_lstAllCard[0].mConditionLst[i]);
            }
            #region
            /*
            if (_tmpNeed.Count > 0&&_needRoleNum>0)
            {
                for (int i = 1; i < _lstAllCard.Count; i++)
                {
                    for (int j = 0; j < _lstAllCard[i].mConditionLst.Count; j++)
                    {
                        if (_tempNeed.Contains(_lstAllCard[i].mConditionLst[j]))
                        {
                            if (_needRoleNum > 0)
                            {
                                _tempNeed.Remove(_lstAllCard[i].mConditionLst[j]);
                                ClickData(_lstAllCard[i], index);
                                index++;
                                if (_needRoleNum == 0)
                                {
                                    if (_tmpNeed.Count == 0)
                                    {
                                        _btnExploreStart.interactable = true;
                                        return;
                                    }
                                    else
                                    {
                                        _btnExploreStart.interactable = false;
                                    }
                                }
                            }
                            else
                            {
                                if (_tmpNeed.Count == 0)
                                {
                                    _btnExploreStart.interactable = true;
                                    return;
                                }
                                else
                                {
                                    _btnExploreStart.interactable = false;
                                    return;
                                }

                            }
                        }
                        //else
                        //{
                        //    for (int k = 0; k < _needRoleNum; k++)
                        //    {
                        //        ClickData(_lstAllCard[_lstAllCard.Count - index], index);
                        //        index++;
                        //        k--;

                        //        if (_needRoleNum == 0)
                        //        {
                        //            _btnExploreStart.interactable = false;
                        //            return;
                        //        }
                        //    }
                        //}
                    }
                }
            }
            else
            {
                if (_needRoleNum > 0)
                {
                    for (int k = 0; k < _needRoleNum; k++)
                    {
                        ClickData(_lstAllCard[_lstAllCard.Count - index], index);
                        index++;
                        k--;

                        if (_needRoleNum == 0)
                        {
                            _btnExploreStart.interactable = true;
                            return;
                        }
                    }
                }
            }
             */
            #endregion
            //_tempNeed 条件 _needRoleNum 需要的角色
            if (_tempNeed.Count > 0)
            {
                for (int i = 1; i < _lstAllCard.Count; i++)
                {
                    for (int j = 0; j < _lstAllCard[i].mConditionLst.Count; j++)
                    {
                        //需要更多角色
                        if (_needRoleNum > 0)
                        {
                            //需要满足更多的条件需要更多的角色
                            if (_tempNeed.Count > 0)
                            {
                                //有角色满足条件
                                if (_tempNeed.Contains(_lstAllCard[i].mConditionLst[j]))
                                {
                                    _tempNeed.Remove(_lstAllCard[i].mConditionLst[j]);
                                    //如果同一个角色满足多个条件不上阵，第一次满足的时候已经放上去
                                    if (!_cardDataSelRole.Contains(_lstAllCard[i]))
                                    {
                                        ClickData(_lstAllCard[i], index);
                                        index++;
                                    } 
                                  
                                }
                              
                            }
                            //已经满足所有条件
                            else
                            {
                                //条件满足需要更多的角色
                                if (_needRoleNum > 0)
                                {
                                    for (int k = 0; k < _needRoleNum; k++)
                                    {
                                        //上阵英雄数量等于拥有英雄数量，提示英雄不足，并return
                                        if (_lstAllCard.Count == _cardDataSelRole.Count)
                                        {
                                            PopupTipsMgr.Instance.ShowTips(4000106);
                                            return;
                                        }
                                        ClickData(_lstAllCard[_lstAllCard.Count - index], index);
                                        index++;
                                        k--;

                                        if (_needRoleNum == 0)
                                        {
                                            _btnExploreStart.interactable = true;
                                            return;
                                        }
                                    }
                                }
                                //条件满足角色数量已经满足
                                else
                                {
                                    _btnExploreStart.interactable = false;
                                    return;
                                }
                            }

                        }
                        //角色数量已经满足
                        else
                        {
                            //需要更多的条件但是角色数量已经满足
                            if (_tempNeed.Count > 0)
                            {
                                _btnExploreStart.interactable = false;
                                return;
                            }
                            //条件满足角色数量也已经满足
                            else
                            {
                                _btnExploreStart.interactable = true;
                                return;
                            }

                        }
                    }
                }
            }
            else
            {
                if (_needRoleNum > 0)
                {
                    for (int i = 0; i < _needRoleNum; i++)
                    {
                        //上阵英雄数量等于拥有英雄数量，提示英雄不足，并return
                        if (_lstAllCard.Count == _cardDataSelRole.Count)
                        {
                            PopupTipsMgr.Instance.ShowTips(4000106);
                            return;
                        }
                        ClickData(_lstAllCard[_lstAllCard.Count - index], index);
                        index++;
                        i--;

                        if (_needRoleNum == 0)
                        {
                            _btnExploreStart.interactable = true;
                            return;
                        }
                    }
                }
                else
                {
                    _btnExploreStart.interactable = true;
                    return;
                }
            }
        }

    }

    /// <summary>
    ///     挑选符合阵营的英雄
    /// </summary>
    /// <param name="vo"></param>
    /// <returns></returns>
    private CardDataVO SetCampsData(CardDataVO vo)
    {
        if (_exploreDataVo.mRoleCampsCanSel != null)
            for (int i = 0; i < _exploreDataVo.mRoleCampsCanSel.Count; i++)

                if (vo.mCardConfig.Camp == _exploreDataVo.mRoleCampsCanSel[i] && !_cardDataSelRole.Contains(vo))
                {
                    //_cardDataSelRole.Add(vo);
                    //needNumCfg--;
                    return vo;
                }

        return null;
    }

    /// <summary>
    ///     挑选符合类型的英雄
    /// </summary>
    /// <param name="vo"></param>
    /// <returns></returns>
    private CardDataVO SetCardTypeData(CardDataVO vo)
    {
        if (_exploreDataVo.mRoleTypesCanSel != null)
            for (int i = 0; i < _exploreDataVo.mRoleTypesCanSel.Count; i++)

                if (vo.mCardConfig.Type == _exploreDataVo.mRoleTypesCanSel[i] && !_cardDataSelRole.Contains(vo))
                {
                    //_cardDataSelRole.Add(vo);
                    //needNumCfg--;
                    return vo;
                }

        return null;
    }

    /// <summary>
    ///     挑选符合星级的英雄
    /// </summary>
    /// <param name="vo"></param>
    /// <returns></returns>
    private CardDataVO SetCardStarCondData(CardDataVO vo)
    {
        if (GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardStarCond != 0)

            if (vo.mCardConfig.Rarity >= GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardStarCond && !_cardDataSelRole.Contains(vo))
            {
                //_cardDataSelRole.Add(vo);
                //needNumCfg--;
                return vo;
            }

        return null;
    }

    /// <summary>
    ///     针对相应的需求条件，来给予拥有的英雄设置条件list
    /// </summary>
    private void SetCardConditionlst()
    {
        _lstAllCard = new List<CardDataVO>();
        for (int i = 0; i < _lstDatas.Count; i++)
        {
            _lstDatas[i].mConditionLst = new List<string>();
            if (SetCampsData(_lstDatas[i]) != null)
            {
                SetConditionlst(_lstDatas[i], 1);
            }

            if (SetCardTypeData(_lstDatas[i]) != null)
            {
                SetConditionlst(_lstDatas[i], 2);
            }

            if (SetCardStarCondData(_lstDatas[i]) != null)
            {
                SetConditionlst(_lstDatas[i], 3);
            }

            _lstAllCard.Add(_lstDatas[i]);
        }


        //去除服务器返回的已经选择过的角色
        for (int i = 0; i < _lstAllCard.Count; i++)
        {
            if (ExploreDataModel.Instance.mLstSelRoleID != null)
            {
                for (int k = 0; k < ExploreDataModel.Instance.mLstSelRoleID.Count; k++)
                {
                    if (_lstAllCard[i].mCardID == ExploreDataModel.Instance.mLstSelRoleID[k])
                    {
                        _lstAllCard.Remove(_lstAllCard[i]);
                        i--;
                        break;
                    }
                }
            }
        }

        if (_lstAllCard.Count == 0)
        {
            //PopupTipsMgr.Instance.ShowTips("There is not enough hero");
            return;
        }
        //排序
        _lstAllCard.Sort((x, y) =>
        {
            if (x.mConditionLst.Count.CompareTo(y.mConditionLst.Count) != 0)
                return -x.mConditionLst.Count.CompareTo(y.mConditionLst.Count);
            if (GameConfigMgr.Instance.GetCardConfig(x.mCardCfgId).Rarity.CompareTo(GameConfigMgr.Instance.GetCardConfig(y.mCardCfgId).Rarity) != 0)
                return -GameConfigMgr.Instance.GetCardConfig(x.mCardCfgId).Rarity.CompareTo(GameConfigMgr.Instance.GetCardConfig(y.mCardCfgId).Rarity);
            if (x.mCardLevel.CompareTo(y.mCardLevel) != 0)
                return x.mCardLevel.CompareTo(y.mCardLevel);
            if (x.mCardRank.CompareTo(y.mCardRank) != 0)
                return x.mCardRank.CompareTo(y.mCardRank);
            if (x.mCardCfgId.CompareTo(y.mCardCfgId) != 0)
                return x.mCardCfgId.CompareTo(y.mCardCfgId);
            return 1;
        });
        //for (int i = 0; i < _lstAllCard[0].mConditionLst.Count; i++)
        //{
        //    LogHelper.Log(_lstAllCard[0].mConditionLst[i] + "---");
        //}

    }

    /// <summary>
    ///     设置条件list方法
    /// </summary>
    /// <param name="vo"></param>
    /// <param name="value"></param>
    private void SetConditionlst(CardDataVO vo, int value)
    {
        switch (value)
        {
            case 1:
                vo.RefreshCondition("camp" + vo.mCardConfig.Camp);
                break;
            case 2:
                vo.RefreshCondition("cardtype" + vo.mCardConfig.Type);
                break;
            case 3:
                vo.RefreshCondition("button_star_0" + GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardStarCond.ToString());
                break;
        }
    }
    public override void Hide()
    {
        base.Hide();
        if (_lstCardView.Count != 0)
        {
            for (int i = 0; i < _lstCardView.Count; i++)
            {
                ClickRemove(_lstCardView[0]);
            }
        }
        if (_exploreHeroBagView != null)
            _exploreHeroBagView.Hide();
    }
}