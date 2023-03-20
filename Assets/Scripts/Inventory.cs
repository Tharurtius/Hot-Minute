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
    public GameObject nextButton;
    public GameObject resumeButton;
    public GameObject pausePanel;
    public Text pauseTitle;
    [Header("Prefabs")]
    [SerializeField] private GameObject foamPrefab;
    [SerializeField] private GameObject axeBlade;
    [SerializeField] private Sprite blankSprite;


    private void Awake()
    {
        //reference itself to the gamemanager
        GameManager.Singleton.currentInventory = this;

        //if death counter is lower than 5
        if (GameManager.deathsAllowed < 5)
        {
            for (int i = 0; i < 5 - GameManager.deathsAllowed; i++)
            {
                LowerCount();
            }
        }
        SetupUI();
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
                    Destroy(tile.attachedObjects.OfType<ItemTile>().First().gameObject);
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
                    //remove the tile
                    Destroy(tile.attachedObjects.OfType<ItemTile>().First().gameObject);
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
                SetupUI();
            }
        }
    }

    /// <summary>
    /// Uses the item
    /// </summary>
    /// <param name="player">The transform of the player</param>
    /// <returns>If item was used or not</returns>
    public bool UseItem(Transform player)
    {
        //check if slot is empty or player is dead
        if (invSlot == null || PlayerMovement.Singleton == null)
        {
            return false;
        }
        //else depends on the item
        else if (invSlot.itemID == "fireaxe")
        {
            //swing axe
            GameObject blade = Instantiate(axeBlade, player.position + player.right, player.rotation);
            blade.transform.Rotate(new Vector3(0, 0, 270));
            Destroy(blade, 0.5f);
            return true;
        }
        else if (invSlot.itemID == "extinguisher")
        {
            //spray foam
            GameObject foam;
            for (int i = 0; i < 5; i++)
            {
                foam = Instantiate(foamPrefab, player.position + player.right, player.rotation);
                Destroy(foam, 1f);
            }
            //destroy extinguisher
            invSlot = null;
            SetupUI();
            return true;
        }
        else if (invSlot.itemID == "gold")
        {
            //get tile
            if (Tile.ActiveTiles.TryGetValue(Vector2Int.RoundToInt(PlayerMovement.Singleton.transform.position), out Tile tile))
            {
                //if at exit
                if (AtExit())
                {
                    //increase cash
                    GameManager.cash += invSlot.itemValue;

                    invSlot = null;
                    SetupUI();
                }
            }
            return true;
        }
        else if (invSlot.itemID == "person")
        {
            //get tile
            if (Tile.ActiveTiles.TryGetValue(Vector2Int.RoundToInt(PlayerMovement.Singleton.transform.position), out Tile tile))
            {
                //if at exit
                if (AtExit())
                {
                    //gamemanager raise score and lower number of people in scene
                    GameManager.score++;
                    invSlot = null;
                    PersonTile.CurrentPeopleCount--;
                    SetupUI();  
                }
            }
            return true;
        }
        else
        {
            //else item cannot be used;
            return false;
        }
    }
    /// <summary>
    /// Sets up UI
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
        goldCounter.text = "Gold: " + GameManager.cash;
        scoreCounter.text = "Score: " + GameManager.score;
    }

    public bool AtExit()
    {
        //get tile
        if (Tile.ActiveTiles.TryGetValue(Vector2Int.RoundToInt(PlayerMovement.Singleton.transform.position), out Tile tile))
        {
            foreach (TileBehaviour item in tile.attachedObjects)
            {
                if (item.gameObject.CompareTag("Exit"))
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// Lowers health in the UI
    /// </summary>
    public void LowerHealth()
    {
        //should always be true
        if (healthBar.transform.childCount > 0)
        {
            Destroy(healthBar.transform.GetChild(0).gameObject);
        }
    }
    /// <summary>
    /// Lower death counter
    /// </summary>
    public void LowerCount()
    {
        //should always be true
        if (deathsBar.transform.childCount > 0)
        {
            Destroy(deathsBar.transform.GetChild(0).gameObject);
        }
    }
}
