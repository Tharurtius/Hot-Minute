using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private Button speed;
    [SerializeField] private Button foam;
    [SerializeField] private Button cooldown;
    [SerializeField] private Text cost;
    [SerializeField] private Text gold;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject upgradePanel;
    public void Speed()
    {
        if (GameManager.cash >= GameManager.cost)
        {
            GameManager.cash -= GameManager.cost;
            GameManager.Singleton.UpgradeSpeed();
        }
        else
        {
            DisableButton(speed);
        }
    }

    public void Foam()
    {
        if (GameManager.cash >= GameManager.cost)
        {
            GameManager.cash -= GameManager.cost;
            GameManager.Singleton.UpgradeFoam();
        }
        else
        {
            DisableButton(foam);
        }
    }

    public void Cooldown()
    {
        if (GameManager.cash >= GameManager.cost)
        {
            GameManager.cash -= GameManager.cost;
            GameManager.Singleton.UpgradeCooldown();
        }
        else
        {
            DisableButton(cooldown);
        }
    }

    public void DisableButton(Button button)
    {
        button.interactable = false;

        button.GetComponentInChildren<Text>().text = "Can't afford this";
    }

    public void SetupUI()
    {
        cost.text = GameManager.cost.ToString("N0");
        gold.text = "Gold :" + GameManager.cash.ToString("N0");
    }

    public void UpgradeScreen()
    {
        upgradePanel.SetActive(true);
        pausePanel.SetActive(false);
    }
}
