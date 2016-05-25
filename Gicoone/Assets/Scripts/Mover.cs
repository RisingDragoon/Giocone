using UnityEngine;
using System.Collections;

public class Mover : Turret 
{
	void Start ()
	{
		StartCoroutine ( mov () );
	
	}
	private IEnumerator mov()
	{
		yield return new WaitForSeconds(1);
		while (true) 
		{			    
			AttemptMove ( turretDirection );
            //TODO
			yield return new WaitForSeconds (60.0f/80);//si deve muovere a tempo di beat
		}

	}
}
