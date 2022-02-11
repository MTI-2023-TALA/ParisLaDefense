using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellManager : MonoBehaviour
{
    private Button canonBallBtn;
    private Button chainSawBtn;

    public GameObject chainSawPrefab;
    public GameObject canonBallPrefab;

    private DataManager dataManager;
    private EnemyManager enemyManager;

    private void Start()
    {
        canonBallBtn = GameObject.Find(ButtonName.spellCanonBallBtn).GetComponent<Button>();
        chainSawBtn = GameObject.Find(ButtonName.spellChainSawBtn).GetComponent<Button>();

        dataManager = GameObject.Find(ObjectName.gameManager).GetComponent<DataManager>();
        enemyManager = GameObject.Find(ObjectName.gameManager).GetComponent<EnemyManager>();

        canonBallBtn.onClick.AddListener(SpellCanonBall);
        chainSawBtn.onClick.AddListener(SpellChainSaw);
    }

    private void SpellChainSaw()
    {
        int cost = 5;
        if (!dataManager.HasEnoughMana(cost))
        {
            return;
        }

        dataManager.RemoveMana(cost);
        ChainSaw chainSaw = Instantiate(chainSawPrefab, enemyManager.waypoints[enemyManager.waypoints.Count - 1], Quaternion.identity).GetComponent<ChainSaw>();
        chainSaw.Init(enemyManager.waypoints);
        Debug.Log("saw");
    }

    private void SpellCanonBall() 
    {
        int cost = 3;
        if (!dataManager.HasEnoughMana(cost))
        {
            return;
        }

        dataManager.RemoveMana(cost);
        Instantiate(canonBallPrefab, new Vector3(1f, 25f), Quaternion.identity);
        Debug.Log("canon");
    }
}
