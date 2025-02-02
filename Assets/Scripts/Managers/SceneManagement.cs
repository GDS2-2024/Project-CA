using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{

    private static SceneManagement instance;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGameModeSettings()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        print("quitting application");
        Application.Quit();
    }

    public void LoadDeathMatch()
    {
        SceneManager.LoadScene(2);
    }
}
