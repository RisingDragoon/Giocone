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
	public float bpm;
	protected float beat;
	private int i = 0;
	private float doubleBeat;
	protected int dir = 1;
	public string tipoTorretta;//ferma o mobile
	public GameObject cerchio;//ciò che deve sparare la torretta ferma


	void Start()
	{
		beat = 60.0f / bpm;
		doubleBeat = beat + beat;
		if (tipoTorretta=="mobile")
		{
			StartCoroutine ( Movimento () );
		}
		else if (tipoTorretta=="ferma")
		{
			StartCoroutine ( Spara (dir) );
		}
	}

	public void Muoviti()
	{		
		AttemptMove ( path[i]._hor , path[i]._ver );
		//float vec = transform.rotation.y;
		Vector3 rot = new Vector3(0,0,0);

		if ( path[i]._hor == 0 && path[i]._ver == 1 )//su
		{	
			if (dir == 1) 
			{				
				rot.Set(0, -90,0);
			} 
			else if (dir == 2)
			{
				rot.Set(0, -180,0);
			}
			else if (dir==3)
			{
				rot.Set(0,+90,0);
			}
			dir = 0;
		}
		else if ( path[i]._hor == 1 && path[i]._ver == 0)//destra
		{
			if (dir == 0) 
			{
				rot.Set(0, +90,0);
			} 
			else if (dir == 2)
			{
				rot.Set(0, -90,0);
			}
			else if (dir==3)
			{
				rot.Set(0,-180,0);
			}
			dir = 1;
		}
		else if ( path[i]._hor == 0 && path[i]._ver == -1 )//giu
		{
			if (dir == 0) 
			{
				rot.Set(0, +180,0);
			} 
			else if (dir == 1)
			{
				rot.Set(0, +90,0);
			}
			else if (dir==3)
			{
				rot.Set(0,-90,0);
			}
			dir = 2;
		}
		else if ( path[i]._hor ==-1 && path[i]._ver == 0)//sinistra
		{
			if (dir == 0) 
			{
				rot.Set(0, -90,0);
			} 
			else if (dir == 1)
			{
				rot.Set(0, +180,0);
			}
			else if (dir==2)
			{
				rot.Set(0,+90,0);
			}
			dir = 3;
		}
		transform.Rotate (rot);
		i++;
		if (i == path.Length)
		{
			i = 0;
		}
	}

	private IEnumerator Movimento()
	{
		yield return new WaitForSeconds(beat/2);
		while (true) 
		{	
			Muoviti ();
			yield return new WaitForSeconds (beat);
		}

	}
	private IEnumerator Spara(int direzione)
	{
		yield return new WaitForSeconds(1);
		while (true) 
		{	
			Instantiate (cerchio,transform.position,Quaternion.identity);
			yield return new WaitForSeconds (doubleBeat);
		}

	}
}
