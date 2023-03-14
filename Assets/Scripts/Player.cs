using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamage
{
    [SerializeField] private int health;
    [SerializeField] private float lastTimeDamaged;
    [SerializeField] private float damageCoolDown;
    [SerializeField] private float regenerationCoolDown;
    int IDamage.health { get => health; set => health = value; }
    float IDamage.lastTimeDamaged { get => lastTimeDamaged; set => lastTimeDamaged = value; }
    float IDamage.damageCoolDown { get => damageCoolDown; set => damageCoolDown = value; }
    float IDamage.regenerationCoolDown { get => regenerationCoolDown; set => regenerationCoolDown = value; }
}
