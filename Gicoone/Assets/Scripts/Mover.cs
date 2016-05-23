using UnityEngine;
using System.Collections;

public class Mover : Turret 
{
	private int hor = 0;
	private int ver = 1;
	void Start ()
	{
		StartCoroutine ( mov () );
	
	}
	private IEnumerator mov()
	{
		yield return new WaitForSeconds(1);
		while (true) 
		{	
			switch (dir)
			{
				case 0:
					hor = 0;
					ver = 1;
				break;
				case 1:
					hor = 1;
					ver = 0;
				break;
				case 2:
					hor = 0;
					ver = -1;
				break;
				case 3:
					hor = -1;
					ver = 0;
				break;

			}
			AttemptMove ( hor, ver );
			yield return new WaitForSeconds (60.0f/80);
		}

	}
}
