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
    [SerializeField] public int startingPlayerHealth = 3;
    [SerializeField] public Item startingItem;
    [SerializeField] public float startingCash = 0f;
    [SerializeField] public int startingScore = 0;
    [SerializeField] public int startingDifficulty = 0;
    [Header("Level Generation")]
    public int initialFireCount = 3;
    public int initialCivillianCount = 3;
    public int initialfireExtinguisherCount = 1;
    public int initialfireAxeCount = 1;
    //static game stats
    public static int playerHealth = 3;
    public static Item currentItem;
    public static float cash = 0f;
    public static int score = 0;
    public static int difficulty = 0;
    public static bool playerIsAlive = false;
    //scene references
    [System.NonSerialized] public Camera mainCamera;
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
            currentItem = Singleton.startingItem;
            cash = Singleton.startingCash;
            difficulty = Singleton.startingDifficulty;
            return;
        }
        playerHealth = 3;
        currentItem = null;
        cash = 0f;
        score = 0;
        difficulty = 0;
    }
}
public interface IDamage
{
    int health { get; set; }
    float lastTimeDamaged { get; set; }
    float damageCoolDown { get; set; }
    void TakeDamage()
    {
        if (lastTimeDamaged + damageCoolDown > Time.time)
        {
            return;
        }
        health--;
        lastTimeDamaged = Time.time;
        if (health <= 0 && this is MonoBehaviour)
        {
            GameObject.Destroy(((MonoBehaviour)this).gameObject);
        }
        OnDamageTaken();
    }
    void OnDamageTaken() { }
}
public interface IFlash
{
    Color damageColour { get; set; }
    void Flash(SpriteRenderer spriteRenderer)
    {
        if (this is MonoBehaviour)
        {
            ((MonoBehaviour)this).StartCoroutine(FlashCorutine(spriteRenderer));
        }
    }
    private IEnumerator FlashCorutine(SpriteRenderer spriteRenderer)
    {
        Color originalColour = spriteRenderer.color;
        spriteRenderer.color = damageColour;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForFixedUpdate();
            spriteRenderer.color = Color.Lerp(damageColour, originalColour, i / 12f);
        }
        spriteRenderer.color = originalColour;
    }
}
