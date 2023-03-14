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
    [Header("Game Settings")]
    public int initialFireCount = 3;
    public Camera mainCamera;
    private void OnEnable()
    {
        Singleton = this;
    }
    private void Start()
    {
        mainCamera = Camera.main;
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
