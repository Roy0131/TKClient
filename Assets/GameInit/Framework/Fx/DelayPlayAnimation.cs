using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Animation))]
public class DelayPlayAnimation : MonoBehaviour {



	public AnimationClip anim;
	public float delay;
	float startTime;
	float endTime;
	bool  ready = false;


	//tian jia dao cai dan
	const string menuTitle = "Utility/DelayPlayAnimation";
    #if UNITY_EDITOR
	[UnityEditor.MenuItem(menuTitle)]
	static void AddComp()
	{
		GameObject[] objSels = Selection.gameObjects;
		Debug.Log("objSels:" + objSels.Length);
		for (int i = 0; i < objSels.Length; ++i)
		{
			GameObject objSelect = objSels[i];
			DelayPlayAnimation info = objSelect.GetComponent<DelayPlayAnimation>();
			if (info == null)
			{
				info = objSelect.AddComponent<DelayPlayAnimation>();
			}
			Debug.Log("objSelect:" + objSelect.name);
		}
	}
#endif


	// Use this for initialization
	void Start () {

	}

	void OnEnable() 
	{
		startTime = Time.time + delay;
        if (anim != null)
        {
            endTime = startTime + anim.length;
        }
        else
        {
            endTime += 1.0f;
        }
		ready = true;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Time.time > endTime)
        {
            //Destroy(this);
        }
        if (Time.time < startTime)
            return;
        if (ready)
        {
            if (anim != null)
            {
                GetComponent<Animation>().Stop();
                GetComponent<Animation>().Play(anim.name);
            }
            ready = false;
        }

    }
	
	#if UNITY_EDITOR
	//tian jia dao cai dan
	[UnityEditor.MenuItem(menuTitle, true)]
	static bool ValidateCreate()
	{
		return Selection.activeGameObject != null;
	}
#endif

}
