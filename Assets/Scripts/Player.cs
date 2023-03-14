using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamage, IFlash
{
    private float lastTimeDamaged;
    [SerializeField] private float damageCoolDown;
    [SerializeField] private Color _damageColour = Color.white;

    public Color damageColour { get => _damageColour; set => _damageColour = value; }
    #region IDamage variables
    int IDamage.health { get => GameStats.playerHealth; set => GameStats.playerHealth = value; }
    float IDamage.lastTimeDamaged { get => lastTimeDamaged; set => lastTimeDamaged = value; }
    float IDamage.damageCoolDown { get => damageCoolDown; set => damageCoolDown = value; }
    #endregion
    void IDamage.OnDamageTaken()
    {
        if (TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            ((IFlash)this).Flash(spriteRenderer);
        }
    }
}
