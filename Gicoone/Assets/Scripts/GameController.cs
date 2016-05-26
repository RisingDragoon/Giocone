using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	private List<Turret> enemies;
	
    private AudioSource audioSource;
	
	void Start()
    {
        canMove = false;
        beat = 60.0f / bpm;
		enemies = new List<Turret>();
		
		GameObject[] enemiesObj = GameObject.FindGameObjectsWithTag("Enemy");
		
		foreach ( GameObject obj in enemiesObj )
		{
			Turret script = obj.GetComponent<Turret>();
			enemies.Add( script );
		}
		
        audioSource = GetComponent<AudioSource>();
		
        StartCoroutine( PlayBeat() );
	}
	
	// Questa funzione serve ad interfacciarsi col cubo verde/rosso di debug.
	void Update()
    {
        if ( canMove )
		{
            cubeMesh.material = green;
		}
        else
		{
            cubeMesh.material = red;
		}
		
        if ( Input.GetButtonDown( "Stealth" ) )
        {
            if ( canMove )
            {
                Debug.Log("OK!");
            }
                
            else
            {
                Debug.Log("Vaffanculo.");
            }
                
        }
	}
	
    private IEnumerator PlayBeat()
    {
        float halfTolerance = tolerance / 2;

        yield return new WaitForSeconds( beat - halfTolerance );
		
        while ( true )
        {
            canMove = true;
            audioSource.Play(); // Placeholder per la musica.
            yield return new WaitForSeconds( halfTolerance );

            foreach ( Turret enemy in enemies )
			{
                if ( enemy.active )
                {
                    enemy.ExecuteAction();
                }                
			}
			
            yield return new WaitForSeconds( halfTolerance );			
            canMove = false;
            yield return new WaitForSeconds( beat - tolerance );
        }
    }
}