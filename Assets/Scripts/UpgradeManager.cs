using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public void Speed()
    {
        if (GameManager.cash >= GameManager.cost)
        {
            GameManager.cash -= GameManager.cost;
            GameManager.Singleton.UpgradeSpeed();
        }
    }

    public void Foam()
    {
        if (GameManager.cash >= GameManager.cost)
        {
            GameManager.cash -= GameManager.cost;
            GameManager.Singleton.UpgradeFoam();
        }
    }

    public void Cooldown()
    {
        if (GameManager.cash >= GameManager.cost)
        {
            GameManager.cash -= GameManager.cost;
            GameManager.Singleton.UpgradeCooldown();
        }
    }
}
