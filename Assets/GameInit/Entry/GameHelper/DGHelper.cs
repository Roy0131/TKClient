using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public static class DGEaseType
{
    public const int None = 0;

    public const int InOutQuad = 1;
    public const int InBack = 2;
    public const int OutQuad = 3;
    public const int OutBounce = 4;

    public static Ease GetDGEase(int type)
    {
        switch (type)
        {
            case InOutQuad:
                return Ease.InOutQuad;
            case InBack:
                return Ease.InBack;
            case OutQuad:
                return Ease.OutQuad;
            case OutBounce:
                return Ease.OutBounce;
        }
        return Ease.Linear;
    }
}

public static class DGHelper
{

    #region tween do kill
    public static void DoKill(RectTransform target)
    {
        target.DOKill();
    }

    public static void DoKill(Text target)
    {
        target.DOKill();
    }

    public static void DoKill(Image target)
    {
        target.DOKill();
    }
    #endregion

    #region do fade logic
    public static void DoTextFade(Text target, float endValue, float duration, int easeType = 0, Action callBack = null)
    {
        TweenCallback OnEnd = () =>
        {
            if (callBack != null)
                callBack();
        };

        if (easeType == DGEaseType.None)
            target.DOFade(endValue, duration).SetEase(DGEaseType.GetDGEase(easeType)).onComplete = OnEnd;
        else
            target.DOFade(endValue, duration).SetEase(DGEaseType.GetDGEase(easeType)).SetLoops(-1, LoopType.Yoyo);
    }

    public static void DoImageFade(Image target, float endValue, float duration, int easeType = 0, Action action = null)
    {
        TweenCallback onEnd = () =>
        {
            if (action != null)
                action();
        };

        target.DOFade(endValue, duration).SetEase(DGEaseType.GetDGEase(easeType)).onComplete = onEnd;
    }
    #endregion

    public static void DoRotateCircle(Transform target)
    {
        target.DOLocalRotate(new Vector3(0, 0, -360), 3f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
    }

    public static void DoImageFillAmount(Image target, float endValue, float duration, int easeType = 0)
    {
        target.DOFillAmount(endValue, duration).SetEase(DGEaseType.GetDGEase(easeType));
    }

    public static void DoScale(RectTransform target, Vector3 endValue, float duration, int easeType = 0)
    {
        target.DOScale(endValue, duration).SetEase(DGEaseType.GetDGEase(easeType));
    }

    public static void DoAnchorPos(RectTransform target, Vector2 endValue, float duration, int easeType = 0, Action callBack = null)
    {
        TweenCallback onEnd = () =>
        {
            if (callBack != null)
                callBack();
        };

        target.DOAnchorPos(endValue, duration).SetEase(DGEaseType.GetDGEase(easeType)).onComplete = onEnd;
    }

    public static void DoAnchorPosX(RectTransform target, float endValue, float duration, int easeType = 0, Action action = null)
    {
        TweenCallback onEnd = () =>
        {
            if (action != null)
                action();
        };

        target.DOAnchorPosX(endValue, duration).SetEase(DGEaseType.GetDGEase(easeType)).onComplete = onEnd;
    }

    public static void DoScale(RectTransform target, float endValue, float duration, int easeType = 0, Action callBack = null)
    {
        TweenCallback onEnd = () =>
        {
            if (callBack != null)
                callBack();
        };

        target.DOScale(endValue, duration).SetEase(DGEaseType.GetDGEase(easeType)).onComplete = onEnd;
    }

    public static void DoLocalMoveX(Transform target, float targetX, float duration, int easeType = 0, Action callBack = null)
    {
        TweenCallback onEnd = () =>
        {
            if (callBack != null)
                callBack();
        };

        target.DOLocalMoveX(targetX, duration).SetEase(DGEaseType.GetDGEase(easeType)).onComplete = onEnd;
    }

    public static void DoLocalMoveY(Transform target, float targetY, float duration, int easeType = 0, Action action = null)
    {
        TweenCallback onEnd = () =>
        {
            if (action != null)
                action();
        };

        target.DOLocalMoveY(targetY, duration).SetEase(DGEaseType.GetDGEase(easeType)).onComplete = onEnd;
    }

    public static void DoMove(Transform target, Vector3 endValue, float duration, int easeType = 0, Action action = null)
    {
        TweenCallback onEnd = () =>
        {
            if (action != null)
                action();
        };

        target.DOMove(endValue, duration).SetEase(DGEaseType.GetDGEase(easeType)).onComplete = onEnd;
    }

    public static void DoLocalMove(Transform target, Vector3 endValue, float duration, int easeType = 0, Action action = null)
    {
        TweenCallback onEnd = () =>
        {
            if (action != null)
                action();
        };

        target.DOLocalMove(endValue, duration).SetEase(DGEaseType.GetDGEase(easeType)).onComplete = onEnd;
    }
}