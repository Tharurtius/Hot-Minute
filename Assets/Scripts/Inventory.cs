using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [Header("Inventory slot")]
    //actual inventory slot
    public Item invSlot;
    [Header("UI shit")]
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject deathsBar;
    [SerializeField] private Image itemImage;
    [SerializeField] private Text itemText;
    [SerializeField] private Text goldCounter;
    [SerializeField] private Text scoreCounter;
    [Header("Prefabs")]
    [SerializeField] private GameObject foamPrefab;
    [SerializeField] private GameObject axeBlade;
    [SerializeField] private Sprite blankSprite;


    private void Awake()
    {
        //reference itself to the gamemanager
        GameManager.Singleton.currentInventory = this;
    }

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
                    //get item
                    invSlot = tile.attachedObjects.OfType<ItemTile>().First().item;
                    //remove the tile
                    Destroy(tile.attachedObjects.OfType<ItemTile>().First());
                    //change UI
                    SetupUI();
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
                    //changeUI
                    SetupUI();
                }
            }
        }
    }

    public void DropItem()
    {
        //get tile
        if (Tile.ActiveTiles.TryGetValue(Vector2Int.RoundToInt(PlayerMovement.Singleton.transform.position), out Tile tile))
        {
            //if something is in the inventory and tile has no items
            if (invSlot != null && !tile.attachedObjects.OfType<ItemTile>().Any())
            {
                Instantiate(invSlot.itemTile, PlayerMovement.Singleton.transform.position, Quaternion.identity);
                invSlot = null;
            }
        }
    }

    public void UseItem()
    {
        //depends on the item
    }
    /// <summary>
    /// Sets up ui to new item
    /// </summary>
    public void SetupUI()
    {
        //if empty
        if (invSlot == null)
        {
            itemImage.sprite = blankSprite;
            itemText.text = "Empty";
        }
        else
        {
            itemImage.sprite = invSlot.itemImage;
            itemText.text = invSlot.itemName;
        }
    }
}
