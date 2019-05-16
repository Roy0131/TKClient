using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.UI
{
    public static class ObjectHelper
    {
        public static void SetEnableStatus(Button btn, bool enable)
        {
            btn.interactable = enable;
        }

        public static void SetObjectLayer(Transform transform, int layer)
        {
            transform.gameObject.layer = layer;
            for (int i = 0; i < transform.childCount; i++)
                ObjectHelper.SetObjectLayer(transform.GetChild(i), layer);
        }

        public static void AddChildToParent(Transform child, Transform parent, bool blResetScale = true)
        {
            SetObjectLayer(child, parent.gameObject.layer);
            child.SetParent(parent, false);
            child.localPosition = Vector3.zero;
            if (blResetScale)
                child.localScale = Vector3.one;
        }
        /// <summary>
        /// 线性弹出
        /// </summary>
        /// <param name="transObj"></param>
        public static void PopAnimationLiner(Transform transObj)
        {
            AnimationScale(transObj, Ease.OutQuad, 0.3f);
        }
        /// <summary>
        /// 弹性弹出
        /// </summary>
        /// <param name="transObj"></param>
        public static void PopAnimationBack(Transform transObj)
        {
            AnimationScale(transObj, Ease.OutBack, 0.4f);
        }
        private static void AnimationScale(Transform transObj, Ease ease, float time)
        {

            if (transObj.localScale == Vector3.one)
                transObj.localScale = Vector3.zero;
            transObj.DOScale(Vector3.one, time).SetEase(ease);
        }
        /// <summary>
        /// 弹性移动
        /// </summary>
        /// <param name="transObj"></param>
        /// <param name="vecOri"></param>
        /// <param name="vecDes"></param>
        public static void AnimationMoveBack(Transform transObj, direction direction)
        {
            AnimationMove(transObj, direction, Ease.OutBack, 0.4f, 500);
        }
        /// <summary>
        /// 弹性移动撞击模式
        /// </summary>
        /// <param name="transObj"></param>
        /// <param name="vecOri"></param>
        /// <param name="vecDes"></param>
        public static void AnimationMoveBounce(Transform transObj, direction direction)
        {
            AnimationMove(transObj, direction, Ease.OutBounce, 0.6f, 200);
        }
        /// <summary>
        /// 线性移动
        /// </summary>
        /// <param name="transObj"></param>
        /// <param name="vecOri"></param>
        /// <param name="vecDes"></param>
        public static void AnimationMoveLiner(Transform transObj, direction direction)
        {
            AnimationMove(transObj, direction, Ease.OutQuad, 0.3f, 300);
        }

        public static void AnimationMove(Transform transObj, Vector3 vecOri, Vector3 vecDes, float time)
        {
            if (transObj.localPosition == vecDes)
                transObj.localPosition = vecOri;
            transObj.DOMove(vecDes, time).SetEase(Ease.OutQuad);
        }
        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="transObj"></param>
        /// <param name="isLeft"></param>
        /// <param name="ease"></param>
        public enum direction
        {
            up = 1,
            down = 2,
            left = 3,
            right = 4,
        }

        private static void AnimationMove(Transform transObj, direction direction, Ease ease, float time, float value)
        {
            #region
            //up 1
            //down 2
            //left 3
            //right 4
            #endregion
            Transform trans = transObj;
            Vector3 vec = Vector3.zero;
            switch (direction)
            {
                case direction.up:
                    vec = new Vector3(0f, -value, 0f);
                    break;
                case direction.down:
                    vec = new Vector3(0f, value, 0f);
                    break;
                case direction.left:
                    vec = new Vector3(value, 0f, 0f);
                    break;
                case direction.right:
                    vec = new Vector3(-value, 0f, 0f);
                    break;

            }

            Vector3 vecDes = transObj.localPosition;
            Vector3 vecOri = transObj.localPosition + vec;

            if (transObj.localPosition == vecDes)
                transObj.localPosition = vecOri;

            transObj.DOLocalMove(vecDes, time).SetEase(ease);
        }
        /// <summary>
        /// load资源并设定比例
        /// </summary>
        /// <param name="image"></param>
        /// <param name="sprite"></param>
        public static void SetSprite(Image image, Sprite sprite)
        {
            image.sprite = sprite;
            image.preserveAspect = true;
        }
    }
}