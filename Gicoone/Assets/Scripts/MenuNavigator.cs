using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuNavigator : MonoBehaviour
{
    public void LoadNewGame()
    {
        SceneManager.LoadScene( 2 );
    }
}