using Framework.UI;
using Spine;
using Spine.Unity;
using UnityEngine;

public class AnimatorFighter : SceneRenderUnit
{
    protected SkeletonAnimation _animator;
    protected bool _blPlaying = false;

    private Vector3 _hitWorldPosition;
    protected FighterMaterialEffect _beHitEffect;
    public AnimatorFighter(BattleUnitType type)
        : base(type)
    {
        
    }

    protected virtual void OnPlayEnd(TrackEntry entry)
    {
        _blPlaying = false;
    }

    public virtual void PlayAction(string action, bool loop = false)
    {
        //Debuger.Log("play action:" + action + ", curAction:" + _animator.AnimationName);
        if (string.IsNullOrEmpty(action))
        {
            LogHelper.LogWarning("action:" + action + " was invalid");
            return;
        }
        if (_animator == null || _animator.skeleton.Data.FindAnimation(action) == null || _animator.AnimationName == action)
        {
            //Debuger.LogWarning("[action:" + action + " was playing]");
            return;
        }
        if (_blPlaying && action != ActionName.Death)
            return;
        _animator.state.SetAnimation(0, action, loop);
        _blPlaying = action != ActionName.Idle;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        ParseAnimator();
        PlayAction(ActionName.Idle, true);
    }

    protected virtual void ParseAnimator()
    {
        _animator = _modelObject.GetComponent<SkeletonAnimation>();
        if (_animator == null)
            return;
        _animator.state.Complete += OnPlayEnd;
    }

    public Vector3 FireDummyWorldPosition
    {
        get
        {
            Vector3 result = Vector3.zero;
            string dummyName = "fire_dummy" + "_" + _animator.AnimationName;
            Bone fireBone = _animator.skeleton.FindBone(dummyName);
            if (fireBone == null)
            {
                LogHelper.Log("AnimatorFighter.GetFireDummyPos() => role model:" + _modelName + " fire dummy:" + dummyName + " was not found!!");
                return Vector3.zero;
            }
            result = fireBone.GetWorldPosition(_animator.transform);
            return result;
        }
    }

    public Vector3 mHitWorldPosition
    {
        get
        {
            //if (_hitWorldPosition.Equals(Vector3.positiveInfinity))
            Bone hitBone = _animator.skeleton.FindBone("hit_dummy");
            if (hitBone != null)
                _hitWorldPosition = hitBone.GetWorldPosition(_modelObject.transform);
            else
            {
                _hitWorldPosition = mUnitRoot.position;
                LogHelper.LogError("[AnimatorFighter.mHitWorldPosition() => model name:" + _modelName + " hit_dummy not found!!!]");
            }
            return _hitWorldPosition;

        }
    }

	protected override void OnDispose()
	{
		if (_animator != null)
		{
            _animator.state.Complete -= OnPlayEnd;
            RefreshTimeScale(1f);
			_animator = null;
		}
        if(_beHitEffect != null)
        {
            _beHitEffect.Dispose();
            _beHitEffect = null;
        }
        base.OnDispose();
	}

    public SkeletonAnimation Animator
    {
        get { return _animator; }
    }

    public void RefreshTimeScale(float timeScale)
    {
        if (_animator.timeScale.Equals(timeScale))
            return;
        _animator.timeScale = timeScale;
    }
}