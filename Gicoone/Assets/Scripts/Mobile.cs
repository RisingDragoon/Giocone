using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
public class Mobile : MonoBehaviour
{
    public enum Direction : byte
	{
		Up,
		Down,
		Left,
		Right
	};
	
	protected bool moving;
	// protected bool busy;
	// protected bool paused;

    private float speed;
	
	public Rigidbody rbody;
	// private BoxCollider coll;
	// private Animator anim;
	protected LayerMask blockingLayer;
	
	protected void Start()
	{
		moving = false;
		
		GameController gameController = GameObject.FindGameObjectWithTag( "GameController" ).GetComponent<GameController>();
		speed = gameController.bpm / 6.0f;
		
		rbody = GetComponent<Rigidbody>();
        Debug.Log("Start mobile");
		//coll = GetComponent<BoxCollider>();
		// anim = GetComponent<Animator>();

		blockingLayer = 1 << LayerMask.NameToLayer( "Blocking" );
	}
	
	public bool AttemptMove( Vector3 offset )
	{
		Vector3 startPos = transform.position;
		Vector3 endPos = startPos + offset;

        RaycastHit hit;
		bool isBlocked = Physics.Linecast( startPos, endPos, out hit, blockingLayer );
        
		if ( !isBlocked || transform.CompareTag( "Projectile" ) )
        {
            StartCoroutine( MoveSmoothly( endPos ) );
           
            return true;
        }
        else if ( transform.CompareTag( "Player" ) )
		{
			if ( hit.transform.CompareTag( "Finish" ) )
			{
				Destroy( hit.transform.gameObject );
				Debug.Log( "Il player ha preso il vinile!" );
				return true;
			}
			else if ( hit.transform.CompareTag( "Pushable" ) )
			{
				Mobile pushed = hit.transform.GetComponent<Mobile>();

				if ( pushed != null && pushed.AttemptMove( offset ) )
				{
					StartCoroutine( MoveSmoothly( endPos ) );
					return true;
				}
				else
				{
					return false;
				}
			}
			else if ( hit.transform.CompareTag( "Pickup" ) )
			{
				StartCoroutine( MoveSmoothly( endPos ) );
				Destroy( hit.transform.gameObject );
				gameObject.GetComponent<Player>().GainLife();
				return true;
			}
			else
			{
				return false;
			}
		}
        else
        {
            return false;
        }

		// else
		//	anim.SetInteger( "pace", 0 );
		
		// anim.SetInteger( "hor", hor );
		// anim.SetInteger( "ver", ver );
	}
	
	public bool AttemptMove( Direction dir )
	{
		Vector3 offset = dir.ToVector();
		return AttemptMove( offset );
	}
	
	public void Rotate( Vector3 offset )
	{
		Quaternion rotation = Quaternion.LookRotation( offset );
		StartCoroutine( RotateSmoothly( rotation ) );
	}
	
	public void Rotate( Direction dir )
	{
		Vector3 offset = dir.ToVector();
		Rotate( offset );
	}
	
	private IEnumerator MoveSmoothly( Vector3 endPos )
	{
		moving = true;
		
		GameObject blocker = Instantiate( Resources.Load( "Blocker" ), endPos, Quaternion.identity ) as GameObject;
		// anim.SetInteger( "pace", 1 );
		
		if ( transform.CompareTag( "Projectile" ) )
		{
			Destroy( blocker );
		}
		
		float sqrDistanceLeft = ( transform.position - endPos ).sqrMagnitude;
		
		while ( sqrDistanceLeft > float.Epsilon )
		{
			Vector3 newPos = Vector3.MoveTowards( rbody.position, endPos, speed * Time.deltaTime );
			rbody.MovePosition( newPos );
			sqrDistanceLeft = ( transform.position - endPos ).sqrMagnitude;
			
			yield return null;
		}
		
		Destroy( blocker );
		moving = false;
	}
	
	private IEnumerator RotateSmoothly( Quaternion endRot )
	{
		float angleLeft = Quaternion.Angle( transform.rotation, endRot );
		
		while ( angleLeft > float.Epsilon )
		{
			Quaternion newRot = Quaternion.RotateTowards( rbody.rotation, endRot, speed * 90 * Time.deltaTime );
			rbody.MoveRotation( newRot );
			angleLeft = Quaternion.Angle( transform.rotation, endRot );
			
			yield return null;
		}
	}
}

public static class DirectionExtension
{
    public static Mobile.Direction Invert( this Mobile.Direction dir )
    {
        switch ( dir )
        {
            case Mobile.Direction.Up:
                dir = Mobile.Direction.Down;
                break;
            case Mobile.Direction.Down:
                dir = Mobile.Direction.Up;
                break;
            case Mobile.Direction.Left:
                dir = Mobile.Direction.Right;
                break;
            case Mobile.Direction.Right:
                dir = Mobile.Direction.Left;
                break;
        }
        return dir;
    }
	
	public static Vector3 ToVector( this Mobile.Direction dir )
	{
		Vector3 vec = Vector3.zero;
		
		switch ( dir )
		{
			case Mobile.Direction.Up:
				vec = Vector3.forward;
				break;
			case Mobile.Direction.Down:
				vec = Vector3.back;
				break;
			case Mobile.Direction.Left:
				vec = Vector3.left;
				break;
			case Mobile.Direction.Right:
				vec = Vector3.right;
				break;
		}
		
		return vec;
	}
}