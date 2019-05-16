using UnityEngine;

public class MTTargetAttackAction : SkillActionBase
{
    private MoveActionDataVO _actionDataVO;
    protected MoveAction _moveAction;
    protected override void OnStart()
    {
        if (!mActionItemData.HasBehitFighters)
        {
            LogHelper.Log("MMTargetAttackAction has no targeter fighter, skillID:" + mActionItemData.mSkillConfig.ID);
            _status = AttackNodeStatus.Attacking;
        }
        else
        {
            _attacker.ChangeSortLayerOffest(RenderLayerOffset.FighterAttack);
            _moveAction = new MoveAction();
            _status = AttackNodeStatus.MoveToAttack;
            _actionDataVO = new MoveActionDataVO();
            Vector3 targetPos = GetTargetPos();
            if (!string.IsNullOrEmpty(mActionItemData.mSkillConfig.MoveTargetOffset))
            {
                string[] tmp = mActionItemData.mSkillConfig.MoveTargetOffset.Split(',');
                float offsetX = float.Parse(tmp[0]);
                float offsetY = float.Parse(tmp[1]);
                float x = targetPos.x;
                float y = targetPos.y + offsetY;
                if (_attacker.mDefaultPos.x < 0.01f)
                    x += offsetX;
                else
                    x -= offsetX;
                targetPos.Set(x, y, 0f);
            }
            if (mActionItemData.mSkillConfig.CastAnimBeforeMove != 0)
            {
                _attacker.PlayAction(mActionItemData.mSkillConfig.CastAnim);
                if (!string.IsNullOrWhiteSpace(mActionItemData.mSkillConfig.CastSound))
                    SoundMgr.Instance.PlayEffectSound(mActionItemData.mSkillConfig.CastSound, mActionItemData.mSkillConfig.CastSoundDelay, false);
                string moveTimeInCast = mActionItemData.mSkillConfig.MoveTimeInCast;
                if (!string.IsNullOrEmpty(moveTimeInCast))
                {
                    string[] tmp = moveTimeInCast.Split(',');
                    int delayTime = int.Parse(tmp[0]);
                    int endTime = int.Parse(tmp[1]);
                    int moveTime = endTime - delayTime;
                    _damageOffsetFrame = endTime + delayTime;
                    _actionDataVO.InitData(_attacker, targetPos, moveTime, delayTime);
                }else
                {
                    _actionDataVO.InitData(_attacker, targetPos, GameConst.BATTLE_MOVE_TIME);
                }
            }
            else
            {
                float flDist = Vector3.Distance(targetPos, _attacker.mUnitRoot.localPosition);
                float time = flDist / 15;
                int frame = (int)(time * GameConst.BATTLE_MOVE_SPEED);
                _actionDataVO.InitData(_attacker, targetPos, frame);
            }
            _moveAction.InitData(_actionDataVO);
        }
    }

    protected override Vector3 GetTargetPos()
    {
        if (mActionItemData.mSkillConfig.SkillAnimType == 3)
        {
			//JZH修改 :独立一个取被击者INDEX函数，先调用该函数
			int target_index = BattleManager.Instance.mBattleScene.GetSkillTargetIndex (_curBehitFighters[0].mSeatIndex, mActionItemData.mSkillConfig.RangeType);
			return BattleManager.Instance.mBattleScene.GetMoveToSeatPosition(_curBehitFighters[0].mSide, target_index);
        }
        return base.GetTargetPos();
    }

    protected override void OnUpdate()
    {
		base.OnUpdate();
        if (_status == AttackNodeStatus.MoveReset || _status == AttackNodeStatus.MoveToAttack)
            OnMove();
    }

    private void OnMove()
    {
        _moveAction.Update();
        if (_moveAction.mblMoveEnd)
        {
            if (_status == AttackNodeStatus.MoveToAttack)
                OnStartAttack();
            else
                OnActionEnd();
        }
    }

    protected override void OnAttackEnd()
    {
        ResetFighterAction();
        _status = AttackNodeStatus.MoveReset;
        _actionDataVO.InitData(_attacker, _attacker.mDefaultPos, GameConst.BATTLE_MOVE_TIME);
        _moveAction.InitData(_actionDataVO);
    }

    protected override void OnDispose()
    {
        if (_moveAction != null)
        {
            _moveAction.Dispose();
            _moveAction = null;
        }
        if (_actionDataVO != null)
        {
            _actionDataVO.Dispose();
            _actionDataVO = null;
        }
        base.OnDispose();
    }
}