using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonTile : ItemTile, IDamage, IFlash
{
    //for if type comparison in itemtile script

    [SerializeField] private Color _damageColour = Color.white;

    public Color damageColour { get => _damageColour; set => _damageColour = value; }
    void IDamage.OnDamageTaken()
    {
        if (TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            ((IFlash)this).Flash(spriteRenderer);
        }
        if (health <= 0)
        {
            //also lower score here
            Destroy(gameObject);
        }
    }
}
