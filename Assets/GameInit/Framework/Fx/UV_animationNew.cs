using UnityEngine;
using System.Collections;

public class UV_animationNew : MonoBehaviour
{
    public float SpeedX;
    public float SpeedY;
    public float Direction;

    public bool isActive = false;
    public float runTime = 1.0f;



    private Material material;
    private float deltX;
    private float deltY;

    private float _runDelayTime;

    private void OnEnable()
    {
        _runDelayTime = runTime;
    }


    void Start()
    {
        material = GetComponent<Renderer>().material;
     

    }

    void Update()
    {
        if (material)
        {

            deltX += SpeedX * Time.deltaTime * Direction;
            deltY += SpeedY * Time.deltaTime * Direction;
            material.SetTextureOffset("_MainTex", new Vector2(deltX, deltY));

        }
        if (isActive)
        {
            _runDelayTime -= Time.deltaTime;
            if (_runDelayTime <= 0.01f)
            {
                this.gameObject.SetActive(false);
            }
        }

    }

}
