using UnityEngine;
using Framework.UI;

namespace NewBieGuide
{
    public class GuideUIMgr : Singleton<GuideUIMgr>
    {
        private GuideUIView _uiView;
        public void Show(GuideStepDataVO vo)
        {
            if(_uiView == null)
            {
                _uiView = new GuideUIView();
                GameObject guideObject = GameResMgr.Instance.LoadUIObjectSync(SingletonResName.UINewBieGuide);
                _uiView.SetDisplayObject(guideObject);
                _uiView.mRectTransform.SetParent(GameUIMgr.Instance.mGuideRoot, false);
            }
            _uiView.Show(vo);
        }

        public void Hide()
        {
            MainMapMgr.Instance.Enable = true;
            if (_uiView == null)
                return;
            _uiView.Hide();
        }

        public void Dispose()
        {
            MainMapMgr.Instance.Enable = true;
            if (_uiView != null)
            {
                _uiView.Dispose();
                _uiView = null;
            }
        }
    }

    internal class GuideUIView : UIBaseView
    {
        private MaskGuideView _maskGuideView;
        private AlertGuideView _alertGuideView;

        protected override void ParseComponent()
        {
            base.ParseComponent();

            _maskGuideView = new MaskGuideView();
            _maskGuideView.SetDisplayObject(Find("MaskGuideRoot"));

            _alertGuideView = new AlertGuideView();
            _alertGuideView.SetDisplayObject(Find("AlertGuideRoot"));
        }

        protected override void Refresh(params object[] args)
        {
            base.Refresh(args);
            GuideStepDataVO vo = args[0] as GuideStepDataVO;
            NewBieGuideMgr.Instance.mBlGuideForce = vo.mOpenMainMapDrag == 0;
            if(vo.mGuideType == GuideType.ModuleGuide)
            {
                _maskGuideView.Show(vo);
                _alertGuideView.Hide();
            }
            else
            {
                _alertGuideView.Show(vo);
                _maskGuideView.Hide();
            }
        }

        public override void Dispose()
        {
            if(_maskGuideView != null)
            {
                _maskGuideView.Dispose();
                _maskGuideView = null;
            }
            if(_alertGuideView != null)
            {
                _alertGuideView.Dispose();
                _alertGuideView = null;
            }
            base.Dispose();
        }
    }
}
    
