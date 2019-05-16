using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxSort : MonoBehaviour {

    private int _sortingOrderTop = 80;
    private int _sortingOrderBottm = -20;

    public GameObject[] topObj;
    public GameObject[] bottomObj;
    private void Start()
    {
        Sort(topObj, _sortingOrderTop);
        Sort(bottomObj, _sortingOrderBottm);
    }
    private void Sort(GameObject[] obj,int _sortOrder)
    {
        for (int i = 0; i <= obj.Length; i++)
        {
            obj[i].GetComponent<Renderer>().sortingOrder += _sortOrder;
        }
    }
}
