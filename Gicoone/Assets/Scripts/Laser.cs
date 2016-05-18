using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
	private Vector3 start;
	private Vector3 end;
	private LineRenderer line;
	private ParticleSystem part;
	//private Particle[] particles;

	void Start ()
	{
		part = GetComponent<ParticleSystem> ();
		line = GetComponent<LineRenderer> ();
		line.SetWidth (.2f,.2f);
	}
	
	void FixedUpdate () 
	{
		start = transform.position;
		RaycastHit hit;
		bool ok = Physics.Raycast (transform.position, Vector3.forward,out hit, 100);
		if (ok)
		{					
			//Debug.Log (hit.distance);
			part.startLifetime = hit.distance/5.08f;
			end = hit.point;
			//particles = part.GetParticles ();
		}
		line.SetPosition (0, start);
		line.SetPosition (1, end);
	}
}
