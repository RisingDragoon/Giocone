using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public float bpm;
    public float tolerance;

    public MeshRenderer cubeMesh;
    public Material red;
    public Material green;

    private bool canPress;
    private float beat;

    private AudioSource audioSource;

	void Start ()
    {
        canPress = false;
        beat = 60.0f / bpm;

        audioSource = GetComponent<AudioSource>();

        StartCoroutine(PlayBeat());
	}
	
	void Update ()
    {
        if (canPress)
            cubeMesh.material = green;
        else
            cubeMesh.material = red;

        if (Input.GetButtonDown("Jump"))
        {
            if (canPress)
                Debug.Log("OK!");
            else
                Debug.Log("Vaffanculo.");
        }
	}

    private IEnumerator PlayBeat()
    {
        yield return new WaitForSeconds( beat - tolerance / 2 );

        while (true)
        {
            canPress = true;
            audioSource.Play(); // Placeholder per la musica.
            yield return new WaitForSeconds( tolerance );

            canPress = false;
            yield return new WaitForSeconds( beat - tolerance );
        }
    }
}