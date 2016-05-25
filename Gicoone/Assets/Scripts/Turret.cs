using UnityEngine;
using System.Collections;

public class Turret : Mobile 
{

	protected Direction[] path = new Direction[1];
	protected Direction[] pathInverse = new Direction[1];
    protected Direction turretDirection = Direction.Right;

	public string turretType = "ferma";//ferma o mobile
	public GameObject circle;//ciò che deve sparare la torretta ferma
    public float bpm;

    protected float beat;

    
	private float doubleBeat;
	private int indexPath = 0;
    private bool inverse=false;
    private bool temp=false;

	void Start()
	{
		beat = 60.0f / bpm;
		doubleBeat = beat + beat;
        Inverti();
		if ( turretType == "mobile" )
		{
			StartCoroutine ( Movimento () );
		}
		else if ( turretType == "stop" )
		{
			StartCoroutine ( Spara () );
		}        
	}

	public void Muoviti()
	{
        if (!inverse)
        {
            //normale
            temp = AttemptMove(path[indexPath]);
        }
        else
        {
            //contrario
            temp = AttemptMove(pathInverse[indexPath]);
        }
        if (temp == inverse )
        {
            inverse = true;
        }
        else
        {
            inverse = false;
        }

        #region Rotazione
        Vector3 rot = new Vector3(0,0,0);
		if ( path[indexPath] == Direction.Up )
		{	
			if ( turretDirection == Direction.Right ) 
			{				
				rot.Set(0, -90,0);
			} 
			else if ( turretDirection == Direction.Down ) 
			{
				rot.Set(0, -180,0);
			}
			else if ( turretDirection == Direction.Left )
			{
				rot.Set(0,+90,0);
			}
			turretDirection = Direction.Up;
		}
		else if ( path[indexPath] == Direction.Right )
		{
			if ( turretDirection == Direction.Up ) 
			{
				rot.Set(0, +90,0);
			} 
			else if ( turretDirection == Direction.Down )
			{
				rot.Set(0, -90,0);
			}
			else if ( turretDirection == Direction.Left )
			{
				rot.Set(0,-180,0);
			}
			turretDirection = Direction.Right;
		}
		else if ( path[indexPath] == Direction.Down )
		{
			if ( turretDirection == Direction.Up ) 
			{
				rot.Set(0, +180,0);
			} 
			else if ( turretDirection == Direction.Right )
			{
				rot.Set(0, +90,0);
			}
			else if ( turretDirection == Direction.Left )
			{
				rot.Set(0,-90,0);
			}
			turretDirection = Direction.Down;
		}
		else if ( path[indexPath] == Direction.Left )
		{
			if ( turretDirection == Direction.Up ) 
			{
				rot.Set(0, -90,0);
			} 
			else if ( turretDirection == Direction.Right )
			{
				rot.Set(0, +180,0);
			}
			else if ( turretDirection == Direction.Down)
			{
				rot.Set(0,+90,0);
			}
			turretDirection = Direction.Left;
		}
		transform.Rotate (rot);
        #endregion

        if (!inverse)//normale
        {
            indexPath++;
        }
        else//inverso
        {
            indexPath--;
        }
        if ( indexPath == path.Length )//arriva in fondo normale
		{
			indexPath = 0;
		}
        if (inverse && indexPath == 0)//arriva in fondo inverso
        {
            indexPath = path.Length;
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
	private IEnumerator Spara()
	{
		yield return new WaitForSeconds(1);
		while (true) 
		{	
			Instantiate (circle,transform.position,Quaternion.identity);
			yield return new WaitForSeconds (doubleBeat);
		}

	}

    private void Inverti()
    {
        int index = path.Length;
        for (int i = 0; i < path.Length; i++)
        {
            pathInverse[index] = path[i];
            index--;
        }
        for (int i = 0; i < pathInverse.Length; i++)
        {
            if (pathInverse[i] == Direction.Up)
            {
                pathInverse[i] = Direction.Down;
            }
            else if (pathInverse[i] == Direction.Right)
            {
                pathInverse[i] = Direction.Left;
            }
            else if (pathInverse[i] == Direction.Down)
            {
                pathInverse[i] = Direction.Up;
            }
            else
            {
                pathInverse[i] = Direction.Right;
            }
        }
        
    }
}
