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
				    	AttemptMove( hor, ver );
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

    private void LoseLife()
    {
        lives--;
        Debug.Log( "Hai perso una vita, scemo! Te ne restano " + lives );
		
		if ( lives == 0 )
		{
			Debug.Log( "Sei morto!" );
			// Gestire il game over.
		}
		
        // Gestire la GUI.
    }
}