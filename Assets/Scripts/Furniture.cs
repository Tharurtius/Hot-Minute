using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Furniture : TileBehaviour, IDamage
{
    [SerializeField] private int health;
    private float lastTimeDamaged;
    [SerializeField] private float damageCoolDown;
    int IDamage.health { get => health; set => health = value; }
    float IDamage.lastTimeDamaged { get => lastTimeDamaged; set => lastTimeDamaged = value; }
    float IDamage.damageCoolDown { get => damageCoolDown; set => damageCoolDown = value; }
    void IDamage.TakeDamage() { TakeDamage(); }
    private void Update()
    {
        CheckTile();
    }
    public void CheckTile()
    {
        //if fire on the tile
        if (Tile.ActiveTiles[positionInt].attachedObjects.OfType<Fire>().Any())
        {
            Destroy(gameObject);
            TakeDamage();
            lastTimeDamaged = Time.time;
        }
    }
    private void TakeDamage()
    {
        if (lastTimeDamaged + damageCoolDown > Time.time)
        {
            return;
        }
        health--;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Axe"))
        {
            health -= 2;
        }
    }
}
