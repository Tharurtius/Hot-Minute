using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonTile : ItemTile, IDamage, IFlash
{
    //for if type comparison in itemtile script
    [SerializeField] private Color _damageColour = Color.white;
    private static int currentPeopleCount = 0;
    public static int CurrentPeopleCount
    {
        get => currentPeopleCount;
        set
        {
            currentPeopleCount = value;
            //ui update
            GameManager.Singleton.currentInventory.SetupUI();
        }
    }
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
            currentPeopleCount--;
            GameManager.Singleton.currentInventory.LowerCount();
        }
    }
    private void Start()
    {
        currentPeopleCount++;
    }
}
