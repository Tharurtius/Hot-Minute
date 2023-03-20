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
    //[Header("Game Settings")]

    [Header("Game Start Settings")]
    [SerializeField] private int startingPlayerHealth = 3;
    [SerializeField] private float startingCash = 0f;
    [SerializeField] private int startingScore = 0;
    [SerializeField] private int startingDifficulty = 0;
    [SerializeField] private int startingDeathsAllowed = 5;
    [Header("Level Generation")]
    public int initialFireCount = 3;
    public int initialCivillianCount = 3;
    public int initialfireExtinguisherCount = 1;
    public int initialfireAxeCount = 1;
    //static game stats
    public static int playerHealth = 3;
    public static float cash = 0f;
    public static int score = 0;
    public static int difficulty = 0;
    public static bool playerIsAlive = false;
    public static int deathsAllowed = 5;
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
            return;
        }
        playerHealth = 3;
        cash = 0f;
        score = 0;
        difficulty = 0;
        deathsAllowed = 5;
    }
}