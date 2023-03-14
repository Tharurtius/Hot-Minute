using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemTile : TileBehaviour, IDamage
{
    //script is intended to go on prefabs

    //store scriptable data here
    public Item item;

    [SerializeField] protected int health;
    private float lastTimeDamaged;
    [SerializeField] private float damageCoolDown;
    int IDamage.health { get => health; set => health = value; }
    float IDamage.lastTimeDamaged { get => lastTimeDamaged; set => lastTimeDamaged = value; }
    float IDamage.damageCoolDown { get => damageCoolDown; set => damageCoolDown = value; }
    private void Update()
    {
        CheckTile();
    }
    public void CheckTile()
    {
        //if fire on the tile
        if (Tile.ActiveTiles[positionInt].attachedObjects.OfType<Fire>().Any())
        {
            ((IDamage)this).TakeDamage();
        }
    }
}
