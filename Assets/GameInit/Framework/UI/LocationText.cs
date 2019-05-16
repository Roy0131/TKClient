using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Local_Text", 10)]
public class LocationText :Text
{
    public int IDLanguage;

    public const string LocationTextAwake = "locationTextAwake";
    public const string LocationTextDestroy = "locationTextDestroy";

    protected override void Awake()
    {
        base.Awake();
        if (GameDriver.Instance == null)
            return;
        GameDriver.Instance.mPluginDispather.DispathEvent(LocationTextAwake, this);
    }

    public bool IDInValid()
    {
        return IDLanguage == 0 || IDLanguage == -100;
    }
    
    public override string text
    {
        get
        {
            return base.text;
        }

        set
        {
            base.text = value;
        }
    }

    void OnDestroy()
    {
        if (GameDriver.Instance == null)
            return;
        GameDriver.Instance.mPluginDispather.DispathEvent(LocationTextDestroy, this);
    }
}
