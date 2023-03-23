using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    [SerializeField] GameObject creditsUI;
    [SerializeField] GameObject tutorialUI;
    public void CloseGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenCredits()
    {
        creditsUI.SetActive(true);
    }

    public void OpenTutorial()
    {
        tutorialUI.SetActive(true);
    }
    
}
