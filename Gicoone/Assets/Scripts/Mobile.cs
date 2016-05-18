using UnityEngine;
using System.Collections;

public class Mobile : MonoBehaviour
{
	public float speed = 10.0f;
	// public GameController gameController;
	
	protected bool isMoving;
	// protected bool busy;
	// protected bool paused;
	
	protected GameController gameController;
	
	private Rigidbody rbody;
	// private BoxCollider coll;
	// private Animator anim;
	private LayerMask blockingLayer;
	
	void Start()
	{
		isMoving = false;
		
		GameObject gameControllerObj = GameObject.FindGameObjectWithTag( "GameController" );
		
		if ( gameControllerObj != null )
			gameController = gameControllerObj.GetComponent<GameController>();
		
		rbody = GetComponent<Rigidbody>();
		// coll = GetComponent<BoxCollider>();
		// anim = GetComponent<Animator>();
		
		blockingLayer = 1 << LayerMask.NameToLayer( "Blocking Layer" );
	}
	
	protected void AttemptMove( int hor, int ver )
	{
		Vector3 startPos = transform.position;
		Vector3 endPos = startPos + new Vector3( hor, 0.0f, ver );
		
		// coll.enabled = false;
		bool hit = Physics.Linecast( startPos, endPos, blockingLayer );
		// coll.enabled = true;
		
		if ( !hit )
			StartCoroutine( MoveSmoothly( endPos ) );
		// else
		//	anim.SetInteger( "pace", 0 );
		
		// anim.SetInteger( "hor", hor );
		// anim.SetInteger( "ver", ver );
	}
	
	private IEnumerator MoveSmoothly( Vector3 endPos )
	{
		isMoving = true;
		
		// anim.SetInteger( "pace", 1 );
		
		float sqrDistanceLeft = (transform.position - endPos).sqrMagnitude;
		
		while ( sqrDistanceLeft > float.Epsilon )
		{
			Vector3 newPos = Vector3.MoveTowards( rbody.position, endPos, speed * Time.deltaTime );
			rbody.MovePosition( newPos );
			sqrDistanceLeft = (transform.position - endPos).sqrMagnitude;
			yield return null;
		}
		
		isMoving = false;
		// TriggerChecks();
	}
	
	/*
	private void TriggerChecks()
	{
		// coll.enabled = false;
		Collider2D step = Physics2D.OverlapPoint( transform.position );
		// coll.enabled = true;

		if ( step != null )
		{
			Debug.Log( "Trigger attivato!" );
		}
	}
	*/
}