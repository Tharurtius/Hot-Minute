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
        Transform wallParent = new GameObject().transform;
        wallParent.gameObject.AddComponent<CompositeCollider2D>();
        for (int j = 0; j < sizeToGenerate.y; j++)
        {
            Instantiate(GameManager.Singleton.wallPrefab, new Vector2(0, j), Quaternion.identity).transform.parent = wallParent;
        }
        for (int i = 1; i < sizeToGenerate.x - 1; i++)
        {
            Instantiate(GameManager.Singleton.wallPrefab, new Vector2(i, 0), Quaternion.identity).transform.parent = wallParent;
            for (int j = 1; j < sizeToGenerate.y - 1; j++)
            {
                Instantiate(GameManager.Singleton.floorPrefab, new Vector2(i, j), Quaternion.identity);
            }
            Instantiate(GameManager.Singleton.wallPrefab, new Vector2(i, sizeToGenerate.y - 1), Quaternion.identity).transform.parent = wallParent;

        }
        for (int j = 0; j < sizeToGenerate.y; j++)
        {
            Instantiate(GameManager.Singleton.wallPrefab, new Vector2(sizeToGenerate.x - 1, j), Quaternion.identity).transform.parent = wallParent;
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
        Instantiate(GameManager.Singleton.playerPrefab, (Vector2)doorSpawnPosition, Quaternion.identity);
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
            Fire newFire = Fire.ForceSpread(newTileOnFire.positionInt);
            possibleFireTiles.Remove(newTileOnFire);
            for (int j = 0; j < Random.Range(0, 4); j++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (newFire.Spread(out Vector2 vector2))
                    {
                        if (Tile.ActiveTiles.TryGetValue(Vector2Int.RoundToInt(vector2), out Tile tile))
                        {
                            possibleFireTiles.Remove(tile);
                        }
                        break;
                    }
                }
            }
        }
    }
    private void GenerateCivillians()
    {
        List<Tile> possibleCivillianTiles = new List<Tile>();
        foreach (Tile tile in Tile.ActiveTiles.Values)
        {
            if (!CivillianCanSpawn(tile))
            {
                continue;
            }
            possibleCivillianTiles.Add(tile);
        }
        float longestDistance = 0;
        Tile furthestTile;
        if (possibleCivillianTiles.Count <= 0)
        {
            furthestTile = possibleCivillianTiles[0];
        }
        else
        {
            return;
        }
        foreach (Tile tile in possibleCivillianTiles)
        {
            if (Vector2.Distance(tile.position, exit.position) > longestDistance)
            {
                furthestTile = tile;
                longestDistance = Vector2.Distance(tile.position, exit.position);
            }
        }
        Instantiate(GameManager.Singleton.civillianPrefab, furthestTile.position, Quaternion.identity).GetComponent<Fire>();
    }
    private bool CivillianCanSpawn(Tile tile)
    {
        if (tile.attachedObjects.OfType<Wall>().Any())
        {
            return false;
        }
        if (tile.attachedObjects.OfType<Fire>().Any())
        {
            return false;
        }
        if (Vector2.Distance(tile.position, exit.position) < 4)
        {
            return false;
        }
        return true;
    }
}
