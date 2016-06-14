using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : Mobile
{
    public int maxLives;
	
	public Text livesText;
	public Text stealthText;
	
	[HideInInspector]
	public bool canMove;
	[HideInInspector]
	public bool inStealth;
	
	// Queste variabili servono al debug dello stealth.
    public MeshRenderer mesh_debug;
    public Material inactive_debug;
    public Material active_debug;
	
	private bool axisPressed; // Il giocatore sta tenendo premuti i tasti di movimento?
    private int lives;
	
    new void Start()
    {
        base.Start();
		
        canMove = false;
		inStealth = false;

		axisPressed = false;
        lives = maxLives;
		
		UpdateLivesUI();
		
		//stealthText.text = null;
    }

	void Update()
	{
		if ( Input.GetButtonDown( "Stealth" ) && canMove )
		{
			inStealth = true;
			canMove = false;
		}
		else if ( Input.GetButtonUp( "Stealth" ) )
		{
			inStealth = false;
		}
		
		// Interfaccia con il debug dello stealth.
		if ( inStealth )
			mesh_debug.material = active_debug;
		else
			mesh_debug.material = inactive_debug;
		
		if ( !inStealth )
		{
			int hor = (int) Input.GetAxisRaw( "Horizontal" );
			int ver = (int) Input.GetAxisRaw( "Vertical" );
			
			if ( hor != 0 || ver != 0 )
			{
				if ( !axisPressed )
				{
					if ( canMove && !moving )
					{
						if ( hor == 0 || ver == 0 ) // Se il giocatore prova a muoversi contemporaneamente su due assi, l'input viene ignorato.
						{
							Vector3 vec = new Vector3( hor, 0.0f, ver );
							Rotate( vec );
							AttemptMove( vec );
							canMove = false; // Il giocatore non può muoversi due volte nello stesso beat.
						}
					}
					else
					{
						LoseLife();
					}
				}
				
				axisPressed = true;
			}
			else
			{
				axisPressed = false;
			}
		}
	}

	public void GainLife()
	{
		if ( lives < maxLives )
		{
			lives++;
		}
		
		UpdateLivesUI();
	}

    public void LoseLife()
    {
        lives--;
		
		if ( lives == 0 )
		{
			// Gestire il game over.
			Debug.Log( "Il player è morto!" );
		}
		
        UpdateLivesUI();
    }
	
	private void UpdateLivesUI()
	{
		//livesText.text = "Vite: " + lives;
	}
	
	public IEnumerator StealthSegment( float tolerance )
	{
		bool safeSegment = false;
		float startTime = Time.time;
		Direction toPress = (Direction) Random.Range( 0, 4 );
		
		// Debug.
		string arrowSymbol = "?";
		
		switch ( toPress )
		{
			case Direction.Up:
                arrowSymbol = "\u2191";
                break;
            case Direction.Down:
                arrowSymbol = "\u2193";
                break;
            case Direction.Left:
                arrowSymbol = "\u2190";
                break;
            case Direction.Right:
                arrowSymbol = "\u2192";
                break;
		}
		
		stealthText.text = "Devi premere " + arrowSymbol;
		// Fine Debug.
		
		while ( true )
		{
			int hor = (int) Input.GetAxisRaw( "Horizontal" );
			int ver = (int) Input.GetAxisRaw( "Vertical" );
			
			if ( hor != 0 || ver != 0 )
			{
				if ( !axisPressed )
				{
					if ( canMove )
					{
						Vector3 vecToPress = toPress.ToVector();
						
						if ( hor == vecToPress.x && ver == vecToPress.z )
						{
							Debug.Log( "Input corretto." );
							safeSegment = true;
							
							stealthText.text = null; // Debug.
						}
						else
						{
							Debug.Log( "Input errato." );
							inStealth = false;
						}
						
						canMove = false;
					}
					else
					{
						Debug.Log( "Button mashing." );
						inStealth = false;
					}
				}
				
				axisPressed = true;
			}
			else
			{
				axisPressed = false;
			}
			
			if ( startTime + tolerance <= Time.time )
			{
				if ( !safeSegment )
				{
					Debug.Log( "Nessun input o input errato, termino lo stealth." );
					inStealth = false;
					
					stealthText.text = null; // Debug.
				}
				
				break;
			}
			
			yield return null;
		}
	}
}