using UnityEngine;
using System.Collections;

public class Player : Mobile
{
	private bool isAxisPressed = false; // Il giocatore sta tenendo premuti i tasti di movimento?
	
	void Update()
	{
		int hor = (int) Input.GetAxisRaw( "Horizontal" );
		int ver = (int) Input.GetAxisRaw( "Vertical" );
		
		if ( hor != 0 || ver != 0 )
		{
			if ( gameController.canMove && !isMoving && !isAxisPressed )
			{
				if ( hor == 0 || ver == 0 ) // Se il giocatore prova a muoversi contemporaneamente su due assi, l'input viene ignorato.
				{
					AttemptMove( hor, ver );
					gameController.canMove = false; // Il giocatore non può muoversi due volte nello stesso beat.
				}
			}
			
			isAxisPressed = true;
		}
		else
			isAxisPressed = false;
	}
}