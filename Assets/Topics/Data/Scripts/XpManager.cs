using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpManager : MonoBehaviour
{
    private int level = 1;
    private int nextLevel = 0;
    private int xpRequired = 10;
    public int availablePoint = 0;
    public float moneyLevel = 0;
    public float damageLevel = 0;
    public float rangeLevel = 0;
    private UiManager uiManager;


    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("UiManager"))
        {
            uiManager = GameObject.Find("UiManager").GetComponent<UiManager>();
            uiManager.UpdateXpUi(progression(), level);
        }
    }

    public void updateUI()
    {
        uiManager = GameObject.Find("UiManager").GetComponent<UiManager>();
        uiManager.UpdateXpUi(progression(), level);
    }

    public void addXp()
    {
        nextLevel += 1;
        if (nextLevel == xpRequired)
        {
            level += 1;
            availablePoint += 1;
            xpRequired += 10;
            nextLevel = 0;
        }

        uiManager.UpdateXpUi(progression(), level);
    }

    public float progression()
    {
        return ((float)nextLevel / (float)xpRequired);
    }
}
