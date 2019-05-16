using UnityEngine;
using System.Collections;

public class AnimationTexNew : MonoBehaviour
{
    public float countX = 2;
    public float countY = 2;
    public float ScrollSpeed = 10;
    
    //public bool loop = true;
    //public float runTime = 1.0f;

    private float offsetX = 0.0f;
    private float offsetY = 0.0f;
    //private float _runDelayTime;
    private float _offsetConstX;
    private float _offsetConstY;
    private float _time = 0;

    //private void OnEnable()
    //{
    //    _runDelayTime = runTime;
    //}
    void OnEnable()
    {
        float x_1 = 1.0f / countX;
        float y_1 = 1.0f / countY;
        float y_2 = (float)1.0 / countY * (countY-1);
        _offsetConstY = 1 - 1 / countY;
        _offsetConstX = 1 / countX;
        this.GetComponent<Renderer>().material.mainTextureScale = new Vector2(x_1, y_1);

        this.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, _offsetConstY));
    }
    void Update()
    {
        _time += Time.deltaTime;
        //_time = Time.time;
        //float frame = Mathf.Floor(_time * ScrollSpeed);
        float frame = Mathf.Floor(_time * ScrollSpeed);

        offsetX = frame / countX;
        //Debug.Log(offsetX+"---------------");
      offsetY = -(frame - frame % countX) / countY / countX;
        this.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY+ _offsetConstY));


        //if (!loop)
        //{
        //    _runDelayTime -= Time.deltaTime;
        //    if (_runDelayTime <= 0.01f)
        //    {
        //        this.gameObject.SetActive(false);
        //    }
        //}
    }
    private void OnDisable()
    {
        _time = 0;
    }
}
