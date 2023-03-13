using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleThing : TileBehaviour
{
    [SerializeField] private bool destroy;
    [SerializeField] private bool debug;
    void Update()
    {
        position = Vector2.right * Time.time * 0.2f;
        //Debug.Log(position);
        foreach (KeyValuePair<Vector2Int, Tile> pair in Tile.ActiveTiles)
        {
            //Debug.Log($"There are {pair.Value.attachedObjects.Count} objects at tile {pair.Key}");
            foreach (TileBehaviour tileBehaviour in pair.Value.attachedObjects)
            {
                Debug.Log($"Item {tileBehaviour} is at {tileBehaviour.position}");
            }
        }
        if (destroy)
        {
            Destroy(gameObject);
            destroy = false;
        }
    }
}
