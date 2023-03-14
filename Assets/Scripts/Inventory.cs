using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Inventory : MonoBehaviour
{
    public Item invSlot;
    [SerializeField] private Image itemImage;
    [SerializeField] private Text itemText;

    [SerializeField] private GameObject foamPrefab;

    public void GetItem()
    {
        //get tile
        if (Tile.ActiveTiles.TryGetValue(Vector2Int.RoundToInt(PlayerMovement.Singleton.transform.position), out Tile tile))
        {
            //if item tile is on tile
            if (tile.attachedObjects.OfType<ItemTile>().Any())
            {
                //if inventory is empty
                if (invSlot == null)
                {
                    invSlot = tile.attachedObjects.OfType<ItemTile>().First().item;
                }
                else
                //if inventory is not empty
                {
                    //store inventory slot item
                    ItemTile lastItem = invSlot.itemTile;
                    //get new item
                    invSlot = tile.attachedObjects.OfType<ItemTile>().First().item;
                    //instantiate last item to tile
                    Instantiate(lastItem, PlayerMovement.Singleton.transform.position, Quaternion.identity);
                }
            }
        }
    }

    public void DropItem()
    {
        //if something is in the inventory
        if (invSlot != null)
        {
            Instantiate(invSlot.itemTile, PlayerMovement.Singleton.transform.position, Quaternion.identity);
            invSlot = null;
        }
    }

    public void UseItem()
    {
        //depends on the item
    }
}
