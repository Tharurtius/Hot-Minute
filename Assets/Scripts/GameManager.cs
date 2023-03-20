using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    #region Singelton
    private static GameManager _singleton;
    public static GameManager Singleton
    {
        get { return _singleton; }
        set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (value == null)
            {
                Debug.LogWarning($"wtf you doing setting {nameof(value)}'s singleton to nothing??");
            }
            else if (value != _singleton)
            {
                Debug.LogWarning($"{nameof(value)} already exists in the scene\nDeleting duplicate...");
                Destroy(value);
            }
        }
    }
    #endregion
    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject doorPrefab;
    public GameObject firePrefab;
    public GameObject playerPrefab;
    public GameObject civillianPrefab;
    public GameObject fireExtinguisherPrefab;
    public GameObject fireAxePrefab;
    public GameObject furniturePrefab;
    public GameObject goldPrefab;
    //[Header("Game Settings")]

    [Header("Game Start Settings")]
    [SerializeField] private int startingPlayerHealth = 3;
    [SerializeField] private float startingCash = 0f;
    [SerializeField] private int startingScore = 0;
    [SerializeField] private int startingDifficulty = 0;
    [SerializeField] private int startingDeathsAllowed = 5;
    [SerializeField] private int startingFoam = 0;
    [SerializeField] private float startingSpeed = 1f;
    [SerializeField] private float startingCooldown = 1f;
    [SerializeField] private float startingCost = 1f;
    [Header("Level Generation")]
    public int initialFireCount = 3;
    public int initialFurnitureCount = 3;
    public int initialCivillianCount = 3;
    public int initialGoldCount = 3;
    public int initialfireExtinguisherCount = 1;
    public int initialfireAxeCount = 1;
    //static game stats
    public static int playerHealth = 3;
    public static float cash = 0f;
    public static int score = 0;
    public static int difficulty = 0;
    public static bool playerIsAlive = false;
    public static int deathsAllowed = 5;
    public static int bonusFoam = 0;
    public static float bonusSpeed = 1f;
    public static float cooldownReduction = 1f;
    public static float cost = 1f;

    //scene references
    [System.NonSerialized] public Camera mainCamera;
    [System.NonSerialized] public Inventory currentInventory;
    private void OnValidate()
    {
        
    }
    private void OnEnable()
    {
        Singleton = this;
    }
    private void Start()
    {
        if (!playerIsAlive)
        {
            ResetStats();
        }
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        if (deathsAllowed <= 0 || playerHealth <= 0)
        {
            playerIsAlive = false;
            EndGame();
        }
    }
    /// <summary>
    /// Resets the game stats to default values
    /// If a singleton of the game manager exists in the current scene, it sets the values to the starting values of the instance
    /// </summary>
    public static void ResetStats()
    {
        playerIsAlive = true;
        if (Singleton)
        {
            playerHealth = Singleton.startingPlayerHealth;
            cash = Singleton.startingCash;
            difficulty = Singleton.startingDifficulty;
            score = Singleton.startingScore;
            deathsAllowed = Singleton.startingDeathsAllowed;
            bonusFoam = Singleton.startingFoam;
            bonusSpeed = Singleton.startingSpeed;
            cooldownReduction = Singleton.startingCooldown;
            cost = Singleton.startingCost;
            return;
        }
        playerHealth = 3;
        cash = 0f;
        score = 0;
        difficulty = 0;
        deathsAllowed = 5;
        bonusFoam = 0;
        bonusSpeed = 1f;
        cooldownReduction = 1f;
        cost = 1f;
    }
    /// <summary>
    /// Runs various stuff for the end of level
    /// </summary>
    public void NextLevel()
    {
        //regenerate player health
        playerHealth = Mathf.Min(3, playerHealth + 1);
        //add 1 more death allowed if less than 5
        if (deathsAllowed < 5)
        {
            deathsAllowed++;
        }
        //restart time
        Time.timeScale = 1f;
    }
    /// <summary>
    /// When you win the level, brings up UI
    /// </summary>
    public void LevelWon()
    {
        currentInventory.pausePanel.SetActive(true);
        currentInventory.resumeButton.SetActive(false);
        currentInventory.nextButton.SetActive(true);
        currentInventory.pauseTitle.text = "No more survivors in the building";
        //pause time so player doesnt die in background
        Time.timeScale = 0f;
    }

    public void Pause()
    {
        Time.timeScale = 0f;

        currentInventory.pausePanel.SetActive(true);
    }

    public void Unpause()
    {
        Time.timeScale = 1f;

        currentInventory.pausePanel.SetActive(false);
    }

    public void EndGame()
    {
        currentInventory.pausePanel.SetActive(true);
        currentInventory.resumeButton.SetActive(false);
        currentInventory.pauseTitle.text = "You can no longer work";
    }
    //for use by a referencer
    public void UpgradeFoam()
    {
        bonusFoam++;
        cost++;
    }

    public void UpgradeSpeed()
    {
        bonusSpeed += 0.1f;
        cost++;
    }

    public void UpgradeCooldown()
    {
        cooldownReduction *= 0.95f;
        cost++;
    }
}