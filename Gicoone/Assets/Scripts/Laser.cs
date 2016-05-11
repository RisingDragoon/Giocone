using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
	private Transform startPosition;
	private Transform endPosition;
	private LineRenderer laser;

	void Start ()
	{
		laser = GetComponent<LineRenderer> ();
		laser.SetWidth (.2f,.2f);
		startPosition.position = new Vector3(0.0f,0.0f,0.0f);
		//endPosition.position = transform.position;
	}
	
	void FixedUpdate () 
	{
		Vector3 forward = transform.forward;
		RaycastHit hit;
		bool ok = Physics.Raycast (transform.position, Vector3.forward, out hit, 100);
		if (ok)
		{			
			Debug.Log ("Intercetta qualcosa  " + hit.distance.ToString());
			//if (hit.point!=null) 
			//{
				endPosition.position = hit.point;
			//}
		}
		laser.SetPosition (0, startPosition.position);
		laser.SetPosition (1, endPosition.position);
	}
}
