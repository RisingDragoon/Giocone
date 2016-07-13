using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuNavigator : MonoBehaviour
{
    public void LoadNewGame()
    {
        SceneManager.LoadScene( "Lv1" );
    }
	
	public void QuitGame()
	{
		Application.Quit();
	}
}