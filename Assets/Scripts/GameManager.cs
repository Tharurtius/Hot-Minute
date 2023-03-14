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
    float regenerationCoolDown { get; set; }
    void TakeDamage()
    {
        if (lastTimeDamaged + damageCoolDown > Time.time)
        {
            return;
        }
        health--;
        lastTimeDamaged = Time.time;
    }
}
