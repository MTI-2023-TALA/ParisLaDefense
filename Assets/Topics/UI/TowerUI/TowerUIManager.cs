using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUIManager : MonoBehaviour
{
    [SerializeField] private GameObject towerUI;
     
    private bool isActive = false;

    [SerializeField] private Button btnClose;

    public void Start()
    {
        this.removeTowerUI();
        btnClose.onClick.AddListener(removeTowerUI);
    }

    public void moveTowerUi(Vector3 newPos)
    {
        isActive = true;
        towerUI.SetActive(isActive);
        towerUI.transform.position = newPos;
    }

    public void removeTowerUI()
    {
        isActive = false;
        towerUI.SetActive(isActive);
    }

    public bool isTowerUIActive()
    {
        return isActive;
    }
}
