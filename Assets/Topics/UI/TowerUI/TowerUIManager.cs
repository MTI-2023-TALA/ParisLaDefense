using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUIManager : MonoBehaviour
{
    [SerializeField] private GameObject towerUI;
    [SerializeField] private GameObject towerUIupgrade;

    private bool isActive = false;

    [SerializeField] private Button btnCloseBuy;
    [SerializeField] private Button btnCloseUpgrade;

    public void Start()
    {
        this.removeTowerUI();
        btnCloseBuy.onClick.AddListener(removeTowerUI);
        btnCloseUpgrade.onClick.AddListener(removeTowerUI);
    }

    public void moveTowerUi(Vector3 newPos)
    {
        isActive = true;
        towerUI.SetActive(isActive);
        towerUI.transform.position = newPos;
    }

    public void moveTowerUiUpgrade(Vector3 newPos)
    {
        isActive = true;
        towerUIupgrade.SetActive(isActive);
        towerUIupgrade.transform.position = newPos;
    }

    public void removeTowerUI()
    {
        isActive = false;
        towerUI.SetActive(isActive);
        towerUIupgrade.SetActive(isActive);
    }

    public bool isTowerUIActive()
    {
        return isActive;
    }
}
