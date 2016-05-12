using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
	private Vector3 start;
	private Vector3 end;
	private LineRenderer laser;
	private ParticleSystem part;
	//private Particle[] particles;

	void Start ()
	{
		part = GetComponent<ParticleSystem> ();
		laser = GetComponent<LineRenderer> ();
		laser.SetWidth (.2f,.2f);
		start = transform.position;
	}
	
	void FixedUpdate () 
	{
		RaycastHit hit;
		bool ok = Physics.Raycast (transform.position, Vector3.forward, out hit, 100);
		if (ok)
		{					
			//Debug.Log (hit.distance);
			part.startLifetime = hit.distance/5.08f;
			end = hit.point;
			//particles = part.GetParticles ();

		}

		laser.SetPosition (0, start);
		laser.SetPosition (1, end);
	}
}
