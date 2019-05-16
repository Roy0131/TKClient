using UnityEngine;
using Framework.Core;


namespace Framework.UI
{
    public class CameraShake : UpdateBase
    {
        private float shakeTime = 0.0f;
        private float fps = 20.0f;
        private float frameTime = 0.0f;
        private float shakeDelta = 0.005f;
        private bool _blShakeCamera = false;

        public CameraShake()
        {
            Initialize();
            shakeTime = 0.47f;
            fps = 20.0f;
            frameTime = 0.03f;
            shakeDelta = 0.005f;
        }

        public override void Update()
        {
            if (_blShakeCamera)
            {
                if (shakeTime > 0)
                {
                    shakeTime -= Time.deltaTime;
                    if (shakeTime <= 0)
                    {
                        Camera.main.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
                        _blShakeCamera = false;
                        shakeTime = 0.47f;
                        fps = 20.0f;
                        frameTime = 0.03f;
                        shakeDelta = 0.005f;
                    }
                    else
                    {
                        frameTime += Time.deltaTime;

                        if (frameTime > 1.0 / fps)
                        {
                            frameTime = 0;
                            Camera.main.rect = new Rect(shakeDelta * (-1.0f + 4.0f * Random.value), shakeDelta * (-1.0f + 4.0f * Random.value), 1.0f, 1.0f);
                        }
                    }
                }
            }
        }

        public void StartShake()
        {
            _blShakeCamera = true;
        }
    }
}