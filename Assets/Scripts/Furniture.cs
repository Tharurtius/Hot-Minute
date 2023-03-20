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
    private void Update()
    {
        CheckTile();
    }
    public void CheckTile()
    {
        if (tile.attachedObjects.OfType<Fire>().Any())
        {
            ((IDamage)this).TakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Axe"))
        {
            ((IDamage)this).TakeDamage();
        }
    }
}
