using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageGray : MonoBehaviour{

    private bool _blGray = false;
    public void SetGray()
    {
        if (_blGray)
            return;
        this.GetComponent<Image>().material = Resources.Load("mat/Gray", typeof(Material)) as Material;
        _blGray = true;
        //Debuger.Log("-----------------设置成灰色");
    }

    public void SetNormal()
    {
        if (!_blGray)
            return;
        this.GetComponent<Image>().material = null;
        _blGray = false;
        //Debuger.Log("-----------------设置成正常颜色");
    }
    public void SetGrayClip()
    {
        if (_blGray)
            return;
        this.GetComponent<Image>().material = Resources.Load("mat/grayClip", typeof(Material)) as Material;
        _blGray = true;
    }
}
