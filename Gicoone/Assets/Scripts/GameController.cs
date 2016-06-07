using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public float bpm;
	
	// Queste variabili servono ad interfacciarsi col cubo verde/rosso di debug.
    public MeshRenderer mesh_debug;
    public Material inactive_debug;
    public Material active_debug;
	
    private float beat;
    private float tolerance;
	private Player player;
    private List<Turret> enemies;
	
    private AudioSource audioSource;
	
	void Start()
    {
        beat = 60.0f / bpm;
        tolerance = beat * 0.5f;
		
		GameObject playerObj = GameObject.FindGameObjectWithTag( "Player" );
		
		if ( playerObj != null )
        {
            player = playerObj.GetComponent<Player>();
        }
		
		enemies = new List<Turret>();
		GameObject[] enemiesObj = GameObject.FindGameObjectsWithTag( "Enemy" );
		
		foreach ( GameObject obj in enemiesObj )
		{
			Turret enemy = obj.GetComponent<Turret>();
			enemies.Add( enemy );
		}
		
        audioSource = GetComponent<AudioSource>();
		
        StartCoroutine( PlayBeat() );
	}
	
	void Update()
    {
		// Interfaccia col cubo di debug.
        if ( player.canMove )
            mesh_debug.material = active_debug;
        else
            mesh_debug.material = inactive_debug;
	}
	
    private IEnumerator PlayBeat()
    {
        float halfTolerance = tolerance / 2;
        audioSource.Play(); // Musica.
        //audioSource.loop = true;
        yield return new WaitForSeconds( beat - halfTolerance );
		
        while ( true )
        {
            player.canMove = true;
			
			if ( player.inStealth )
			{
				StartCoroutine( player.StealthSegment( tolerance ) );
			}
			
            yield return new WaitForSeconds( halfTolerance );

            foreach ( Turret enemy in enemies )
			{
                if ( enemy != null  )//volendo da mettere se active==true
                {
                    enemy.ExecuteAction();
                }                
			}
			
            yield return new WaitForSeconds( halfTolerance );			
            player.canMove = false;
            yield return new WaitForSeconds( beat - tolerance );
        }
    }
}