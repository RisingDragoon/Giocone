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
public class Turret : MonoBehaviour //Mettere Mobile come classe padre
{
	public Couple[] path = new Couple[5];

	void Start ()
	{
		
	}
	
	void Update () 
	{
	
	}
}
