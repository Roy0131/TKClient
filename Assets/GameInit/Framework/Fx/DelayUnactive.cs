using UnityEngine;

public class DelayUnactive : MonoBehaviour {

 
    private float _runDelayTime;
    public float runTime = 1.0f;
    private void OnEnable()
    {
        _runDelayTime = runTime;
    }

    void Update()
    {
        _runDelayTime -= Time.deltaTime;
        if (_runDelayTime <= 0.01f)
        {
            //this.gameObject.SetActive(false);
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    private void OnDisable()
    {
        //this.gameObject.SetActive(true);
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
}
