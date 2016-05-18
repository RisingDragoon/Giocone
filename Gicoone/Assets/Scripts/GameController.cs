using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public float bpm;
    public float tolerance;
	
	[HideInInspector]
	public bool canMove;
	
	// Queste variabili servono ad interfacciarsi col cubo verde/rosso di debug.
    public MeshRenderer cubeMesh;
    public Material red;
    public Material green;
	
    private float beat;
	
    private AudioSource audioSource;
	
	void Start()
    {
        canMove = false;
        beat = 60.0f / bpm;
		
        audioSource = GetComponent<AudioSource>();
		
        StartCoroutine( PlayBeat() );
	}
	
	// Questa funzione serve ad interfacciarsi col cubo verde/rosso di debug.
	void Update()
    {
        if ( canMove )
            cubeMesh.material = green;
        else
            cubeMesh.material = red;
		
        if ( Input.GetButtonDown( "Stealth" ) )
        {
            if ( canMove )
                Debug.Log( "OK!" );
            else
                Debug.Log( "Vaffanculo." );
        }
	}
	
    private IEnumerator PlayBeat()
    {
        yield return new WaitForSeconds( beat - tolerance / 2 );
		
        while ( true )
        {
            canMove = true;
            audioSource.Play(); // Placeholder per la musica.
            yield return new WaitForSeconds( tolerance );
			
            canMove = false;
            yield return new WaitForSeconds( beat - tolerance );
        }
    }
}