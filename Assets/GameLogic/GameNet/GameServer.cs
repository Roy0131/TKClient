using Google.Protobuf;
using Msg.ClientMessage;
using Msg.ClientMessageId;
using System.Collections.Generic;
using UnityEngine;

public class GameServer : ServerBase
{
    protected override void InitNetMsgHandle()
    {
        _blEnable = false;
        base.InitNetMsgHandle();

        #region hero data msg
        RegisterNetMsgType<S2CEnterGameResponse>((int)MSGID.S2CEnterGameResponse, S2CEnterGameResponse.Parser, DoEnterGame);
        RegisterNetMsgType<S2CReconnectResponse>((int)MSGID.S2CReconnectResponse, S2CReconnectResponse.Parser, DoReconnect);
        RegisterNetMsgType<S2CPlayerInfoResponse>((int)MSGID.S2CPlayerInfoResponse, S2CPlayerInfoResponse.Parser, HeroDataModel.DoPlayerInfo);
        RegisterNetMsgType<S2CPlayerChangeNameResponse>((int)MSGID.S2CPlayerChangeNameResponse, S2CPlayerChangeNameResponse.Parser, HeroDataModel.DoPlayerName);
        RegisterNetMsgType<S2CPlayerChangeHeadResponse>((int)MSGID.S2CPlayerChangeHeadResponse, S2CPlayerChangeHeadResponse.Parser, HeroDataModel.DoPlayerHead);
        RegisterNetMsgType<S2CTeamsResponse>((int)MSGID.S2CTeamsResponse, S2CTeamsResponse.Parser, HeroDataModel.DoTeamChange);
        RegisterNetMsgType<S2CEnterGameCompleteNotify>((int)MSGID.S2CEnterGameCompleteNotify, S2CEnterGameCompleteNotify.Parser, HeroDataModel.DoEnterGameComplete);
        RegisterNetMsgType<S2CRolesChangeNotify>((int)MSGID.S2CRolesChangeNotify, S2CRolesChangeNotify.Parser, HeroDataModel.DoHeroRoleChange);
        RegisterNetMsgType<S2CRolesResponse>((int)MSGID.S2CRolesResponse, S2CRolesResponse.Parser, HeroDataModel.DoHeroRoleResponse);
        RegisterNetMsgType<S2CRoleHandbookResponse>((int)MSGID.S2CRoleHandbookResponse, S2CRoleHandbookResponse.Parser, HeroDataModel.DoRoleHandBookResponse);
        RegisterNetMsgType<S2CRoleAttrsResponse>((int)MSGID.S2CRoleAttrsResponse, S2CRoleAttrsResponse.Parser, HeroDataModel.DoRoleAttriResponse);
        RegisterNetMsgType<S2CSetTeamResponse>((int)MSGID.S2CSetTeamResponse, S2CSetTeamResponse.Parser, HeroDataModel.DoSetTeamResponse);
        RegisterNetMsgType<S2CHeartbeat>((int)MSGID.S2CHeartbeat, S2CHeartbeat.Parser, HeroDataModel.UpdateSystemTime);
        #endregion

        #region hero bag msg
        RegisterNetMsgType<S2CItemsUpdate>((int)MSGID.S2CItemsUpdate, S2CItemsUpdate.Parser, BagDataModel.DoUpdateItem);
        RegisterNetMsgType<S2CItemsSync>((int)MSGID.S2CItemsSync, S2CItemsSync.Parser, BagDataModel.DoItemSync);
        RegisterNetMsgType<S2CItemFusionResponse>((int)MSGID.S2CItemFusionResponse, S2CItemFusionResponse.Parser, BagDataModel.DoFusionItem);
        RegisterNetMsgType<S2CItemSellResponse>((int)MSGID.S2CItemSellResponse, S2CItemSellResponse.Parser, BagDataModel.DoSellItem);
        RegisterNetMsgType<S2CItemUpgradeResponse>((int)MSGID.S2CItemUpgradeResponse, S2CItemUpgradeResponse.Parser, BagDataModel.DoItemUpgradeResponse);
        RegisterNetMsgType<S2CRoleLeftSlotResultSaveResponse>((int)MSGID.S2CRoleLeftslotResultSaveResponse, S2CRoleLeftSlotResultSaveResponse.Parser, BagDataModel.DoItemUpgradeResponse);
        RegisterNetMsgType<S2CItemOneKeyUpgradeResponse>((int)MSGID.S2CItemOnekeyUpgradeResponse, S2CItemOneKeyUpgradeResponse.Parser, BagDataModel.DoItemUpgradeByOneKey);
        #endregion

        #region battle msg
        RegisterNetMsgType<S2CBattleResultResponse>((int)MSGID.S2CBattleResultResponse, S2CBattleResultResponse.Parser, BattleDataModel.DoBattleResult);
        RegisterNetMsgType<S2CBattleRandomRewardNotify>((int)MSGID.S2CBattleRandomRewardNotify, S2CBattleRandomRewardNotify.Parser, BattleDataModel.DoBattleRandomReward);
        #endregion

        #region Campaign msg
        RegisterNetMsgType<S2CCampaignDataResponse>((int)MSGID.S2CCampaignDataResponse, S2CCampaignDataResponse.Parser, HangupDataModel.DoCampaignData);
        RegisterNetMsgType<S2CCampaignHangupIncomeResponse>((int)MSGID.S2CCampaignHangupIncomeResponse, S2CCampaignHangupIncomeResponse.Parser, HangupDataModel.DoHangupReward);
        RegisterNetMsgType<S2CBattleSetHangupCampaignResponse>((int)MSGID.S2CBattleSetHangupCampaignResponse, S2CBattleSetHangupCampaignResponse.Parser, HangupDataModel.DoHangupBack);
        RegisterNetMsgType<S2CCampaignAccelerateIncomeResponse>((int)MSGID.S2CCampaignAccelerateIncomeResponse, S2CCampaignAccelerateIncomeResponse.Parser, HangupDataModel.DoAccelerate);
        RegisterNetMsgType<S2CTowerDataResponse>((int)MSGID.S2CTowerDataResponse, S2CTowerDataResponse.Parser, CTowerDataModel.DoCTowerData);
        RegisterNetMsgType<S2CTowerRankingListResponse>((int)MSGID.S2CTowerRankingListResponse, S2CTowerRankingListResponse.Parser, CTowerDataModel.DoTowerRankingData);
        RegisterNetMsgType<S2CTowerRecordsInfoResponse>((int)MSGID.S2CTowerRecordsInfoResponse, S2CTowerRecordsInfoResponse.Parser, CTowerDataModel.DoTowerRecordInfoData);
        RegisterNetMsgType<S2CTowerRecordDataResponse>((int)MSGID.S2CTowerRecordDataResponse, S2CTowerRecordDataResponse.Parser, CTowerDataModel.DoTowerRecordData);
        #endregion

        #region Mail msg
        RegisterNetMsgType<S2CMailListResponse>((int)MSGID.S2CMailListResponse, S2CMailListResponse.Parser, MailDataModel.DoListMail);
        RegisterNetMsgType<S2CMailSendResponse>((int)MSGID.S2CMailSendResponse, S2CMailSendResponse.Parser, MailDataModel.DoSendMail);
        RegisterNetMsgType<S2CMailDetailResponse>((int)MSGID.S2CMailDetailResponse, S2CMailDetailResponse.Parser, MailDataModel.DoDetaMail);
        RegisterNetMsgType<S2CMailGetAttachedItemsResponse>((int)MSGID.S2CMailGetAttachedItemsResponse, S2CMailGetAttachedItemsResponse.Parser, MailDataModel.DoGetAttachedItem);
        RegisterNetMsgType<S2CMailDeleteResponse>((int)MSGID.S2CMailDeleteResponse, S2CMailDeleteResponse.Parser, MailDataModel.DoDelete);
        RegisterNetMsgType<S2CMailsNewNotify>((int)MSGID.S2CMailsNewNotify, S2CMailsNewNotify.Parser, MailDataModel.DoMailNew);
        #endregion

        #region DrawCard msg
        RegisterNetMsgType<S2CDrawCardResponse>((int)MSGID.S2CDrawCardResponse, S2CDrawCardResponse.Parser, RecruitDataModel.DoDrawCard);
        RegisterNetMsgType<S2CDrawDataResponse>((int)MSGID.S2CDrawDataResponse, S2CDrawDataResponse.Parser, RecruitDataModel.DoDrawData);
        RegisterNetMsgType<S2CRoleDisplaceResponse>((int)MSGID.S2CRoleDisplaceResponse, S2CRoleDisplaceResponse.Parser, HeroCallModel.DoHeroReplacement);
        RegisterNetMsgType<S2CRoleDisplaceConfirmResponse>((int)MSGID.S2CRoleDisplaceConfirmResponse, S2CRoleDisplaceConfirmResponse.Parser, HeroCallModel.DoHeroReplacementConfirm);
        #endregion

        #region role levelup rankup decompose fusion response
        RegisterNetMsgType<S2CRoleLevelUpResponse>((int)MSGID.S2CRoleLevelupResponse, S2CRoleLevelUpResponse.Parser, HeroDataModel.DoRoleLevelupResponse);
        RegisterNetMsgType<S2CRoleRankUpResponse>((int)MSGID.S2CRoleRankupResponse, S2CRoleRankUpResponse.Parser, HeroDataModel.DoRoleRankupResponse);
        RegisterNetMsgType<S2CRoleFusionResponse>((int)MSGID.S2CRoleFusionResponse, S2CRoleFusionResponse.Parser, HeroDataModel.DoRoleFusionResponse);
        RegisterNetMsgType<S2CRoleLockResponse>((int)MSGID.S2CRoleLockResponse, S2CRoleLockResponse.Parser, HeroDataModel.DoRoleLockResponse);
        RegisterNetMsgType<S2CRoleDecomposeResponse>((int)MSGID.S2CRoleDecomposeResponse, S2CRoleDecomposeResponse.Parser, DecomposeDataModel.DoRoleDecompose);
        #endregion

        #region equipment response msg
        RegisterNetMsgType<S2CRoleLeftSlotOpenResponse>((int)MSGID.S2CRoleLeftslotOpenResponse, S2CRoleLeftSlotOpenResponse.Parser, HeroDataModel.DoOpenLeftSlotResponse);
        RegisterNetMsgType<S2CRoleOneKeyEquipResponse>((int)MSGID.S2CRoleOnekeyEquipResponse, S2CRoleOneKeyEquipResponse.Parser, HeroDataModel.DoWearEquipOneKeyResponse);
        RegisterNetMsgType<S2CRoleOneKeyUnequipResponse>((int)MSGID.S2CRoleOnekeyUnequipResponse, S2CRoleOneKeyUnequipResponse.Parser, HeroDataModel.DoTakeoffEquipOneKeyResponse);
        RegisterNetMsgType<S2CItemEquipResponse>((int)MSGID.S2CItemEquipResponse, S2CItemEquipResponse.Parser, HeroDataModel.DoWearEquipmentResponse);
        RegisterNetMsgType<S2CItemUnequipResponse>((int)MSGID.S2CItemUnequipResponse, S2CItemUnequipResponse.Parser, HeroDataModel.DoTakeoffEquipmentResponse);
        #endregion

        #region Gold msg
        RegisterNetMsgType<S2CGoldHandDataResponse>((int)MSGID.S2CGoldHandDataResponse, S2CGoldHandDataResponse.Parser, GoldDataModel.DoGoldData);
        RegisterNetMsgType<S2CTouchGoldResponse>((int)MSGID.S2CTouchGoldResponse, S2CTouchGoldResponse.Parser, GoldDataModel.DoGoldTou);
        #endregion

        #region Shop msg
        RegisterNetMsgType<S2CShopDataResponse>((int)MSGID.S2CShopDataResponse, S2CShopDataResponse.Parser, ShopDataModel.DoShopData);
        RegisterNetMsgType<S2CShopBuyItemResponse>((int)MSGID.S2CShopBuyItemResponse, S2CShopBuyItemResponse.Parser, ShopDataModel.DoShopBuy);
        RegisterNetMsgType<S2CShopRefreshResponse>((int)MSGID.S2CShopRefreshResponse, S2CShopRefreshResponse.Parser, ShopDataModel.DoShopResfresh);
        RegisterNetMsgType<S2CShopAutoRefreshNotify>((int)MSGID.S2CShopAutoRefreshNotify, S2CShopAutoRefreshNotify.Parser, ShopDataModel.DoShopAuto);
        #endregion

        #region Arena RankListData
        RegisterNetMsgType<S2CArenaDataResponse>((int)MSGID.S2CArenaDataResponse, S2CArenaDataResponse.Parser, ArenaDataModel.DoArenaData);
        RegisterNetMsgType<S2CArenaMatchPlayerResponse>((int)MSGID.S2CArenaMatchPlayerResponse, S2CArenaMatchPlayerResponse.Parser, ArenaDataModel.DoMatchPlayer);
        RegisterNetMsgType<S2CArenaPlayerDefenseTeamResponse>((int)MSGID.S2CArenaPlayerDefenseTeamResponse, S2CArenaPlayerDefenseTeamResponse.Parser, PlayerInfoDataModel.DoPlayerDefenseTeam);
        RegisterNetMsgType<S2CRankListResponse>((int)MSGID.S2CRankListResponse, S2CRankListResponse.Parser, DoRankListResponse);
        RegisterNetMsgType<S2CBattleRecordListResponse>((int)MSGID.S2CBattleRecordListResponse, S2CBattleRecordListResponse.Parser, ArenaDataModel.DoBattleRecordData);
        RegisterNetMsgType<S2CBattleRecordResponse>((int)MSGID.S2CBattleRecordResponse, S2CBattleRecordResponse.Parser, ArenaDataModel.DoArenaRecordResponse);
        RegisterNetMsgType<S2CArenaScoreNotify>((int)MSGID.S2CArenaScoreNotify, S2CArenaScoreNotify.Parser, BattleDataModel.DoArenaBattleResult);
        #endregion

        #region Task msg
        RegisterNetMsgType<S2CTaskDataResponse>((int)MSGID.S2CTaskDataResponse, S2CTaskDataResponse.Parser, TaskDataModel.DoTaskData);
        RegisterNetMsgType<S2CTaskRewardResponse>((int)MSGID.S2CTaskRewardResponse, S2CTaskRewardResponse.Parser, TaskDataModel.DoTaskReward);
        RegisterNetMsgType<S2CTaskValueNotify>((int)MSGID.S2CTaskValueNotify, S2CTaskValueNotify.Parser, TaskDataModel.DoTaskValue);
        #endregion

        #region ActivityCopy
        RegisterNetMsgType<S2CActiveStageDataResponse>((int)MSGID.S2CActiveStageDataResponse, S2CActiveStageDataResponse.Parser, ActivityCopyDataModel.DoActiveStageData);
        RegisterNetMsgType<S2CActiveStageAssistRoleListResponse>((int)MSGID.S2CActiveStageAssistRoleListResponse, S2CActiveStageAssistRoleListResponse.Parser, ActivityCopyDataModel.DoActiveStageAssistRoleListData);
        RegisterNetMsgType<S2CActiveStageBuyChallengeNumResponse>((int)MSGID.S2CActiveStageBuyChallengeNumResponse, S2CActiveStageBuyChallengeNumResponse.Parser, ActivityCopyDataModel.DoActiveStageBuyChallengeNumData);
        RegisterNetMsgType<S2CActiveStageRefreshNotify>((int)MSGID.S2CActiveStageRefreshNotify, S2CActiveStageRefreshNotify.Parser, ActivityCopyDataModel.DoActiveNotify);
        #endregion

        #region Explore
        RegisterNetMsgType<S2CExploreDataResponse>((int)MSGID.S2CExploreDataResponse, S2CExploreDataResponse.Parser, ExploreDataModel.DoExploreData);// 请求探索数据
        RegisterNetMsgType<S2CExploreSelRoleResponse>((int)MSGID.S2CExploreSelRoleResponse, S2CExploreSelRoleResponse.Parser, ExploreDataModel.DoExploresSelRole);// 选择探索角色
        RegisterNetMsgType<S2CExploreStartResponse>((int)MSGID.S2CExploreStartResponse, S2CExploreStartResponse.Parser, ExploreDataModel.DoExploreStart);// 开始探索
        RegisterNetMsgType<S2CExploreSpeedupResponse>((int)MSGID.S2CExploreSpeedupResponse, S2CExploreSpeedupResponse.Parser, ExploreDataModel.DoExploreSpeedup);// 加速
        RegisterNetMsgType<S2CExploreRefreshResponse>((int)MSGID.S2CExploreRefreshResponse, S2CExploreRefreshResponse.Parser, ExploreDataModel.DoExploreRefresh);// 刷新探索任务
        RegisterNetMsgType<S2CExploreLockResponse>((int)MSGID.S2CExploreLockResponse, S2CExploreLockResponse.Parser, ExploreDataModel.DoExploreLock);// 锁定或解锁探索任务
        RegisterNetMsgType<S2CExploreGetRewardResponse>((int)MSGID.S2CExploreGetRewardResponse, S2CExploreGetRewardResponse.Parser, ExploreDataModel.DoExploreGetReward);// 探索奖励
        RegisterNetMsgType<S2CExploreCancelResponse>((int)MSGID.S2CExploreCancelResponse, S2CExploreCancelResponse.Parser, ExploreDataModel.DoExploreTaskRemove);// 取消探索任务
        RegisterNetMsgType<S2CExploreStoryNewNotify>((int)MSGID.S2CExploreStoryNewNotify, S2CExploreStoryNewNotify.Parser, ExploreDataModel.DoExploreStoryNew);// 剧情探索任务通知
        RegisterNetMsgType<S2CExploreRemoveNotify>((int)MSGID.S2CExploreRemoveNotify, S2CExploreRemoveNotify.Parser, ExploreDataModel.DoExploreRemove);
        #endregion

        #region Friend msg datas
        RegisterNetMsgType<S2CFriendListResponse>((int)MSGID.S2CFriendListResponse, S2CFriendListResponse.Parser, FriendDataModel.DoFriendListResult);
        RegisterNetMsgType<S2CFriendRecommendResponse>((int)MSGID.S2CFriendRecommendResponse, S2CFriendRecommendResponse.Parser, FriendDataModel.DoRecommendFriendResult);
        RegisterNetMsgType<S2CFriendAskPlayerListResponse>((int)MSGID.S2CFriendAskPlayerListResponse, S2CFriendAskPlayerListResponse.Parser, FriendDataModel.DoFriendAskListReulst);
        RegisterNetMsgType<S2CFriendRemoveResponse>((int)MSGID.S2CFriendRemoveResponse, S2CFriendRemoveResponse.Parser, FriendDataModel.DoRemoveFriendResult);
        RegisterNetMsgType<S2CFriendAskPlayerListAddNotify>((int)MSGID.S2CFriendAskPlayerListAddNotify, S2CFriendAskPlayerListAddNotify.Parser, FriendDataModel.DoAskFriendListApplyResult);
        RegisterNetMsgType<S2CFriendAgreeResponse>((int)MSGID.S2CFriendAgreeResponse, S2CFriendAgreeResponse.Parser, FriendDataModel.DoFriendAgreeResult);
        RegisterNetMsgType<S2CFriendListAddNotify>((int)MSGID.S2CFriendListAddNotify, S2CFriendListAddNotify.Parser, FriendDataModel.DoFriendListAddResult);
        RegisterNetMsgType<S2CFriendAskResponse>((int)MSGID.S2CFriendAskResponse, S2CFriendAskResponse.Parser, FriendDataModel.DoFriendAskResult);
        RegisterNetMsgType<S2CFriendRefuseResponse>((int)MSGID.S2CFriendRefuseResponse, S2CFriendRefuseResponse.Parser, FriendDataModel.DoFriendRefuseResult);
        RegisterNetMsgType<S2CFriendGivePointsResponse>((int)MSGID.S2CFriendGivePointsResponse, S2CFriendGivePointsResponse.Parser, FriendDataModel.DoFriendGivePointsResult);
        RegisterNetMsgType<S2CFriendGetPointsResponse>((int)MSGID.S2CFriendGetPointsResponse, S2CFriendGetPointsResponse.Parser, FriendDataModel.DoFriendGetPointsResult);
        RegisterNetMsgType<S2CFriendDataResponse>((int)MSGID.S2CFriendDataResponse, S2CFriendDataResponse.Parser, FriendDataModel.DoFriendDataResult);
        RegisterNetMsgType<S2CFriendSetAssistRoleResponse>((int)MSGID.S2CFriendSetAssistRoleResponse, S2CFriendSetAssistRoleResponse.Parser, FriendDataModel.DoSetAssistRoleResult);
        RegisterNetMsgType<S2CFriendSearchBossResponse>((int)MSGID.S2CFriendSearchBossResponse, S2CFriendSearchBossResponse.Parser, FriendDataModel.DoSearchFriendBossResult);
        RegisterNetMsgType<S2CFriendGetAssistPointsResponse>((int)MSGID.S2CFriendGetAssistPointsResponse, S2CFriendGetAssistPointsResponse.Parser, FriendDataModel.DoFriendAssistPointsResult);
        RegisterNetMsgType<S2CFriendRemoveNotify>((int)MSGID.S2CFriendRemoveNotify, S2CFriendRemoveNotify.Parser, FriendDataModel.DoFriendListRemoveResult);
        #endregion

        #region Guild data msg
        RegisterNetMsgType<S2CGuildDataResponse>((int)MSGID.S2CGuildDataResponse, S2CGuildDataResponse.Parser, GuildDataModel.DoGuildDataResult);
        RegisterNetMsgType<S2CGuildRecommendResponse>((int)MSGID.S2CGuildRecommendResponse, S2CGuildRecommendResponse.Parser, GuildDataModel.DoRecommemdGuildResult);
        RegisterNetMsgType<S2CGuildSearchResponse>((int)MSGID.S2CGuildSearchResponse, S2CGuildSearchResponse.Parser, GuildDataModel.DoSearchGuildResult);
        RegisterNetMsgType<S2CGuildCreateResponse>((int)MSGID.S2CGuildCreateResponse, S2CGuildCreateResponse.Parser, GuildDataModel.DoGuildCreateResult);
        RegisterNetMsgType<S2CGuildAskJoinResponse>((int)MSGID.S2CGuildAskJoinResponse, S2CGuildAskJoinResponse.Parser, GuildDataModel.DoGuildAskJoinResult);
        RegisterNetMsgType<S2CGuildAgreeJoinNotify>((int)MSGID.S2CGuildAgreeJoinNotify, S2CGuildAgreeJoinNotify.Parser, GuildDataModel.DoGuildAgreeJoinResult);
        RegisterNetMsgType<S2CGuildSignInResponse>((int)MSGID.S2CGuildSignInResponse, S2CGuildSignInResponse.Parser, GuildDataModel.DoGuildSignResult);
        RegisterNetMsgType<S2CGuildDonateListResponse>((int)MSGID.S2CGuildDonateListResponse, S2CGuildDonateListResponse.Parser, GuildDataModel.DoDonateListResult);
        RegisterNetMsgType<S2CGuildDonateResponse>((int)MSGID.S2CGuildDonateResponse, S2CGuildDonateResponse.Parser, GuildDataModel.DoGuildDonateResult);
        RegisterNetMsgType<S2CGuildDonateItemNotify>((int)MSGID.S2CGuildDonateItemNotify, S2CGuildDonateItemNotify.Parser, GuildDataModel.DoGuildDonateItemNot);
        RegisterNetMsgType<S2CGuildMemebersResponse>((int)MSGID.S2CGuildMembersResponse, S2CGuildMemebersResponse.Parser, GuildDataModel.DoMembersResult);
        RegisterNetMsgType<S2CGuildQuitResponse>((int)MSGID.S2CGuildQuitResponse, S2CGuildQuitResponse.Parser, GuildDataModel.DoQuitGuildResult);
        RegisterNetMsgType<S2CGuildAskListResponse>((int)MSGID.S2CGuildAskListResponse, S2CGuildAskListResponse.Parser, GuildDataModel.DoAskListResult);
        RegisterNetMsgType<S2CGuildAgreeJoinResponse>((int)MSGID.S2CGuildAgreeJoinResponse, S2CGuildAgreeJoinResponse.Parser, GuildDataModel.DoAgreeJoinResult);
        RegisterNetMsgType<S2CGuildAskDonateResponse>((int)MSGID.S2CGuildAskDonateResponse, S2CGuildAskDonateResponse.Parser, GuildDataModel.DoReqDonateResult);
        RegisterNetMsgType<S2CGuildRecruitResponse>((int)MSGID.S2CGuildRecruitResponse, S2CGuildRecruitResponse.Parser, GuildDataModel.DoRecruitResult);
        RegisterNetMsgType<S2CGuildDismissResponse>((int)MSGID.S2CGuildDismissResponse, S2CGuildDismissResponse.Parser, GuildDataModel.DoDismissGuildResult);
        RegisterNetMsgType<S2CGuildCancelDismissResponse>((int)MSGID.S2CGuildCancelDismissResponse, S2CGuildCancelDismissResponse.Parser, GuildDataModel.DoCancelDismissGuildResult);
        RegisterNetMsgType<S2CGuildInfoModifyResponse>((int)MSGID.S2CGuildInfoModifyResponse, S2CGuildInfoModifyResponse.Parser, GuildDataModel.DoGuildInfoModifyResult);
        RegisterNetMsgType<S2CGuildAnouncementResponse>((int)MSGID.S2CGuildAnouncementResponse, S2CGuildAnouncementResponse.Parser, GuildDataModel.DoGuildNoticeModifyResult);
        RegisterNetMsgType<S2CGuildSetOfficerResponse>((int)MSGID.S2CGuildSetOfficerResponse, S2CGuildSetOfficerResponse.Parser, GuildDataModel.DoSetOfficerResult);
        RegisterNetMsgType<S2CGuildSetOfficerNotify>((int)MSGID.S2CGuildSetOfficerNotify, S2CGuildSetOfficerNotify.Parser, GuildDataModel.DoSetOfficerNotify);
        RegisterNetMsgType<S2CGuildKickMemberResponse>((int)MSGID.S2CGuildKickMemberResponse, S2CGuildKickMemberResponse.Parser, GuildDataModel.DoKickMemberResult);
        RegisterNetMsgType<S2CGuildKickMemberNotify>((int)MSGID.S2CGuildKickMemberNotify, S2CGuildKickMemberNotify.Parser, GuildDataModel.DoKickMemberNotify);
        RegisterNetMsgType<S2CGuildChangePresidentResponse>((int)MSGID.S2CGuildChangePresidentResponse, S2CGuildChangePresidentResponse.Parser, GuildDataModel.DoChangePresidentResult);
        RegisterNetMsgType<S2CGuildChangePresidentNotify>((int)MSGID.S2CGuildChangePresidentNotify, S2CGuildChangePresidentNotify.Parser, GuildDataModel.DoChangePresidentNotify);
        RegisterNetMsgType<S2CGuildLogsResponse>((int)MSGID.S2CGuildLogsResponse, S2CGuildLogsResponse.Parser, GuildDataModel.DoGuildLogsData);
        RegisterNetMsgType<S2CGuildStageDataResponse>((int)MSGID.S2CGuildStageDataResponse, S2CGuildStageDataResponse.Parser, GuildBossDataModel.DoBossDataResult);
        RegisterNetMsgType<S2CGuildStageRankListResponse>((int)MSGID.S2CGuildStageRankListResponse, S2CGuildStageRankListResponse.Parser, GuildBossDataModel.DoBossRankResult);
        RegisterNetMsgType<S2CGuildStagePlayerRespawnResponse>((int)MSGID.S2CGuildStagePlayerRespawnResponse, S2CGuildStagePlayerRespawnResponse.Parser, GuildBossDataModel.DoBossPlayerRespawn);
        RegisterNetMsgType<S2CGuildStageResetResponse>((int)MSGID.S2CGuildStageResetResponse, S2CGuildStageResetResponse.Parser, GuildBossDataModel.DoBossReset);
        RegisterNetMsgType<S2CGuildStageResetNotify>((int)MSGID.S2CGuildStageResetNotify, S2CGuildStageResetNotify.Parser, GuildBossDataModel.DoResetRemain);
        RegisterNetMsgType<S2CGuildStageAutoRefreshNotify>((int)MSGID.S2CGuildStageAutoRefreshNotify, S2CGuildStageAutoRefreshNotify.Parser, GuildBossDataModel.DoRefreshRemain);
        RegisterNetMsgType<S2CTalentListResponse>((int)MSGID.S2CTalentListResponse, S2CTalentListResponse.Parser, TalentDataModel.DoTalentData);
        RegisterNetMsgType<S2CTalentUpResponse>((int)MSGID.S2CTalentUpResponse, S2CTalentUpResponse.Parser, TalentDataModel.DoTalentUp);
        RegisterNetMsgType<S2CTalentResetResponse>((int)MSGID.S2CTalentResetResponse, S2CTalentResetResponse.Parser, TalentDataModel.DoTalentReset);
        RegisterNetMsgType<S2CGuildDeleteNotify>((int)MSGID.S2CGuildDeleteNotify, S2CGuildDeleteNotify.Parser, GuildDataModel.DoDeleteGuildResult);
        #endregion
        #region chat
        RegisterNetMsgType<S2CChatResponse>((int)MSGID.S2CChatResponse, S2CChatResponse.Parser, ChatModel.DoChatData);// 发送世界聊天消息
        RegisterNetMsgType<S2CChatMsgPullResponse>((int)MSGID.S2CChatMsgPullResponse, S2CChatMsgPullResponse.Parser, ChatModel.DoChatMsgPullData);// 拉取聊天消息
        #endregion

        #region Boon
        RegisterNetMsgType<S2CSignDataResponse>((int)MSGID.S2CSignDataResponse, S2CSignDataResponse.Parser, WelfareDataModel.DoSignData);
        RegisterNetMsgType<S2CSignAwardResponse>((int)MSGID.S2CSignAwardResponse, S2CSignAwardResponse.Parser, WelfareDataModel.DoSignAward);
        RegisterNetMsgType<S2CSevenDaysDataResponse>((int)MSGID.S2CSevendaysDataResponse, S2CSevenDaysDataResponse.Parser, WelfareDataModel.DoSevenData);
        RegisterNetMsgType<S2CSevenDaysAwardResponse>((int)MSGID.S2CSevendaysAwardResponse, S2CSevenDaysAwardResponse.Parser, WelfareDataModel.DoSevenAward);
        RegisterNetMsgType<S2CActivityDataResponse>((int)MSGID.S2CActivityDataResponse, S2CActivityDataResponse.Parser, WelfareDataModel.DoActivityData);
        RegisterNetMsgType<S2CActivityDataNotify>((int)MSGID.S2CActivityDataNotify, S2CActivityDataNotify.Parser, WelfareDataModel.DoDataNotify);
        #endregion

        #region Recharge
        RegisterNetMsgType<S2CChargeDataResponse>((int)MSGID.S2CChargeDataResponse, S2CChargeDataResponse.Parser, RechargeDataModel.DoRechargeData);
        RegisterNetMsgType<S2CChargeResponse>((int)MSGID.S2CChargeResponse, S2CChargeResponse.Parser, RechargeDataModel.DoRecharge);
        RegisterNetMsgType<S2CChargeFirstAwardResponse>((int)MSGID.S2CChargeFirstAwardResponse, S2CChargeFirstAwardResponse.Parser, RechargeDataModel.DoFirstReward);
        RegisterNetMsgType<S2CChargeFirstRewardNotify>((int)MSGID.S2CChargeFirstRewardNotify, S2CChargeFirstRewardNotify.Parser, RechargeDataModel.DoFirstRewardNot);
       
        #endregion

        RegisterNetMsgType<S2CRedPointStatesResponse>((int)MSGID.S2CRedPointStatesResponse, S2CRedPointStatesResponse.Parser, RedPointDataModel.DoRedPointData);

        RegisterNetMsgType<S2CAccountPlayerListResponse>((int)MSGID.S2CAccountPlayerListResponse, S2CAccountPlayerListResponse.Parser, ServerDataModel.DOAccountPlayer);
        RegisterNetMsgType<S2CGuideDataSaveResponse>((int)MSGID.S2CGuideDataSaveResponse, S2CGuideDataSaveResponse.Parser, NewBieGuide.GuideDataModel.DoGuideSaveDataResult);
        RegisterNetMsgType<S2CGuideDataResponse>((int)MSGID.S2CGuideDataResponse, S2CGuideDataResponse.Parser, NewBieGuide.GuideDataModel.DoGuideDataResponse);

        #region Expedition
        RegisterNetMsgType<S2CExpeditionDataResponse>((int)MSGID.S2CExpeditionDataResponse, S2CExpeditionDataResponse.Parser, ExpeditionDataModel.DoExpeditionData);
        RegisterNetMsgType<S2CExpeditionLevelDataResponse>((int)MSGID.S2CExpeditionLevelDataResponse, S2CExpeditionLevelDataResponse.Parser, ExpeditionDataModel.DoExpeditionStageData);
        RegisterNetMsgType<S2CExpeditionCurrLevelSync>((int)MSGID.S2CExpeditionCurrLevelSync, S2CExpeditionCurrLevelSync.Parser, ExpeditionDataModel.DoExpeditionCurrStageData);
        RegisterNetMsgType<S2CExpeditionPurifyRewardResponse>((int)MSGID.S2CExpeditionPurifyRewardResponse, S2CExpeditionPurifyRewardResponse.Parser, ExpeditionDataModel.DoExpeditionReward);
        RegisterNetMsgType<S2CExpeditionPurifyPointsSync>((int)MSGID.S2CExpeditionPurifyPointsSync, S2CExpeditionPurifyPointsSync.Parser, ExpeditionDataModel.DoExpeditionPurifyPoint);
        #endregion

        #region Artifact
        RegisterNetMsgType<S2CArtifactDataResponse>((int)MSGID.S2CArtifactDataResponse, S2CArtifactDataResponse.Parser, ArtifactDataModel.DoArtifactData);
        RegisterNetMsgType<S2CArtifactUnlockResponse>((int)MSGID.S2CArtifactUnlockResponse, S2CArtifactUnlockResponse.Parser, ArtifactDataModel.DoArtifactUnlock);
        RegisterNetMsgType<S2CArtifactLevelUpResponse>((int)MSGID.S2CArtifactLevelupResponse, S2CArtifactLevelUpResponse.Parser, ArtifactDataModel.DoArtifactUp);
        RegisterNetMsgType<S2CArtifactRankUpResponse>((int)MSGID.S2CArtifactRankupResponse, S2CArtifactRankUpResponse.Parser, ArtifactDataModel.DoArtifactRankUp);
        RegisterNetMsgType<S2CArtifactResetResponse>((int)MSGID.S2CArtifactResetResponse, S2CArtifactResetResponse.Parser, ArtifactDataModel.DoArtifactReset);
        RegisterNetMsgType<S2CArtifactDataUpdateNotify>((int)MSGID.S2CArtifactDataUpdateNotify, S2CArtifactDataUpdateNotify.Parser, ArtifactDataModel.DoArtifactNotify);
        #endregion

        #region Carnival
        RegisterNetMsgType<S2CCarnivalDataResponse>((int)MSGID.S2CCarnivalDataResponse, S2CCarnivalDataResponse.Parser, CarnivalDataModel.DoCarnivalData);
        RegisterNetMsgType<S2CCarnivalTaskSetResponse>((int)MSGID.S2CCarnivalTaskSetResponse, S2CCarnivalTaskSetResponse.Parser, CarnivalDataModel.DoCarnivalTaskSet);
        RegisterNetMsgType<S2CCarnivalItemExchangeResponse>((int)MSGID.S2CCarnivalItemExchangeResponse, S2CCarnivalItemExchangeResponse.Parser, CarnivalDataModel.DoCarnivalItemExchange);
        RegisterNetMsgType<S2CCarnivalShareResponse>((int)MSGID.S2CCarnivalShareResponse, S2CCarnivalShareResponse.Parser, CarnivalDataModel.DoCarnivalShare);
        RegisterNetMsgType<S2CCarnivalBeInvitedResponse>((int)MSGID.S2CCarnivalBeInvitedResponse, S2CCarnivalBeInvitedResponse.Parser, CarnivalDataModel.DoCarnivalBeInvited);
        RegisterNetMsgType<S2CCarnivalTaskDataNotify>((int)MSGID.S2CCarnivalTaskDataNotify, S2CCarnivalTaskDataNotify.Parser, CarnivalDataModel.DoCarnivalNotify);
        #endregion
    }

    public void ReqDataSync(params GameServerDataSync[] types)
    {
        C2SDataSyncRequest req = new C2SDataSyncRequest();
        if (types.Length > 0)
        {
            for (int i = 0; i < types.Length; i++)
            {
                switch (types[i])
                {
                    case GameServerDataSync.Base:
                        req.Base = true;
                        break;
                    case GameServerDataSync.Items:
                        req.Items = true;
                        break;
                    case GameServerDataSync.Roles:
                        req.Roles = true;
                        break;
                    case GameServerDataSync.Teams:
                        req.Teams = true;
                        break;
                    case GameServerDataSync.Campaigns:
                        req.Campaigns = true;
                        break;
                    case GameServerDataSync.Tower:
                        req.Tower = true;
                        break;
                    case GameServerDataSync.Arena:
                        req.Arena = true;
                        break;
                    case GameServerDataSync.Task:
                        req.Task = true;
                        break;
                    case GameServerDataSync.ActiveStage:
                        req.ActiveStage = true;
                        break;
                    case GameServerDataSync.Guild:
                        req.Guild = true;
                        break;
                    case GameServerDataSync.Friend:
                        req.Friend = true;
                        break;
                    case GameServerDataSync.Chat:
                        req.Chat = true;
                        break;
                    case GameServerDataSync.Explore:
                        req.Explore = true;
                        break;
                    case GameServerDataSync.Mail:
                        req.Mail = true;
                        break;
                    case GameServerDataSync.Sign:
                        req.Sign = true;
                        break;
                    case GameServerDataSync.SevenDays:
                        req.SevenDays = true;
                        break;
                    case GameServerDataSync.Talent:
                        req.Talent = true;
                        break;
                    case GameServerDataSync.GoldHand:
                        req.GoldHand = true;
                        break;
                    case GameServerDataSync.Guide:
                        req.Guide = true;
                        break;
                }
            }
        }
        Send(MSGID.C2SDataSyncRequest, req.ToByteString());
    }

    public void ReqBattleRecordData(int recordID)
    {
        C2SBattleRecordRequest req = new C2SBattleRecordRequest();
        req.Id = recordID;
        Send(MSGID.C2SBattleRecordRequest, req.ToByteString());
    }
    static bool isRank = false;
    public void ReqRankData(int rankType, bool blRank = false, params int[] args)
    {
        isRank = blRank;
        C2SRankListRequest req = new C2SRankListRequest();
        req.RankListType = rankType;
        if (args != null || args.Length > 0)
        {
            for (int i = 0; i < args.Length; i++)
                req.Params.Add(args[i]);
        }
        Send(MSGID.C2SRankListRequest, req.ToByteString());
    }

    public static void DoRankListResponse(S2CRankListResponse value)
    {
        if (isRank)
            RankDataModel.DoRank(value);
        else if(value.RankListType == 1)
            ArenaDataModel.DoRankListData(value);
    }

    public static void DoEnterGame(S2CEnterGameResponse value)
    {
        _blEnterGame = true;
        HeroDataModel.DoEnterGame(value);
    }

    public static void DoReconnect(S2CReconnectResponse value)
    {
        LoginHelper.mToken = value.NewToken;
        NetReconnectMgr.Instance.HideReconnect();
    }

    public void ReqAccountPlayer()
    {
        C2SAccountPlayerListRequest req = new C2SAccountPlayerListRequest();
        Send(MSGID.C2SAccountPlayerListRequest, req.ToByteString());
    }

    public void ReqLoginGameServer(string acc)
    {
        _blEnable = true;
        C2SEnterGameRequest enterGameReq = new C2SEnterGameRequest();
        enterGameReq.Acc = acc;

        Send(MSGID.C2SEnterGameRequest, enterGameReq.ToByteString());
    }

    public void ReqReconnect()
    {
        C2SReconnectRequest req = new C2SReconnectRequest();
        Send(MSGID.C2SReconnectRequest, req.ToByteString());
    }

    public void ReqPlayerChangeName(string newName)
    {
        C2SPlayerChangeNameRequest req = new C2SPlayerChangeNameRequest();
        req.NewName = newName;
        Send(MSGID.C2SPlayerChangeNameRequest, req.ToByteString());
    }

    public void ReqPlayerChangHead(int newHead)
    {
        C2SPlayerChangeHeadRequest req = new C2SPlayerChangeHeadRequest();
        req.NewHead = newHead;
        Send(MSGID.C2SPlayerChangeHeadRequest, req.ToByteString());
    }

    public void ReqBattle(C2SBattleResultRequest req)
    {
        Send(MSGID.C2SBattleResultRequest, req.ToByteString());
    }

    public void ReqTestCommand(string cmd, IList<string> args)
    {
        C2S_TEST_COMMAND req = new C2S_TEST_COMMAND();
        req.Cmd = cmd;
        if (args != null)
            req.Args.AddRange(args);
        Send(MSGID.C2STestCommand, req.ToByteString());
    }

    public void ReqItemFusion(int pieceId, int fusionCount)
    {
        C2SItemFusionRequest req = new C2SItemFusionRequest();
        req.FusionNum = fusionCount;
        req.PieceId = pieceId;

        Send(MSGID.C2SItemFusionRequest, req.ToByteString());
    }

    public void ReqItemSell(int ItemId, int ItemNum)
    {
        C2SItemSellRequest req = new C2SItemSellRequest();
        req.ItemId = ItemId;
        req.ItemNum = ItemNum;

        Send(MSGID.C2SItemSellRequest, req.ToByteString());
    }

    public void ReqCampaignData()
    {
        C2SCampaignDataRequest req = new C2SCampaignDataRequest();
        Send(MSGID.C2SCampaignDataRequest, req.ToByteString());
    }

    public void ReqAccelerate()
    {
        C2SCampaignAccelerateIncomeRequest req = new C2SCampaignAccelerateIncomeRequest();
        Send(MSGID.C2SCampaignAccelerateIncomeRequest, req.ToByteString());
    }

    public void ReqMailSend(int mailId, int mailType, string mailTitle, string mailContent, IList<ItemInfo> attachedItems = null)
    {
        C2SMailSendRequest req = new C2SMailSendRequest();
        req.ReceiverId = mailId;
        req.MailType = mailType;
        req.MailTitle = mailTitle;
        req.MailContent = mailContent;
        if (attachedItems != null)
        {
            req.AttachedItems.AddRange(attachedItems);
        }
        Send(MSGID.C2SMailSendRequest, req.ToByteString());
    }

    public void ReqMailList()
    {
        C2SMailListRequest req = new C2SMailListRequest();
        Send(MSGID.C2SMailListRequest, req.ToByteString());
    }

    public void ReqMailDetail(int mailID)
    {
        C2SMailDetailRequest req = new C2SMailDetailRequest();
        req.Ids.Add(mailID);
        Send(MSGID.C2SMailDetailRequest, req.ToByteString());
    }

    public void ReqMailDelete(IList<int> mailID)
    {
        C2SMailDeleteRequest req = new C2SMailDeleteRequest();
        req.MailIds.AddRange(mailID);
        Send(MSGID.C2SMailDeleteRequest, req.ToByteString());
    }

    public void ReqMailAttachedItems(List<int> mailID)
    {
        C2SMailGetAttachedItemsRequest req = new C2SMailGetAttachedItemsRequest();
        req.MailIds.AddRange(mailID);
        Send(MSGID.C2SMailGetAttachedItemsRequest, req.ToByteString());
    }

    public void ReqSetHangup(int campaignId)
    {
        C2SBattleSetHangupCampaignRequest req = new C2SBattleSetHangupCampaignRequest();
        req.CampaignId = campaignId;
        Send(MSGID.C2SBattleSetHangupCampaignRequest, req.ToByteString());
    }

    public void ReqDrawCard(int drawType)
    {
        C2SDrawCardRequest req = new C2SDrawCardRequest();
        req.DrawType = drawType;
        Send(MSGID.C2SDrawCardRequest, req.ToByteString());
    }

    public void ReqRoleDisplace(int groupId,int RoleId)
    {
        C2SRoleDisplaceRequest req = new C2SRoleDisplaceRequest();
        req.GroupId = groupId;
        req.RoleId = RoleId;
        Send(MSGID.C2SRoleDisplaceRequest, req.ToByteString());
    }
    public void ReqRoleDisplaceComfirm()
    {
        C2SRoleDisplaceConfirmRequest req = new C2SRoleDisplaceConfirmRequest();
        Send(MSGID.C2SRoleDisplaceConfirmRequest, req.ToByteString());
    }

    public void ReqDrawData()
    {
        C2SDrawDataRequest req = new C2SDrawDataRequest();
        Send(MSGID.C2SDrawDataRequest, req.ToByteString());
    }

    public void ReqGoldData()
    {
        C2SGoldHandDataRequest req = new C2SGoldHandDataRequest();
        Send(MSGID.C2SGoldHandDataRequest, req.ToByteString());
    }

    public void ReqGoldTou(int type)
    {
        C2STouchGoldRequest req = new C2STouchGoldRequest();
        req.Type = type;
        Send(MSGID.C2STouchGoldRequest, req.ToByteString());

    }

    public void ReqShopData(int type)
    {
        C2SShopDataRequest req = new C2SShopDataRequest();
        req.ShopId = type;
        Send(MSGID.C2SShopDataRequest, req.ToByteString());
    }

    public void ReqShopBuy(int shopId, int itemId, int buyNum = 1)
    {
        C2SShopBuyItemRequest req = new C2SShopBuyItemRequest();
        req.ShopId = shopId;
        req.ItemId = itemId;
        req.BuyNum = buyNum;
        Send(MSGID.C2SShopBuyItemRequest, req.ToByteString());
    }

    public void ReqShopRefresh(int shopId)
    {
        C2SShopRefreshRequest req = new C2SShopRefreshRequest();
        req.ShopId = shopId;
        Send(MSGID.C2SShopRefreshRequest, req.ToByteString());
    }

    public void ReqTaskData(int taskType)
    {
        C2STaskDataRequest req = new C2STaskDataRequest();
        req.TaskType = taskType;
        Send(MSGID.C2STaskDataRequest, req.ToByteString());
    }

    public void ReqTaskReward(int taskId)
    {
        C2STaskRewardRequest req = new C2STaskRewardRequest();
        req.TaskId = taskId;
        Send(MSGID.C2STaskRewardRequest, req.ToByteString());
    }


    /// <summary>
    /// get hangup reward
    /// </summary>
    /// <param name="rewardType"> 0--fixed reward, 1--random reward </param>
    public void ReqHangupReward(int rewardType)
    {
        C2SCampaignHangupIncomeRequest req = new C2SCampaignHangupIncomeRequest();
        req.IncomeType = rewardType;
        Send(MSGID.C2SCampaignHangupIncomeRequest, req.ToByteString());
    }

    public void ReqSetTeam(int type, IList<int> members, int artifactId)
    {
        C2SSetTeamRequest req = new C2SSetTeamRequest();
        req.TeamType = type;
        req.TeamMembers.AddRange(members);
        req.ArtifactId = artifactId;
        Send(MSGID.C2SSetTeamRequest, req.ToByteString());
    }

    public void ReqRoleHandBookData()
    {
        C2SRoleHandbookRequest request = new C2SRoleHandbookRequest();
        Send(MSGID.C2SRoleHandbookRequest, request.ToByteString());
    }

    public void ReqGuildAsk(int itemId,int itemNum)
    {
        C2SGuildAskDonateRequest req = new C2SGuildAskDonateRequest();
        req.ItemId = itemId;
        req.ItemNum = itemNum;
        Send(MSGID.C2SGuildAskDonateRequest, req.ToByteString());
    }

    public void ReqGuildLogs()
    {
        C2SGuildLogsRequest req = new C2SGuildLogsRequest();
        Send(MSGID.C2SGuildLogsRequest, req.ToByteString());
    }
    #region role levelup rankup decompose fusion lock
    public void ReqRoleLevelup(int roleID)
    {
        C2SRoleLevelUpRequest request = new C2SRoleLevelUpRequest();
        request.RoleId = roleID;
        Send(MSGID.C2SRoleLevelupRequest, request.ToByteString());
    }

    public void ReqRoleRankup(int roleId)
    {
        C2SRoleRankUpRequest req = new C2SRoleRankUpRequest();
        req.RoleId = roleId;
        Send(MSGID.C2SRoleRankupRequest, req.ToByteString());
    }

    public void ReqRoleDecompose(IList<int> roles)
    {
        C2SRoleDecomposeRequest req = new C2SRoleDecomposeRequest();
        req.RoleIds.AddRange(roles);
        Send(MSGID.C2SRoleDecomposeRequest, req.ToByteString());
    }

    public void ReqRoleFusion(int fusionCardId, int mainCardId, IList<int> listCard1 = null, IList<int> listCard2 = null, IList<int> listCard3 = null)
    {
        C2SRoleFusionRequest req = new C2SRoleFusionRequest();
        req.FusionId = fusionCardId;
        req.MainCardId = mainCardId;
        if (listCard1 != null)
            req.Cost1CardIds.AddRange(listCard1);
        if (listCard2 != null)
            req.Cost2CardIds.AddRange(listCard2);
        if (listCard3 != null)
            req.Cost3CardIds.AddRange(listCard3);
        Send(MSGID.C2SRoleFusionRequest, req.ToByteString());
    }

    public void ReqRoleLock(int roleId, bool isLock)
    {
        C2SRoleLockRequest req = new C2SRoleLockRequest();
        req.RoleId = roleId;
        req.IsLock = isLock;
        Send(MSGID.C2SRoleLockRequest, req.ToByteString());
    }

    #endregion

    #region tower 
    public void ReqTowerData()
    {
        C2STowerDataRequest req = new C2STowerDataRequest();
        Send(MSGID.C2STowerDataRequest, req.ToByteString());
    }
    public void ReqTowerRankingData()
    {
        C2STowerRankingListRequest req = new C2STowerRankingListRequest();
        Send(MSGID.C2STowerRankingListRequest, req.ToByteString());
    }
    public void ReqTowerRecordInfoData(int recordID)
    {
        C2STowerRecordsInfoRequest req = new C2STowerRecordsInfoRequest();
        req.TowerId = recordID;
        Send(MSGID.C2STowerRecordsInfoRequest, req.ToByteString());
    }
    public void ReqTowerRecordData(int TowerFightId)
    {
        C2STowerRecordDataRequest req = new C2STowerRecordDataRequest();
        req.TowerFightId = TowerFightId;
        Send(MSGID.C2STowerRecordDataRequest, req.ToByteString());
    }
    #endregion
    #region ActivityStage
    public void ReqActiveStageData(int StageType)
    {
        C2SActiveStageDataRequest req = new C2SActiveStageDataRequest();
        req.StageType = StageType;
        Send(MSGID.C2SActiveStageDataRequest, req.ToByteString());
    }

    public void ReqActiveStageBuyChallengeNumData(int stageType,int num)
    {
        C2SActiveStageBuyChallengeNumRequest req = new C2SActiveStageBuyChallengeNumRequest();
        req.StageType = stageType;
        req.Num = num;
        Send(MSGID.C2SActiveStageBuyChallengeNumRequest, req.ToByteString());
    }

    public void ReqActiveStageAssistRoleListData()
    {
        C2SActiveStageAssistRoleListRequest req = new C2SActiveStageAssistRoleListRequest();
        Send(MSGID.C2SActiveStageAssistRoleListRequest, req.ToByteString());
    }
    #endregion
    #region ExploreData
    // 请求探索数据
    public void ReqExploreData()
    {
        //LogHelper.Log("[GameServer.ReqExploreData() => 请求探索数据]");
        C2SExploreDataRequest req = new C2SExploreDataRequest();
        Send(MSGID.C2SExploreDataRequest, req.ToByteString());
    }
    // 选择探索角色
    public void ReqExploresSelRole(int id, bool isStory, List<int> roleIds)
    {
        C2SExploreSelRoleRequest req = new C2SExploreSelRoleRequest();
        req.Id = id;
        req.IsStory = isStory;
        req.RoleIds.Add(roleIds);
        Send(MSGID.C2SExploreSelRoleRequest, req.ToByteString());
    }
    // 开始探索
    public void ReqExploreStart(int id, bool isStory)
    {
        C2SExploreStartRequest req = new C2SExploreStartRequest();
        req.Ids.Add(id);
        req.IsStory = isStory;
        Send(MSGID.C2SExploreStartRequest, req.ToByteString());
    }
    // 加速
    public void ReqExploreSpeedup(int id, bool isStory)
    {
        C2SExploreSpeedupRequest req = new C2SExploreSpeedupRequest();
        req.Ids.Add(id);
        req.IsStory = isStory;
        Send(MSGID.C2SExploreSpeedupRequest, req.ToByteString());
    }
    // 刷新探索任务
    public void ReqExploreRefresh()
    {
        C2SExploreRefreshRequest req = new C2SExploreRefreshRequest();
        Send(MSGID.C2SExploreRefreshRequest, req.ToByteString());
    }
    // 锁定或解锁探索任务
    public void ReqExploreLock(int id, bool isLock)
    {
        C2SExploreLockRequest req = new C2SExploreLockRequest();
        //req.Ids = id;
        req.Ids.Add(id);
        req.IsLock = isLock;
        Send(MSGID.C2SExploreLockRequest, req.ToByteString());
    }
    // 探索奖励
    public void ReqExploreGetReward(int id, bool isStory)
    {
        C2SExploreGetRewardRequest req = new C2SExploreGetRewardRequest();
        req.Id = id;
        req.IsStory = isStory;
        Send(MSGID.C2SExploreGetRewardRequest, req.ToByteString());
    }
    //取消探索任务
    public void ReqExploreTaskRemove(int id,bool isStory)
    {
        C2SExploreCancelRequest req = new C2SExploreCancelRequest();
        req.Id = id;
        req.IsStory = isStory;
        Send(MSGID.C2SExploreCancelRequest, req.ToByteString());
    }
    #endregion
    #region equipment request

    public void ReqOpenLeftSlot(int roleId)
    {
        C2SRoleLeftSlotOpenRequest req = new C2SRoleLeftSlotOpenRequest();
        req.RoleId = roleId;
        Send(MSGID.C2SRoleLeftslotOpenRequest, req.ToByteString());
    }

    public void ReqWearEquipByOneKey(int roleId)
    {
        C2SRoleOneKeyEquipRequest req = new C2SRoleOneKeyEquipRequest();
        req.RoleId = roleId;
        Send(MSGID.C2SRoleOnekeyEquipRequest, req.ToByteString());
    }

    public void ReqTakeOffEquipByOnKey(int roleId)
    {
        C2SRoleOnekeyUnequipRequest req = new C2SRoleOnekeyUnequipRequest();
        req.RoleId = roleId;
        Send(MSGID.C2SRoleOnekeyUnequipRequest, req.ToByteString());
    }

    public void ReqWearEquip(int roleId, int itemId)
    {
        C2SItemEquipRequest req = new C2SItemEquipRequest();
        req.RoleId = roleId;
        req.ItemId = itemId;
        Send(MSGID.C2SItemEquipRequest, req.ToByteString());
    }

    public void ReqTakeoffEquip(int roleId, int slot)
    {
        C2SItemUnequipRequest req = new C2SItemUnequipRequest();
        req.RoleId = roleId;
        req.EquipSlot = slot;
        Send(MSGID.C2SItemUnequipRequest, req.ToByteString());
    }

    public void ReqUpgradeItemByOneKey(IList<int> items)
    {
        C2SItemOneKeyUpgradeRequest req = new C2SItemOneKeyUpgradeRequest();
        req.ItemIds.AddRange(items);
        Send(MSGID.C2SItemOnekeyUpgradeRequest, req.ToByteString());
    }

    /// <summary>
    /// 装备槽升级
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="itemId"></param>
    /// <param name="upGradeType">升级类型   左槽有两种，对应ItemUpgrade.xml表中的UpgradeType</param>
    public void ReqUpgradeItem(int roleId, int itemId, int upGradeType, int itemNum = 1)
    {
        C2SItemUpgradeRequest req = new C2SItemUpgradeRequest();
        req.RoleId = roleId;
        req.ItemId = itemId;
        req.ItemNum = itemNum;
        req.UpgradeType = upGradeType;
        Send(MSGID.C2SItemUpgradeRequest, req.ToByteString());
    }

    public void ReqUpGradeItemSave(int roleId)
    {
        C2SRoleLeftSlotResultSaveRequest req = new C2SRoleLeftSlotResultSaveRequest();
        req.RoleId = roleId;
        Send(MSGID.C2SRoleLeftslotResultSaveRequest, req.ToByteString());
    }

    #endregion

    #region Arena
    public void ReqArenaData()
    {
        C2SArenaDataRequest req = new C2SArenaDataRequest();
        Send(MSGID.C2SArenaDataRequest, req.ToByteString());
    }

    public void ReqPlayerDefenseTeam(int playerId)
    {
        C2SArenaPlayerDefenseTeamRequest req = new C2SArenaPlayerDefenseTeamRequest();
        req.PlayerId = playerId;
        Send(MSGID.C2SArenaPlayerDefenseTeamRequest, req.ToByteString());
    }

    public void ReqMatchPlayer()
    {
        C2SArenaMatchPlayerRequest req = new C2SArenaMatchPlayerRequest();
        Send(MSGID.C2SArenaMatchPlayerRequest, req.ToByteString());
    }

    public void ReqBattleRecord()
    {
        C2SBattleRecordListRequest req = new C2SBattleRecordListRequest();
        Send(MSGID.C2SBattleRecordListRequest, req.ToByteString());
    }

    #endregion

    #region friend
    public void ReqFriendList()
    {
        C2SFriendListRequest req = new C2SFriendListRequest();
        Send(MSGID.C2SFriendListRequest, req.ToByteString());
    }

    public void ReqRecommendFriend()
    {
        C2SFriendRecommendRequest req = new C2SFriendRecommendRequest();
        Send(MSGID.C2SFriendRecommendRequest, req.ToByteString());
    }

    //请求添加好友
    public void ReqAskFriend(params int[] args)
    {
        if (args == null || args.Length == 0)
            return;
        C2SFriendAskRequest req = new C2SFriendAskRequest();
        for (int i = 0; i < args.Length; i++)
            req.PlayerIds.Add(args[i]);
        Send(MSGID.C2SFriendAskRequest, req.ToByteString());
    }

    public void ReqFriendAskPlayerList()
    {
        C2SFriendAskPlayerListRequest req = new C2SFriendAskPlayerListRequest();
        Send(MSGID.C2SFriendAskPlayerListRequest, req.ToByteString());
    }

    public void ReqAgreeFriend(params int[] args)
    {
        if (args == null || args.Length == 0)
            return;
        C2SFriendAgreeRequest req = new C2SFriendAgreeRequest();
        for (int i = 0; i < args.Length; i++)
            req.PlayerIds.Add(args[i]);
        Send(MSGID.C2SFriendAgreeRequest, req.ToByteString());
    }

    public void ReqRefuseFriend(params int[] args)
    {
        if (args == null || args.Length == 0)
            return;
        C2SFriendRefuseRequest req = new C2SFriendRefuseRequest();
        for (int i = 0; i < args.Length; i++)
            req.PlayerIds.Add(args[i]);
        Send(MSGID.C2SFriendRefuseRequest, req.ToByteString());
    }

    public void ReqRemoveFriend(params int[] args)
    {
        if (args == null || args.Length == 0)
            return;
        C2SFriendRemoveRequest req = new C2SFriendRemoveRequest();
        for (int i = 0; i < args.Length; i++)
            req.PlayerIds.Add(args[i]);
        Send(MSGID.C2SFriendRemoveRequest, req.ToByteString());
    }

    public void ReqGivePointsToFriend(params int[] args)
    {
        if (args == null || args.Length == 0)
            return;
        C2SFriendGivePointsRequest req = new C2SFriendGivePointsRequest();
        for (int i = 0; i < args.Length; i++)
            req.FriendIds.Add(args[i]);
        Send(MSGID.C2SFriendGivePointsRequest, req.ToByteString());
    }

    public void ReqGetPointsFromFriend(params int[] args)
    {
        if (args == null || args.Length == 0)
            return;
        C2SFriendGetPointsRequest req = new C2SFriendGetPointsRequest();
        for (int i = 0; i < args.Length; i++)
            req.FriendIds.Add(args[i]);
        Send(MSGID.C2SFriendGetPointsRequest, req.ToByteString());
    }

    public void ReqOnKeyGetAndSendPoints(IList<int> players)
    {
        if (players == null || players.Count == 0)
            return;
        C2SFriendGiveAndGetPointsRequest req = new C2SFriendGiveAndGetPointsRequest();
        req.FriendIds.AddRange(players);
        Send(MSGID.C2SFriendGiveAndGetPointsRequest, req.ToByteString());
    }

    public void ReqFriendAssistData()
    {
        C2SFriendDataRequest req = new C2SFriendDataRequest();
        Send(MSGID.C2SFriendDataRequest, req.ToByteString());
    }

    public void ReqSetAssistRole(int cardId)
    {
        C2SFriendSetAssistRoleRequest req = new C2SFriendSetAssistRoleRequest();
        req.RoleId = cardId;
        Send(MSGID.C2SFriendSetAssistRoleRequest, req.ToByteString());
    }

    public void ReqFriendAssistPoint()
    {
        C2SFriendGetAssistPointsRequest req = new C2SFriendGetAssistPointsRequest();
        Send(MSGID.C2SFriendGetAssistPointsRequest, req.ToByteString());
    }

    public void ReqSearchFriendBoos()
    {
        C2SFriendSearchBossRequest req = new C2SFriendSearchBossRequest();
        Send(MSGID.C2SFriendSearchBossRequest, req.ToByteString());
    }
    #endregion
    #region Guild 
    public void ReqGuildData()
    {
        Send(MSGID.C2SGuildDataRequest, new C2SGuildDataRequest().ToByteString());
    }

    public void ReqRecommemdGuild()
    {
        Send(MSGID.C2SGuildRecommendRequest, new C2SGuildRecommendRequest().ToByteString());
    }

    public void ReqSearchGuild(string key)
    {
        C2SGuildSearchRequest req = new C2SGuildSearchRequest();
        req.Key = key;
        Send(MSGID.C2SGuildSearchRequest, req.ToByteString());
    }

    public void ReqCreateGuild(string name, int logo, string notices)
    {
        C2SGuildCreateRequest req = new C2SGuildCreateRequest();
        req.GuildName = name;
        req.GuildLogo = logo;
        req.Anouncement = ByteString.CopyFromUtf8(notices);
        Send(MSGID.C2SGuildCreateRequest, req.ToByteString());
    }

    public void ReqDismissGuild()
    {
        Send(MSGID.C2SGuildDismissRequest, new C2SGuildDismissRequest().ToByteString());
    }

    public void ReqCancelDismissGuild()
    {
        Send(MSGID.C2SGuildCancelDismissRequest, new C2SGuildCancelDismissRequest().ToByteString());
    }

    public void ReqGuildInfoModify(string newName, int newLogo)
    {
        C2SGuildInfoModifyRequest req = new C2SGuildInfoModifyRequest();
        req.NewGuildName = newName;
        req.NewGuildLogo = newLogo;
        Send(MSGID.C2SGuildInfoModifyRequest, req.ToByteString());
    }

    public void ReqGuildNoticeModify(string value)
    {
        C2SGuildAnouncementRequest req = new C2SGuildAnouncementRequest();
        req.Content = value;
        Send(MSGID.C2SGuildAnouncementRequest, req.ToByteString());
    }

    public void ReqGuildMembers()
    {
        Send(MSGID.C2SGuildMembersRequest, new C2SGuildMembersRequest().ToByteString());
    }

    public void ReqGuildAskListMembers()
    {
        Send(MSGID.C2SGuildAskListRequest, new C2SGuildAskListRequest().ToByteString());
    }

    public void ReqAskJoinGuild(int guildId)
    {
        C2SGuildAskJoinRequest req = new C2SGuildAskJoinRequest();
        req.GuildId = guildId;
        Send(MSGID.C2SGuildAskJoinRequest, req.ToByteString());
    }

    public void ReqAgreeJoinGuild(bool blRefuse, params int[] args)
    {
        if (args != null && args.Length == 0)
            return;
        C2SGuildAgreeJoinRequest req = new C2SGuildAgreeJoinRequest();
        for (int i = 0; i < args.Length; i++)
            req.PlayerIds.Add(args[i]);
        req.IsRefuse = blRefuse;
        Send(MSGID.C2SGuildAgreeJoinRequest, req.ToByteString());
    }

    public void ReqGuildSign()
    {
        Send(MSGID.C2SGuildSignInRequest, new C2SGuildSignInRequest().ToByteString());
    }

    public void ReqDonateList()
    {
        Send(MSGID.C2SGuildDonateListRequest, new C2SGuildDonateListRequest().ToByteString());
    }

    public void ExitGuild()
    {
        Send(MSGID.C2SGuildQuitRequest, new C2SGuildQuitRequest().ToByteString());
    }

    public void ReqGuildDonate(int playerId,int itemId,int itemNum)
    {
        C2SGuildDonateRequest req = new C2SGuildDonateRequest();
        req.PlayerId = playerId;
        req.ItemId = itemId;
        req.ItemNum = itemNum;
        Send(MSGID.C2SGuildDonateRequest, req.ToByteString());
    }

    public void ReqGuildRecruit(string value)
    {
        C2SGuildRecruitRequest req = new C2SGuildRecruitRequest();
        req.Content = ByteString.CopyFromUtf8(value);
        Send(MSGID.C2SGuildRecruitRequest, req.ToByteString());
    }

    public void ReqSetOfficer(int playerID, int setType)
    {
        C2SGuildSetOfficerRequest req = new C2SGuildSetOfficerRequest();
        req.PlayerIds.Add(playerID);
        req.SetType = setType;
        Send(MSGID.C2SGuildSetOfficerRequest, req.ToByteString());
    }

    public void ReqKickMember(int playerID)
    {
        C2SGuildKickMemberRequest req = new C2SGuildKickMemberRequest();
        req.PlayerIds.Add(playerID);
        Send(MSGID.C2SGuildKickMemberRequest, req.ToByteString());
    }

    public void ReqChangePresident(int playerId)
    {
        C2SGuildChangePresidentRequest req = new C2SGuildChangePresidentRequest();
        req.NewPresidentId = playerId;
        Send(MSGID.C2SGuildChangePresidentRequest, req.ToByteString());
    }
    #endregion
    #region chat
    public void ReqChat( int channel,string content)
    {
        C2SChatRequest req = new C2SChatRequest();
        req.Channel = channel;
        req.Content = ByteString.CopyFromUtf8(content); 
        Send(MSGID.C2SChatRequest, req.ToByteString());
    }
    public void ReqChatMsgPull(int channel)
    {
        C2SChatMsgPullRequest req = new C2SChatMsgPullRequest();
        req.Channel = channel;
        Send(MSGID.C2SChatMsgPullRequest, req.ToByteString());
    }
    #endregion

    #region Guild Boss
    public void ReqStageData()
    {
        C2SGuildStageDataRequest req = new C2SGuildStageDataRequest();
        Send(MSGID.C2SGuildStageDataRequest, req.ToByteString());
    }

    public void ReqStageDamage(int bossId)
    {
        C2SGuildStageRankListRequest req = new C2SGuildStageRankListRequest();
        req.BossId = bossId;
        Send(MSGID.C2SGuildStageRankListRequest, req.ToByteString());
    }

    public void ReqStagePlaterRespawn()
    {
        C2SGuildStagePlayerRespawnRequest req = new C2SGuildStagePlayerRespawnRequest();
        Send(MSGID.C2SGuildStagePlayerRespawnRequest, req.ToByteString());
    }

    public void ReqStageReset()
    {
        C2SGuildStageResetRequest req = new C2SGuildStageResetRequest();
        Send(MSGID.C2SGuildStageResetRequest, req.ToByteString());
    }
    #endregion

    #region Talent
    public void ReqTalentData()
    {
        C2STalentListRequest req = new C2STalentListRequest();
        Send(MSGID.C2STalentListRequest, req.ToByteString());
    }

    public void ReqTalentUp(int talentId)
    {
        C2STalentUpRequest req = new C2STalentUpRequest();
        req.TalentId = talentId;
        Send(MSGID.C2STalentUpRequest, req.ToByteString());
    }

    public void ReqTalentReset(int tag)
    {
        C2STalentResetRequest req = new C2STalentResetRequest();
        req.Tag = tag;
        Send(MSGID.C2STalentResetRequest, req.ToByteString());
    }
    #endregion

    #region Boon
    //Sign
    public void ReqSignData()
    {
        C2SSignDataRequest req = new C2SSignDataRequest();
        Send(MSGID.C2SSignDataRequest, req.ToByteString());
    }

    public void ReqSignAward(int index)
    {
        C2SSignAwardRequest req = new C2SSignAwardRequest();
        req.Index = index;
        Send(MSGID.C2SSignAwardRequest, req.ToByteString());
    }
    //Seven
    public void ReqSevenData()
    {
        C2SSevenDaysDataRequest req = new C2SSevenDaysDataRequest();
        Send(MSGID.C2SSevendaysDataRequest, req.ToByteString());
    }

    public void ReqSevenAward()
    {
        C2SSevenDaysAwardRequest req = new C2SSevenDaysAwardRequest();
        Send(MSGID.C2SSevendaysAwardRequest, req.ToByteString());
    }
    //Welfare
    public void ReqActivityData()
    {
        C2SActivityDataRequest req = new C2SActivityDataRequest();
        Send(MSGID.C2SActivityDataRequest, req.ToByteString());
    }
    #endregion

    #region Recharge
    public void ReqRechargeData()
    {
        C2SChargeDataRequest req = new C2SChargeDataRequest();
        Send(MSGID.C2SChargeDataRequest, req.ToByteString());
    }

    public void ReqRecharge(string bundleId)
    {
        C2SChargeRequest req = new C2SChargeRequest();
        req.BundleId = bundleId;
        Send(MSGID.C2SChargeRequest, req.ToByteString());
    }

    public void FirstAward()
    {
        C2SChargeFirstAwardRequest req = new C2SChargeFirstAwardRequest();
        Send(MSGID.C2SChargeFirstAwardRequest, req.ToByteString());
    }

    public void ReqCharge(int channel, string bundleID, string payload, string extraData, int clientIdx)
    {
        //LogHelper.LogWarning("[GameServer.ReqCharge() => channel:" + channel + ", bundleid:" + bundleID + ", payload:" + payload + ", extradata:" + extraData + "]");
        C2SChargeRequest req = new C2SChargeRequest();
        req.BundleId = bundleID;
        req.Channel = channel;
        req.ClientIndex = clientIdx;
        req.PurchareData = ByteString.CopyFromUtf8(payload);
        req.ExtraData = ByteString.CopyFromUtf8(extraData);
        Send(MSGID.C2SChargeRequest, req.ToByteString());
    }
    #endregion

    public void SaveGuideData(int guideId, int stepId)
    {
        C2SGuideDataSaveRequest req = new C2SGuideDataSaveRequest();
        string guideValue = guideId + "_" + stepId;
        req.Data = ByteString.CopyFromUtf8(guideValue);
        Send(MSGID.C2SGuideDataSaveRequest, req.ToByteString());
    }

    public void ReqRedPointState(params RedPointEnum[] args)
    {
        C2SRedPointStatesRequest req = new C2SRedPointStatesRequest();
        if (args != null && args.Length > 0)
        {
            for (int i = 0; i < args.Length; i++)
                req.Modules.Add((int)args[i]);
        }
        Send(MSGID.C2SRedPointStatesRequest, req.ToByteString());
    }

    #region Expedition
    public void ReqExpeditionData()
    {
        C2SExpeditionDataRequest req = new C2SExpeditionDataRequest();
        Send(MSGID.C2SExpeditionDataRequest, req.ToByteString());
    }

    public void ReqExpeditionStageData()
    {
        C2SExpeditionLevelDataRequest req = new C2SExpeditionLevelDataRequest();
        Send(MSGID.C2SExpeditionLevelDataRequest, req.ToByteString());
    }

    public void ReqExpeditionReward()
    {
        C2SExpeditionPurifyRewardRequest req = new C2SExpeditionPurifyRewardRequest();
        Send(MSGID.C2SExpeditionPurifyRewardRequest, req.ToByteString());
    }
    #endregion

    #region Artifact
    //神器数据
    public void ReqArtifactData()
    {
        C2SArtifactDataRequest req = new C2SArtifactDataRequest();
        Send(MSGID.C2SArtifactDataRequest, req.ToByteString());
    }
    //神器解锁
    public void ReqArtifactUnlock(int id)
    {
        C2SArtifactUnlockRequest req = new C2SArtifactUnlockRequest();
        req.Id = id;
        Send(MSGID.C2SArtifactUnlockRequest, req.ToByteString());
    }
    //神器升级
    public void ReqArtifactUp(int id)
    {
        C2SArtifactLevelUpRequest req = new C2SArtifactLevelUpRequest();
        req.Id = id;
        Send(MSGID.C2SArtifactLevelupRequest, req.ToByteString());
    }
    //神器升阶
    public void ReqArtifactRankUp(int id)
    {
        C2SArtifactRankUpRequest req = new C2SArtifactRankUpRequest();
        req.Id = id;
        Send(MSGID.C2SArtifactRankupRequest, req.ToByteString());
    }
    //神器重铸
    public void ReqArtifactReset(int id)
    {
        C2SArtifactResetRequest req = new C2SArtifactResetRequest();
        req.Id = id;
        Send(MSGID.C2SArtifactResetRequest, req.ToByteString());
    }
    #endregion
    #region Carnival
    public void ReqCarnivalData()
    {
        C2SCarnivalDataRequest req = new C2SCarnivalDataRequest();
        Send(MSGID.C2SCarnivalDataRequest, req.ToByteString());
    }

    public void ReqCarnivalTaskSet(int taskId)
    {
        C2SCarnivalTaskSetRequest req = new C2SCarnivalTaskSetRequest();
        req.TaskId = taskId;
        Send(MSGID.C2SCarnivalTaskSetRequest, req.ToByteString());
    }

    public void ReqCarnivalItemExchange(int taskId)
    {
        C2SCarnivalItemExchangeRequest req = new C2SCarnivalItemExchangeRequest();
        req.TaskId = taskId;
        Send(MSGID.C2SCarnivalItemExchangeRequest, req.ToByteString());
    }

    public void ReqCarnivalShare()
    {
        C2SCarnivalShareRequest req = new C2SCarnivalShareRequest();
        Send(MSGID.C2SCarnivalShareRequest, req.ToByteString());
    }

    public void ReqCarnivalBeInvited(string code)
    {
        C2SCarnivalBeInvitedRequest req = new C2SCarnivalBeInvitedRequest();
        req.InviteCode = code;
        Send(MSGID.C2SCarnivalBeInvitedRequest, req.ToByteString());
    }
    #endregion

    protected override void Send(MSGID msgId, ByteString value)
    {
        base.Send(msgId, value);
        if (msgId != MSGID.C2SHeartbeat)
            _flHeartBeatTime = HeartBeatTime;
    }

    private bool _blShowMask = false;

    protected override void OnDataError()
    {
        base.OnDataError();
        GameNetMgr.Instance.AddNetErrorCount();
    }

    protected override void DoRequestSend()
    {
        _curRequest = new GamePostRequest(GameNetMgr.GAME_LOGIC_URL, OnReceiveData, OnDataError);

        C2S_MSG_DATA data = new C2S_MSG_DATA();
        data.Token = LoginHelper.mToken;
        C2S_ONE_MSG oneMsg;
        while (_postDataPools.Count > 0)
        {
            oneMsg = _postDataPools.Dequeue();
            if (oneMsg.MsgCode == (int)MSGID.C2SEnterGameRequest)
            {
                data.MsgList.Clear();
                _curRequest.AddMsgID(oneMsg.MsgCode);
                data.MsgList.Add(oneMsg);
                _postDataPools.Clear();
                _lstRequestID.Clear();
                break;
            }
            _curRequest.AddMsgID(oneMsg.MsgCode);
            data.MsgList.Add(oneMsg);
        }
        _curRequest.InitPostData(data.ToByteArray());
        _curRequest.Send();
    }

    private S2C_ONE_MSG recOneMsg;
    protected override void OnReceiveData(object data)
    {
        S2C_MSG_DATA pbValue = data as S2C_MSG_DATA;

        for (int i = 0; i < pbValue.MsgList.Count; i++)
        {
            recOneMsg = pbValue.MsgList[i];
            if (recOneMsg.ErrorCode < NetErrorCode.None)
            {
                NetErrorHelper.DoErrorCode(recOneMsg.ErrorCode);
                GameNetMgr.Instance.ResetNetErrorCount();
            }
            else
            {
                ProccessOneMsg(recOneMsg);
                GameNetMgr.Instance.ResetNetErrorCount();
            }
            _flShowMaskTime = 0.5f;
        }
        if (_postDataPools.Count == 0)
        {
            if (_blShowMask)
            {
                _blShowMask = false;
                LoadingMgr.Instance.HideRechargeMask();
            }
        }
    }

    #region HeartBeat
    private static bool _blEnterGame;
    private static float _flHeartBeatTime = HeartBeatTime;
    private const float HeartBeatTime = 10f;

    private void SendHeartBeat()
    {
        C2SHeartbeat req = new C2SHeartbeat();
        Send(MSGID.C2SHeartbeat, req.ToByteString());
        
        HeroDataModel.Instance.CheckCacheOrder();
    }
    #endregion

    private float _flShowMaskTime = 0.5f;
    public override void Update()
    {
        base.Update();
        if (_blEnterGame)
        {
            _flHeartBeatTime -= Time.deltaTime;
            if (_flHeartBeatTime <= 0.01f)
            {
                _flHeartBeatTime = HeartBeatTime;
                SendHeartBeat();
            }
        }
        if (_curRequest != null && !_curRequest.BlEnd)
        {
            if (_curRequest.mBlNeedCheckMask)
            {
                if (_flShowMaskTime <= 0.01f)
                {
                    _blShowMask = true;
                    LoadingMgr.Instance.ShowRechargeMask();
                    return;
                }
                _flShowMaskTime -= UnityEngine.Time.deltaTime;
            }
        }
    }
}