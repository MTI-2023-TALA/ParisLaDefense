using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Button playButton;
    public Button optionsButon;
    public Button quitButton;

    private void Start()
    {
        playButton.onClick.AddListener(Play);
        optionsButon.onClick.AddListener(Options);
        quitButton.onClick.AddListener(Quit);
    }

    private void Play()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void Options()
    {
        SceneManager.LoadScene("OptionsScene");
    }

    private void Quit()
    {
        Application.Quit();
    }
}
