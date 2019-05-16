using UnityEngine;
using Framework.Core;

namespace Framework.UI
{
    public class FighterMaterialEffect : UpdateBase
    {
        private MeshRenderer _mainRender;
        private Material _mainMat;

        public FighterMaterialEffect(Transform transform)
        {
            _mainRender = transform.GetComponent<MeshRenderer>();
            _mainMat = _mainRender.material;

            _beHitMat = new Material(Shader.Find("Spine/Skeleton PMA Screen"));
            _beHitMat.mainTexture = _mainMat.mainTexture;

            _grayMat = new Material(Shader.Find("IHGame/RoleGray"));
            _grayMat.mainTexture = _mainMat.mainTexture;
            Initialize();
        }

        #region do damage effect

        private Material _beHitMat;
        private float _defValue = 128f;

        private float _r = 218f;
        private float _g = 43f;
        private float _b = 43f;

        private float _dr, _dg, _db;
        private bool _blRun = false;
        private float _time = 0.1f;

        public void ResetBeHitEffect()
        {
            _blRun = false;
            _mainRender.material = _mainMat;
        }

        public void StartEffect()
        {
            if (_blRun || _mainRender == null)
                return;
            _mainRender.material = _beHitMat;
            _beHitMat.SetColor("_Color", new Color(128f / 255f, 128f / 255f, 128f / 255f, 1f));
            _time = 0.1f;

            _dr = (_r - _defValue) / _time;
            _dg = (_g - _defValue) / _time;
            _db = (_b - _defValue) / _time;

            _tr = _defValue;
            _tg = _defValue;
            _tb = _defValue;
            _blToRed = true;
            _blRun = true;
        }

        private float _tr;
        private float _tg;
        private float _tb;
        private bool _blToRed = false;

        public override void Update()
        {
            if (!_blRun)
                return;
            _tr += Time.deltaTime * _dr;
            _tg += Time.deltaTime * _dg;
            _tb += Time.deltaTime * _db;

            _time -= Time.deltaTime;
            _mainRender.material.SetColor("_Color", new Color(_tr / 255f, _tg / 255f, _tb / 255f, 1f));
            if (_time <= 0.01f)
            {
                if (_blToRed)
                {
                    _dr *= -1f;
                    _dg *= -1f;
                    _db *= -1f;
                    _time = 0.1f;
                    _blToRed = false;
                }
                else
                {
                    _blRun = false;
                    _blToRed = true;
                    _mainRender.material = _mainMat;
                }
            }
        }
        #endregion

        #region Gray Effect
        private Material _grayMat = null;
        public void SetGrayEffect()
        {
            _blRun = false;
            _blToRed = true;
            _mainRender.material = _grayMat;
        }

        public void SetNormal()
        {
            _mainRender.material = _mainMat;
        }
        #endregion

        public override void Dispose()
        {
            if (_mainRender != null)
            {
                _mainRender.material = _mainMat;
                _mainMat = null;
                _mainRender = null;
            }
            _grayMat = null;
            _beHitMat = null;
            base.Dispose();
        }
    }
}