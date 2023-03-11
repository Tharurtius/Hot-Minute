using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fire : TileBehaviour
{
    private Vector2Int[] fourDirections = new Vector2Int[4] {Vector2Int.up, Vector2Int.down , Vector2Int.left , Vector2Int.right };//set up in the editor (Tim: DO NOT)
    [SerializeField] private GameObject fireTilePrefab;


    //test shit, remove in actual game
    float testTimer = 5f;
    private void Update()
    {
        testTimer -= Time.deltaTime;
        if (testTimer <= 0f)
        {
            Spread();
            testTimer = 5f;
        }
    }

    public void Spread()
    {
        foreach (Vector2Int direction in fourDirections)
        {
            //get active tile in that direction
            if (!Tile.ActiveTiles.TryGetValue(tilePosition + direction, out Tile tile))
            {
                //if the tile doesn't exist, skip
                continue;
            }
            if (tile.attachedObjects.OfType<Fire>().Any())
            {
                //if the tile is on fire, skip
                continue;
            }
            if (tile.attachedObjects.OfType<Wall>().Any())
            {
                //if the tile is a wall, skip
                continue;
            }
            //if all looks good, spawn new fire
            Instantiate(fireTilePrefab, (Vector2)(tilePosition + direction), Quaternion.identity);
        }
    }
}
