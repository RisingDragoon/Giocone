using UnityEngine;
using System.Collections;

[System.Serializable]
public class Couple
{
	public int _hor;
	public int _ver;
	public Couple(int hor, int ver)
	{
		_hor = hor;
		_ver = ver;
	}
}
public class Turret : Mobile 
{
	public Couple[] path = new Couple[1];
	private int i=0;

	void Start()
	{
		StartCoroutine ( Movimento () );
	}

	public void Muoviti()
	{		
		AttemptMove ( path[i]._hor , path[i]._ver );
		i++;
		if (i == path.Length)
		{
			i = 0;
		}
	}

	private IEnumerator Movimento()
	{
		yield return new WaitForSeconds(1);

		while (true) 
		{	
			Muoviti ();
			yield return new WaitForSeconds (1);
		}

	}
}
