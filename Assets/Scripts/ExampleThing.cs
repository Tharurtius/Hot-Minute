using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleThing : TileBehaviour
{
    void Update()
    {
        position = Vector2.right * Time.time;
        Debug.Log(position);
        foreach (var item in Tile.ActiveTiles)
        {
            Debug.Log($"There are {item.Value.attachedObjects.Count} objects at tile {item.Key}");
        }
    }
}
