using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemID;
    public Sprite itemImage;
    public string itemName;
    public float itemValue;
    public ItemTile itemTile;
}
