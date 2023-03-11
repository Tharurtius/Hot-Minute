using System.Collections;
using System.Collections.Generic;
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
            //if active tile in that direction
            if (Tile.ActiveTiles.ContainsKey(tilePosition + direction))
            {
                //set up check
                bool flammable = true;
                //check every attached object on the tile
                foreach (TileBehaviour tile in Tile.ActiveTiles[tilePosition + direction].attachedObjects)
                {
                    //if there are no fire or wall objects attached
                    if (tile is Fire || tile is Wall)
                    {
                        //makes tile inflammable
                        flammable = false;
                        break;
                    }
                }
                //if flammable, instantiate fire
                if (flammable)
                {
                    Instantiate(fireTilePrefab, (Vector2)(tilePosition + direction), Quaternion.identity);
                }
            }
            //else do nothing
        }
    }
}
