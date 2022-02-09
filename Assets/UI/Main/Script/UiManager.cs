using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
        _setWaweUI(waveManager);
        _setGoldUI(goldManager);
        _setLifeUI(lifeManager);
        _setManaUI(manaManager);
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
}
