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

    public Button skillTreeBtn;
    public Button backToMainScreen;
    public Button pauseButtonFromPauseUI;

    public Button replayButton;
    public Button victoryReplayButton;
    public GameObject gameOverUI;
    public GameObject victoryUI;
    public GameObject pauseUI;

    public Slider XpBar;
    public Text levelText;

    private DataManager dataManager;
    private AudioSource audioSource;

    public GameObject skillTreeUI;

    private XpManager xpManager;

    public Button moneyUpgrade;
    public Text moneyLevel;
    public Text moneyDesc;
    public Button damageUpgrade;
    public Text damageLevel;
    public Text damageDesc;
    public Button rangeUpgrade;
    public Text rangeLevel;
    public Text rangeDesc;
    public Text availablePoints;

    public Button skillTreeReplayButton;


    public void Start()
    {
        GameObject musicGameObject = GameObject.Find("BackgroundAudio");
        audioSource = musicGameObject.GetComponent<AudioSource>();
        xpManager = GameObject.Find(ObjectName.optionManager).GetComponent<XpManager>();
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
        skillTreeBtn.onClick.AddListener(SkillTree);

        replayButton.onClick.AddListener(Replay);
        victoryReplayButton.onClick.AddListener(Replay);
        gameOverUI.SetActive(false);
        victoryUI.SetActive(false);
        skillTreeUI.SetActive(false);

        moneyUpgrade.onClick.AddListener(upgradeMoney);
        damageUpgrade.onClick.AddListener(upgradeDamage);
        rangeUpgrade.onClick.AddListener(upgradeRange);
        skillTreeReplayButton.onClick.AddListener(SkillTree);
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
            audioSource.Pause();
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
        }
        else
        {
            Time.timeScale = 1;
            pauseButton.GetComponentInChildren<Text>().text = "Pause";
            audioSource.UnPause();
            pauseUI.SetActive(false);
            dataManager.SetGameIsPaused(false);
        }
    }

    private void SkillTree()
    {
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
            audioSource.Pause();
            skillTreeUI.SetActive(true);
            dataManager.SetGameIsPaused(true);
            UpdateSkillTreeUI();
        }
        else
        {
            Time.timeScale = 1;
            audioSource.UnPause();
            skillTreeUI.SetActive(false);
            dataManager.SetGameIsPaused(false);
        }


    }

    private void GoToMainScreen()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void UpdateXpUi(float progress, int level)
    {
        Debug.Log("Hello !");
        levelText.text = "Level " + level;
        XpBar.value = progress;
        XpBar.maxValue = 1;
    }

    private void UpdateSkillTreeUI()
    {
        moneyLevel.text = xpManager.moneyLevel.ToString();
        moneyDesc.text = "+ d'argent (+" + (5 * xpManager.moneyLevel) + "$)";

        damageLevel.text = xpManager.damageLevel.ToString();
        damageDesc.text = "+ de dégâts (+" + (2 * xpManager.damageLevel) + "%)";

        rangeLevel.text = xpManager.rangeLevel.ToString();
        rangeDesc.text = "+ de range (+" + (0.2 * xpManager.rangeLevel) + ")";

        availablePoints.text = "Points disponibles: " + xpManager.availablePoint;
    }

    private void upgradeMoney()
    {
        if (xpManager.availablePoint > 0)
        {
            xpManager.availablePoint -= 1;
            xpManager.moneyLevel += 1;
            UpdateSkillTreeUI();
        }
    }

    private void upgradeDamage()
    {
        if (xpManager.availablePoint > 0)
        {
            xpManager.availablePoint -= 1;
            xpManager.damageLevel += 1;
            UpdateSkillTreeUI();
        }
    }

    private void upgradeRange()
    {
        if (xpManager.availablePoint > 0)
        {
            xpManager.availablePoint -= 1;
            xpManager.rangeLevel += 1;
            UpdateSkillTreeUI();
        }
    }
}
