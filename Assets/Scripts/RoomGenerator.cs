using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    private TileBehaviour exit;
    //[Header("")]
    private void Start()
    {
        roomSize = new Vector2Int(Random.Range(minRoomSize.x, maxRoomSize.x), Random.Range(minRoomSize.y, maxRoomSize.y));
        GenerateWallsAndFloors(roomSize + Vector2Int.one * 2);
        GenerateDoor();
        GenerateFire(GameManager.Singleton.initialFireCount);
    }
    private void OnValidate()
    {
        Singleton = this;
    }
    private void GenerateWallsAndFloors(Vector2Int sizeToGenerate)
    {
        for (int j = 0; j < sizeToGenerate.y; j++)
        {
            Instantiate(GameManager.Singleton.wallPrefab, new Vector2(0, j), Quaternion.identity);
        }
        for (int i = 1; i < sizeToGenerate.x - 1; i++)
        {
            Instantiate(GameManager.Singleton.wallPrefab, new Vector2(i, 0), Quaternion.identity);
            for (int j = 1; j < sizeToGenerate.y - 1; j++)
            {
                Instantiate(GameManager.Singleton.floorPrefab, new Vector2(i, j), Quaternion.identity);
            }
            Instantiate(GameManager.Singleton.wallPrefab, new Vector2(i, sizeToGenerate.y - 1), Quaternion.identity);

        }
        for (int j = 0; j < sizeToGenerate.y; j++)
        {
            Instantiate(GameManager.Singleton.wallPrefab, new Vector2(sizeToGenerate.x - 1, j), Quaternion.identity);
        }
    }
    private void GenerateDoor()
    {
        Vector2Int doorSpawnPosition = new Vector2Int(Random.Range(0, 2), Random.Range(0, 2));
        doorSpawnPosition *= roomSize;
        if (Random.Range(0f, 1f) >= 0.5f)
        {
            doorSpawnPosition.x = Random.Range(1, roomSize.x + 1);
            doorSpawnPosition.y = Mathf.Max(doorSpawnPosition.y, 1);
        }
        else
        {
            doorSpawnPosition.y = Random.Range(1, roomSize.y + 1);
            doorSpawnPosition.x = Mathf.Max(doorSpawnPosition.x, 1);

        }
        exit = Instantiate(GameManager.Singleton.doorPrefab, (Vector2)doorSpawnPosition, Quaternion.identity).GetComponent<TileBehaviour>();
    }
    private void GenerateFire(int amount)
    {
        //try get all the tiles that the fire can start on
        List<Tile> possibleFireTiles = new List<Tile>();
        foreach (Tile tile in Tile.ActiveTiles.Values)
        {
            if (Vector2.Distance(tile.position, exit.position) < 4f)
            {
                continue;
            }
            if (Fire.CanSpread(tile.positionInt))
            {
                possibleFireTiles.Add(tile);
            }
        }
        //
        for (int i = 0; i < amount; i++)
        {
            if (possibleFireTiles.Count == 0)
            {
                break;
            }
            Tile newTileOnFire = possibleFireTiles[Random.Range(0, possibleFireTiles.Count)];
            //use forcespread since we know that we know it is a valid tile
            Fire.ForceSpread(newTileOnFire.positionInt);
            possibleFireTiles.Remove(newTileOnFire);
        }
    }
}
