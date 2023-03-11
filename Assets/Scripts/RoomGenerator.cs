using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : TileBehaviour
{
    #region Singelton
    private static RoomGenerator _singleton;
    public static RoomGenerator Singleton
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
                Debug.LogWarning($"wtf you doing setting {nameof(RoomGenerator)}'s singleton to nothing??");
            }
            else if (value != _singleton)
            {
                Debug.LogWarning($"{nameof(value)} already exists in the scene\nDeleting duplicate...");
                Destroy(value);
            }
        }
    }
    #endregion
    [SerializeField] public Vector2Int minRoomSize = Vector2Int.one * 5;
    [SerializeField] public Vector2Int maxRoomSize = Vector2Int.one * 10;
    private Vector2Int roomSize = Vector2Int.zero;
    [Header("Prefabs")]
    [SerializeField] public GameObject wallPrefab;
    [SerializeField] public GameObject floorPrefab;
    //[Header("")]
    private void Start()
    {
        roomSize = new Vector2Int(Random.Range(minRoomSize.x, maxRoomSize.x), Random.Range(minRoomSize.y, maxRoomSize.y));
        Generate(roomSize + Vector2Int.one);
    }
    private void OnEnable()
    {
        base.OnEnable();
        position = Vector2.zero;
    }
    private void OnValidate()
    {
        Singleton = this;
    }
    private void Generate(Vector2Int sizeToGenerate)
    {
        for (int j = 0; j < sizeToGenerate.y; j++)
        {
            Instantiate(wallPrefab, new Vector2(0, j), Quaternion.identity);
        }
        for (int i = 1; i < sizeToGenerate.x - 1; i++)
        {
            Instantiate(wallPrefab, new Vector2(i, 0), Quaternion.identity);
            for (int j = 1; j < sizeToGenerate.y - 1; j++)
            {
                Instantiate(floorPrefab, new Vector2(i, j), Quaternion.identity);
            }
            Instantiate(wallPrefab, new Vector2(i, sizeToGenerate.y - 1), Quaternion.identity);
            
        }
        for (int j = 0; j < sizeToGenerate.y; j++)
        {
            Instantiate(wallPrefab, new Vector2(sizeToGenerate.x - 1, j), Quaternion.identity);
        }
    }
}
