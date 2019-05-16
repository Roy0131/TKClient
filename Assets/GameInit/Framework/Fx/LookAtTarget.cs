using UnityEngine;
public class LookAtTarget : MonoBehaviour
{
	//public Vector3 vec3;
    //public Transform _target;
	private Vector3 _vecUse;
    void Update()
    {
		transform.LookAt(Camera.main.transform.position);
		_vecUse = new Vector3 (-1, 1, 1);

		transform.localScale =new Vector3(_vecUse.x*transform.localScale.x,_vecUse.y*transform.localScale.y,_vecUse.z*transform.localScale.z);
    }
}
