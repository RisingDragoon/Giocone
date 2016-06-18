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

	private bool beatPlaying;
    private float beat;
    private float tolerance;
	private float startTime;
    private Player player;
    private Boss boss;
    private List<Turret> enemies;

    private AudioSource audioSource;

    void Start()
    {
		beatPlaying = false;
        beat = 60.0f / bpm;
        tolerance = beat * 0.5f;

        GameObject playerObj = GameObject.FindGameObjectWithTag( "Player" );

        if ( playerObj != null )
        {
            player = playerObj.GetComponent<Player>();
        }

        GameObject bossObj = GameObject.FindGameObjectWithTag( "Boss" );

        if ( bossObj != null )
        {
            boss = bossObj.GetComponent<Boss>();
        }

        enemies = new List<Turret>();
        GameObject[] enemiesObj = GameObject.FindGameObjectsWithTag( "Enemy" );

        foreach ( GameObject obj in enemiesObj )
        {
            Turret enemy = obj.GetComponent<Turret>();
            enemies.Add( enemy );
        }

        audioSource = GetComponent<AudioSource>();
		
		startTime = Time.time;
        audioSource.Play(); // Musica.
    }

    void Update()
    {
		float currentTime = ( startTime + Time.time ) % beat;
		
		if ( !beatPlaying && currentTime + tolerance >= beat )
		{
			StartCoroutine( PlayBeat() );
		}
		
        // Interfaccia col cubo di debug.
        if ( player.canMove )
            mesh_debug.material = active_debug;
        else
            mesh_debug.material = inactive_debug;
    }

    private IEnumerator PlayBeat()
    {
        float halfTolerance = tolerance / 2;

		beatPlaying = true;
		player.canMove = true;
		
        if ( player.inStealth )
        {
            StartCoroutine( player.StealthSegment( tolerance ) );
        }
		
		yield return new WaitForSeconds( halfTolerance );
		
		if ( boss != null )
		{
			boss.ExecuteAction();
		}
		
		foreach ( Turret enemy in enemies )
		{
			if ( enemy != null ) // volendo da mettere se active==true
			{
				enemy.ExecuteAction();
			}
		}
		
		yield return new WaitForSeconds( halfTolerance );
		
		player.canMove = false;
		beatPlaying = false;
    }
}