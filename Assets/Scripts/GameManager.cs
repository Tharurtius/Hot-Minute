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
    public Inventory currentInventory;
    //[Header("Game Settings")]

    [Header("Game Start Settings")]
    public int startingPlayerHealth = 3;
    public int startingDeaths = 5;
    public Item startingItem;
    public float startingCash = 0f;
    public int startingDifficulty = 0;
    [Header("Level Generation")]
    public int initialFireCount = 3;
    public int initialCivillianCount = 3;
    public Camera mainCamera;
    

    
    private void OnValidate()
    {
        
    }
    private void OnEnable()
    {
        Singleton = this;
    }
    private void Start()
    {
        if (!GameStats.playerIsAlive)
        {
            GameStats.Reset();
        }
        mainCamera = Camera.main;
    }
}
public static class GameStats
{
    public static int playerHealth = 3;
    public static int deathsAllowed = 5;
    public static Item currentItem;
    public static float cash = 0f;
    public static int difficulty = 0;
    public static bool playerIsAlive = false;
    public static void Reset()
    {
        playerIsAlive = true;
        if (GameManager.Singleton)
        {
            playerHealth = GameManager.Singleton.startingPlayerHealth;
            deathsAllowed = GameManager.Singleton.startingDeaths;
            currentItem = GameManager.Singleton.startingItem;
            cash = GameManager.Singleton.startingCash;
            difficulty = GameManager.Singleton.startingDifficulty;
            return;
        }
        playerHealth = 3;
        deathsAllowed = 5;
        currentItem = null;
        cash = 0f;
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
