using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuNavigator : MonoBehaviour
{
    public void LoadNewGame()
    {
        SceneManager.LoadScene( "Intro" );
    }
	
	public void QuitGame()
	{
		Application.Quit();
	}
	
    private void LoadNextLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

		if (sceneIndex == 12)
		{
			SceneManager.LoadScene( "MainMenu" );
		}
		else {
			SceneManager.LoadScene (sceneIndex + 1);
		}
    }


}