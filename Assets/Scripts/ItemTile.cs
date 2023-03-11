using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemTile : TileBehaviour
{
    //script is intended to go on prefabs

    //store scriptable data here
    public Item item;

    public void CheckTile()
    {
        //if fire on the tile
        if (Tile.ActiveTiles[tilePosition].attachedObjects.OfType<Fire>().Any())
        {
            if (this is PersonTile)
            {
                //also lower player score
            }
            Delete();
        }
    }

    public void Delete()
    {
        Tile.ActiveTiles[tilePosition].attachedObjects.Remove(this);
        Destroy(gameObject);
    }
}
