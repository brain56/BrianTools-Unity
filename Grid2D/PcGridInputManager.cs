using UnityEngine;
using System.Collections;

public class PcGridInputManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckForGridRaycast ();
	}

	private void CheckForGridRaycast()
	{
		bool mouseUp = Input.GetMouseButtonUp (0);

		RaycastHit hit;
		Camera camera = Camera.main;
		Ray ray = camera.ScreenPointToRay (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 1000));
		Debug.DrawRay (ray.origin, ray.direction*1000);

		if (!mouseUp) {
			return;
		}

		if(Physics.Raycast(ray, out hit))
		{
			Debug.Log ("Hit: " + hit.collider.gameObject.name);
		}
	}
}
