using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct WaveManager
{
    public int currentWave;
    public int maxWave;
    public Text textWave;
}

[System.Serializable]
public struct GoldManager
{
    public int currentGold;
    public Text textGold;
}

[System.Serializable]
public struct LifeManager
{
    public int currentLife;
    public int maxLife;
    public Text textLife;
}

[System.Serializable]
public struct ManaManager
{
    public int currentMana;
    public int maxMana;
    public Text textMana;
}

public class UiManager : MonoBehaviour
{
    public WaveManager waveManager;
    public GoldManager goldManager;
    public LifeManager lifeManager;
    public ManaManager manaManager;

    public Button pauseButton;
    public Button backToMainScreen;
    public Button pauseButtonFromPauseUI;

    public Button replayButton;
    public Button victoryReplayButton;
    public GameObject gameOverUI;
    public GameObject victoryUI;
    public GameObject pauseUI;

    private DataManager dataManager;
    private AudioSource audioSource;

    public void Start()
    {
        GameObject musicGameObject = GameObject.Find("BackgroundAudio");
        audioSource = musicGameObject.GetComponent<AudioSource>();
    }

    public void Init(int wave, int maxWave, int gold, int life, int maxLife, int mana, int maxMana)
    {
        Time.timeScale = 1;

        dataManager = GameObject.Find(ObjectName.gameManager).GetComponent<DataManager>();

        this.waveManager.currentWave = wave;
        this.waveManager.maxWave = maxWave;

        this.goldManager.currentGold = gold;

        this.lifeManager.currentLife = life;
        this.lifeManager.maxLife = maxLife;

        this.manaManager.currentMana = mana;
        this.manaManager.maxMana = maxMana;

        _setWaweUI(this.waveManager);
        _setGoldUI(this.goldManager);
        _setLifeUI(this.lifeManager);
        _setManaUI(this.manaManager);

        pauseUI.SetActive(false);
        pauseButton.onClick.AddListener(Pause);
        backToMainScreen.onClick.AddListener(GoToMainScreen);
        pauseButtonFromPauseUI.onClick.AddListener(Pause);

        replayButton.onClick.AddListener(Replay);
        victoryReplayButton.onClick.AddListener(Replay);
        gameOverUI.SetActive(false);
        victoryUI.SetActive(false);
    }

    private void _setWaweUI(WaveManager waveManager)
    {
        waveManager.textWave.text = waveManager.currentWave + "/" + waveManager.maxWave;   
    }

    public void updateWave(int newCurrentWave)
    {
        waveManager.currentWave = newCurrentWave;
        _setWaweUI(waveManager);
    }


    private void _setGoldUI(GoldManager goldManager)
    {
        goldManager.textGold.text = goldManager.currentGold + "$";
    }

    public void updateGold(int newGold)
    {
        goldManager.currentGold = newGold;
        _setGoldUI(goldManager);
    }

    private void _setLifeUI(LifeManager lifeManager)
    {
        lifeManager.textLife.text = lifeManager.currentLife + "/" + lifeManager.maxLife;
        if (lifeManager.currentLife == 0)
        {
            Time.timeScale = 0;
            dataManager.SetGameIsPaused(true);
            gameOverUI.SetActive(true);
        }
    }

    public void updateLife(int newLife)
    {
        lifeManager.currentLife = newLife;
        _setLifeUI(lifeManager);
    }

    private void _setManaUI(ManaManager manaManager)
    {
        manaManager.textMana.text = manaManager.currentMana + "/" + manaManager.maxMana;
    }

    public void updateMana(int newMana)
    {
        manaManager.currentMana = newMana;
        _setManaUI(manaManager);
    }

    public void Replay()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private void Pause()
    {

        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
            audioSource.Pause();
            pauseButton.GetComponentInChildren<Text>().text = "Reprendre";
            pauseUI.SetActive(true);
            dataManager.SetGameIsPaused(true);
        } else
        {
            Time.timeScale = 1;
            pauseButton.GetComponentInChildren<Text>().text = "Pause";
            audioSource.UnPause();
            pauseUI.SetActive(false);
            dataManager.SetGameIsPaused(false);
        }
    }

    private void GoToMainScreen()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
