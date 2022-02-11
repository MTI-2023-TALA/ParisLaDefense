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

    [SerializeField] private UiManager uiManager;

    public void Start()
    {
        uiManager.Init(wave, maxWave, gold, life, maxLife, mana, maxMana);
        StartCoroutine(AddMana());
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

    IEnumerator AddMana()
    {
        while (true) {
            if (mana < 10)
            {
                mana += 1;
                uiManager.updateMana(mana);
                
            }
            yield return new WaitForSeconds(1);
        }
    }
}