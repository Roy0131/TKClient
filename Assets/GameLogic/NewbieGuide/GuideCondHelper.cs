using UnityEngine;

namespace NewBieGuide
{
    public class GuideCondHelper
    {
        public static bool CheckCondition(int conditionId)
        {
            switch (conditionId)
            {
                case GuideJumpCondConst.RecruitNormalJump:
                    return RecruitDataModel.Instance.mAllRecruits[0].LastTime > 0;
                case GuideJumpCondConst.RecruitAdvanceBtn:
                    return RecruitDataModel.Instance.mAllRecruits[1].LastTime > 0;
                case GuideJumpCondConst.HangupChapter2:
                    return HangupDataModel.Instance.mIntHangupCampaignId >= 2;
                case GuideJumpCondConst.HangupChapter3:
                    return HangupDataModel.Instance.mIntUnlockCampaignId >= 3;
                case GuideJumpCondConst.RoleLevel1:
                    return OnLevel(1);
                case GuideJumpCondConst.RoleLevel2:
                    return OnLevel(2);
                case GuideJumpCondConst.RoleLevel3:
                    return OnLevel(3);
                case GuideJumpCondConst.RoleLevel4:
                    return OnLevel(4);
                case GuideJumpCondConst.BattleBtn1:
                    return HangupDataModel.Instance.mIntUnlockCampaignId > 1;
                case GuideJumpCondConst.BattleBtn2:
                    return HangupDataModel.Instance.mIntUnlockCampaignId > 2;
                case GuideJumpCondConst.ArenaJump:
                    return LocalDataMgr.GetBattleTeamCards(TeamType.Defense).Count > 0;
                case GuideJumpCondConst.EquipCount:
                    return BagDataModel.Instance.GetItemCountById(10001) < 3;
            }
            return false;
        }

        public static bool CheckEnterCondition(int enterCondID)
        {
            if (enterCondID == EnterCondConst.HangupProgressOver)
                return LocalDataMgr.CheckCampFirstBattle(2);
            return false;
        }

        private static bool OnLevel(int level)
        {
            for (int i = 0; i < HeroDataModel.Instance.mAllCards.Count; i++)
            {
                if (HeroDataModel.Instance.mAllCards[i].mCardLevel > level)
                    return true;
            }
            return false;
        }
    }

    

    public class RectPoint
    {
        class Point
        {
            public float x, y;
            public void Init(float x, float y)
            {
                this.x = x;
                this.y = y;
            }
        }

        private Point _p1 = new Point();
        private Point _p2 = new Point();
        private Point _p3 = new Point();
        private Point _p4 = new Point();

        public void Init(float x, float y, float width, float height)
        {
            _p1.Init(x - width, y + height);
            _p2.Init(x - width, y - height);
            _p3.Init(x + width, y - height);
            _p4.Init(x + width, y + height);
        }

        Point p = new Point();
        public bool InRange(Vector2 pos)
        {
            p.Init(pos.x, pos.y);
            //Debug.LogWarning("InRange, pos:" + pos + ", p1:" + _p1 + ", p2:" + _p2 + ", p3:" + _p3 + ", p4:" + _p4);
            return GetCross(_p1, _p2, p) * GetCross(_p3, _p4, p) >= 0 && GetCross(_p2, _p3, p) * GetCross(_p4, _p1, p) >= 0;
        }

        // 计算 |p1 p2| X |p1 p|
        float GetCross(Point p1, Point p2, Point p)
        {
            return (p2.x - p1.x) * (p.y - p1.y) - (p.x - p1.x) * (p2.y - p1.y);
        }
    }
}
