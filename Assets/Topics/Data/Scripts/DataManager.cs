using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private int life = 10;
    private int wave = 0;
    private int gold = 200;
    private int mana = 0;

    private int maxLife = 10;
    private int maxWave = 72;
    private int maxMana = 10;

    private bool gameIsPaused;

    [SerializeField] private UiManager uiManager;

    private XpManager xpManager;

    public void Start()
    {
        xpManager = GameObject.Find(ObjectName.optionManager).GetComponent<XpManager>();
        gold += (int)xpManager.moneyLevel * 5;

        uiManager.Init(wave, maxWave, gold, life, maxLife, mana, maxMana);
        this.gameIsPaused = false;
        xpManager.updateUI();
    }

    public void SetGameIsPaused(bool isPaused)
    {
        this.gameIsPaused = isPaused;
        if (this.gameIsPaused)
        {
            this.GetComponent<TowerUIManager>().removeTowerUI();
        }
    }
    public bool GetGameIsPaused()
    {
        return this.gameIsPaused;
    }

    public void TakeDamage()
    {
        if (life == 0)
        {
            return;
        }

        life -= 1;
        uiManager.updateLife(life);
    }

    public void AddGold(int gold)
    {
        this.gold += gold;
        uiManager.updateGold(this.gold);
    }

    public bool RemoveGold(int gold)
    {
        if (this.gold >= gold)
        {
            this.gold -= gold;
            uiManager.updateGold(this.gold);
            return true;
        }
        return false;
    }

    public void ChangeWave(int wave)
    {
        uiManager.updateWave(wave);
    }

    public bool HasEnoughMana(int mana)
    {
        return this.mana >= mana;
    }

    public void RemoveMana(int mana)
    {
        this.mana -= mana;
        uiManager.updateMana(this.mana);
    }

    public void AddMana()
    {
        if (mana < maxMana)
        {
            mana += 1;
            uiManager.updateMana(mana);
        }

    }
}
