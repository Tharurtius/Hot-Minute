using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Furniture : TileBehaviour
{
    //exists to block players and to be destroyed when on fire

    public void CheckTile()
    {
        //if fire on the tile
        if (Tile.ActiveTiles[positionInt].attachedObjects.OfType<Fire>().Any())
        {
            Destroy(gameObject);
        }
    }
}
