using UnityEngine;

public class UIDepth : RComponent
{
    public enum SortObjType
    {
        Canvas,
        Particle,
    }
    public int sortingOrder = 100;
    public string sortingLayerName = "TopLayer";
    public SortObjType sortObjectLayer = SortObjType.Particle;

    private Canvas mCanvas = null;
    private Renderer[] mRenders = null;

    protected override void OnStart()
    {
        base.OnStart();
        Execute();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        Execute();
    }

    void Execute()
    {
        if (sortObjectLayer == SortObjType.Particle)
        {
            if (mRenders == null || mRenders.Length == 0)
            {
                mRenders = GetComponentsInChildren<Renderer>();
                if (mRenders != null)
                {
                    for (int i = 0; i < mRenders.Length; i++)
                    {
                        mRenders[i].sortingLayerName = sortingLayerName;
                        mRenders[i].sortingOrder = sortingOrder;
                    }
                }
            }
        }
        else
        {
            if (mCanvas == null)
            {
                if (GetComponent<Canvas>() != null)
                {
                    mCanvas = GetComponent<Canvas>();
                }
                else
                {
                    mCanvas = gameObject.AddComponent<Canvas>();
                }
                mCanvas.overrideSorting = true;
                mCanvas.sortingLayerName = sortingLayerName;
                mCanvas.sortingOrder = sortingOrder;
            }
            else
            {
                mCanvas.overrideSorting = true;
                mCanvas.sortingLayerName = sortingLayerName;
                mCanvas.sortingOrder = sortingOrder;
            }
        }
    }

    public override void Dispose()
    {
        mCanvas = null;
        mRenders = null;
        base.Dispose();
    }
}
