using UnityEngine;
using System.Collections;

public class Mobile : MonoBehaviour
{
	public enum Direction : byte
	{
		Up,
		Down,
		Left,
		Right
	};

    public bool canRotate;
	
	protected bool isMoving;
	// protected bool busy;
	// protected bool paused;
	
	protected GameController gameController;

    private float speed;
	
	private Rigidbody rbody;
	// private BoxCollider coll;
	// private Animator anim;
	protected LayerMask blockingLayer;
	
	protected void Start()
	{
		isMoving = false;
		
		GameObject gameControllerObj = GameObject.FindGameObjectWithTag( "GameController" );

        if ( gameControllerObj != null )
        {
            gameController = gameControllerObj.GetComponent<GameController>();
        }

        speed = gameController.bpm / 6.0f;
		
		rbody = GetComponent<Rigidbody>();
		// coll = GetComponent<BoxCollider>();
		// anim = GetComponent<Animator>();

		blockingLayer = 1 << LayerMask.NameToLayer( "Blocking" );
	}
	
	public bool AttemptMove( int hor, int ver )
	{
		Vector3 startPos = transform.position;
		Vector3 endPos = startPos + new Vector3( hor, 0.0f, ver );

        RaycastHit hit;
		
		// coll.enabled = false;
		bool isBlocked = Physics.Linecast( startPos, endPos, out hit, blockingLayer );
		// coll.enabled = true;
        
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

				if ( pushed != null && pushed.AttemptMove( hor, ver ) )
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
		int hor, ver;
		
		switch ( dir )
		{
			case Direction.Up:
				hor = 0;
				ver = 1;
				break;
			case Direction.Down:
				hor = 0;
				ver = -1;
				break;
			case Direction.Left:
				hor = -1;
				ver = 0;
				break;
			case Direction.Right:
				hor = 1;
				ver = 0;
				break;
			default:
				hor = 0;
				ver = 0;
				break;
		}
		
		return AttemptMove( hor, ver );
	}
	
	private IEnumerator MoveSmoothly( Vector3 endPos )
	{
		isMoving = true;
		
		// anim.SetInteger( "pace", 1 );
		
		Quaternion endRot = Quaternion.LookRotation( endPos - transform.position );
		
		float sqrDistanceLeft = ( transform.position - endPos ).sqrMagnitude;
		float angleLeft = Quaternion.Angle( transform.rotation, endRot );
		
		while ( sqrDistanceLeft > float.Epsilon || ( canRotate && angleLeft > float.Epsilon ) )
		{
			Vector3 newPos = Vector3.MoveTowards( rbody.position, endPos, speed * Time.deltaTime );
			rbody.MovePosition( newPos );
			sqrDistanceLeft = ( transform.position - endPos ).sqrMagnitude;
			
			if ( canRotate )
			{
				Quaternion newRot = Quaternion.RotateTowards( rbody.rotation, endRot, speed * 90 * Time.deltaTime );
				rbody.MoveRotation( newRot );
				angleLeft = Quaternion.Angle( transform.rotation, endRot );
			}
			
			yield return null;
		}
		
		isMoving = false;
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
}