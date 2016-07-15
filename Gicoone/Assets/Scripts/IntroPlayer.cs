using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroPlayer : MonoBehaviour
{
	private const float fadingTime = 1.0f;
	
	public Button button;
	public Image scene;
	public Sprite[] sceneGraphics = new Sprite[8];
	
	private int sceneStep;
	
	void Start()
    {
		sceneStep = 0;
	}
	
	void Update()
	{
		if ( Input.GetButtonDown( "Submit" ) )
        {
            LoadNextScene();
        }
	}
	
    public void LoadNextScene()
    {
		sceneStep++;
		
		if ( sceneStep >= 8 )
			SceneManager.LoadScene( "Lv1" );
		else
			StartCoroutine( SceneTransition() );
    }
	
	private IEnumerator SceneTransition()
	{
		button.interactable = false;
		
		for ( float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadingTime )
		{
			Color newColor = Color.Lerp( Color.white, Color.clear, t );
			scene.color = newColor;
			yield return null;
		}
		
		scene.sprite = sceneGraphics[sceneStep];
		
		for ( float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadingTime )
		{
			Color newColor = Color.Lerp( Color.clear, Color.white, t );
			scene.color = newColor;
			yield return null;
		}
		
		button.interactable = true;
	}
}