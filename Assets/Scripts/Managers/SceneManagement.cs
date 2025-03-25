using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{

    private static SceneManagement instance;

    private void Awake()
    {
        if (instance == null)
        {
            // If no instance exists, set this one
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        print("quitting application");
        Application.Quit();
    }

    public void LoadDeathMatch()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadKingOfTheHill()
    {
        SceneManager.LoadScene(2);
    }
}
