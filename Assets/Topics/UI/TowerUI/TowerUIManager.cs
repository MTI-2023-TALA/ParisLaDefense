using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUIManager : MonoBehaviour
{
    [SerializeField] private GameObject towerUI;
    [SerializeField] private GameObject towerUIupgrade;

    private TileMapManager tileMapManager;

    private bool isActive = false;
    private bool isUpgradeActive = false;

    [SerializeField] private Button btnCloseBuy;
    [SerializeField] private Button btnCloseUpgrade;
    [SerializeField] public Text sellAmountText;
    [SerializeField] private Text levelUpText;

    public void Start()
    {
        this.removeTowerUI();
        tileMapManager = GameObject.Find(ObjectName.gameManager).GetComponent<TileMapManager>();
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
        isUpgradeActive = true;
        towerUIupgrade.SetActive(isActive);
        towerUIupgrade.transform.position = newPos;
        Turret turret = tileMapManager.GetClosestTurret().GetComponent<Turret>();
        sellAmountText.text = (turret.CalculateSell()).ToString() + "$";
        updateTowerUpgradeCost(turret);
    }

    public void updateTowerUpgradeCost(Turret turret)
    {

        levelUpText.text = (turret.CalculateUpgrade()).ToString() + "$";
        sellAmountText.text = ((turret.CalculateSell())).ToString() + "$";
    }

    public void removeTowerUI()
    {
        isActive = false;
        isUpgradeActive = false;
        towerUI.SetActive(isActive);
        towerUIupgrade.SetActive(isActive);
    }

    public bool isTowerUIActive()
    {
        return isActive;
    }

    public bool isTowerUIUpdateActive()
    {
        return isUpgradeActive;
    }
}
