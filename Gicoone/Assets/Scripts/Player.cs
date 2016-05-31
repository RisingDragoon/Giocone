using UnityEngine;
using System.Collections;

public class Player : Mobile
{
    public int maxLives;

	private bool isAxisPressed = false; // Il giocatore sta tenendo premuti i tasti di movimento?
    private int lives;
	
    new void Start()
    {
        base.Start();

        lives = maxLives;
    }

	void Update()
	{
		int hor = (int) Input.GetAxisRaw( "Horizontal" );
		int ver = (int) Input.GetAxisRaw( "Vertical" );
		
		if ( hor != 0 || ver != 0 )
		{
            if ( !isAxisPressed )
            {
			    if ( gameController.canMove && !isMoving )
			    {
				    if ( hor == 0 || ver == 0 ) // Se il giocatore prova a muoversi contemporaneamente su due assi, l'input viene ignorato.
				    {
						Vector3 vec = new Vector3( hor, 0.0f, ver );
						Rotate( vec );
				    	AttemptMove( vec );
				    	gameController.canMove = false; // Il giocatore non può muoversi due volte nello stesso beat.
				    }
			    }
                else
                {
                    LoseLife();
                }
            }
			
			isAxisPressed = true;
		}
		else
        {
			isAxisPressed = false;
        }
	}

	public void GainLife()
	{
		if ( lives < maxLives )
		{
			lives++;
		}

		Debug.Log( "Il player ha ottenuto una vita. Ora ne ha " + lives + "." );
	}

    private void LoseLife()
    {
        lives--;
        Debug.Log( "Il player ha perso una vita. Ne rimangono " + lives + "." );
		
		if ( lives == 0 )
		{
			// Gestire il game over.
			Debug.Log( "Il player è morto!" );
		}
		
        // Gestire la GUI.
    }
}