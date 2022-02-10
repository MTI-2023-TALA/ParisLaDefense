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

    public void TakeDamage()
    {
        life -= 1;
        // TODO: Game over
        Debug.Log(life);
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
}