using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonTile : ItemTile, IDamage
{
    //for if type comparison in itemtile script
    [SerializeField] private int health;
    private float lastTimeDamaged;
    [SerializeField] private float damageCoolDown;
    int IDamage.health { get => health; set => health = value; }
    float IDamage.lastTimeDamaged { get => lastTimeDamaged; set => lastTimeDamaged = value; }
    float IDamage.damageCoolDown { get => damageCoolDown; set => damageCoolDown = value; }
}
