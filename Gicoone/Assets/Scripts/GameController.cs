using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public float bpm;
	public int notesToPick;
	public int livesToKeep;
	public float timeToBeat;
	
	[HideInInspector]
    public float beat;

    // Queste variabili servono ad interfacciarsi col cubo verde/rosso di debug.
    public MeshRenderer mesh_debug;
    public Material inactive_debug;
    public Material active_debug;

	private bool beatPlaying;
    private float tolerance;
	private float startTime;
	
    private Player player;
    private Boss boss;
    private List<Turret> enemies;
	private Transform rhythmUI;
	private List<GameObject> beatTacks;

    private AudioSource audioSource;

    void Start()
    {
        beat = 60.0f / bpm;

        beatPlaying = false;
        tolerance = beat * 0.5f;
		
		player = GameObject.FindGameObjectWithTag( "Player" ).GetComponent<Player>();

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
		
		rhythmUI = GameObject.Find( "Canvas/RhythmBar" ).transform;
		beatTacks = new List<GameObject>();
		
        audioSource = GetComponent<AudioSource>();

        float frequency = bpm / 60.0f;
        
        GameObject[] pulsers = GameObject.FindGameObjectsWithTag("Pulser");

        foreach ( GameObject obj in pulsers )
        {
            obj.GetComponent<Animator>().SetFloat("bpm", frequency);
        }

        startTime = Time.time;
        audioSource.Play(); // Musica.
    }

    void Update()
    {
        if ( Input.GetButtonDown( "Restart" ) )
        {
            SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
        }
		
		float currentTime = ( startTime + Time.time ) % beat;
		
		if ( !beatPlaying && currentTime + tolerance >= beat )
		{
			StartCoroutine( PlayBeat() );
			
			GameObject tack = Instantiate( Resources.Load( "BeatTack" ) ) as GameObject;
			tack.transform.SetParent( rhythmUI, false );
			tack.GetComponent<Animator>().SetFloat( "bpm", bpm );
			beatTacks.Add( tack );
			
			if ( beatTacks.Count > 4 )
			{
				Destroy( beatTacks[0] );
				beatTacks.RemoveAt( 0 );
			}
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
            player.StartStealthSegment();
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
	
	public void CatchBeatTack()
	{
		int i = beatTacks.Count - 3;
		
		if ( i < 0 )
			return; // Non ci sono ancora abbastanza tacks.
		
		Destroy( beatTacks[i] );
		beatTacks.RemoveAt( i );
	}
}