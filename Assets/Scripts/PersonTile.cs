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
            GameManager.Singleton.currentInventory.SetupUI();
            if (GameManager.deathsAllowed == 0)
            {
                return;
            }
            if (value == 0)
            {
                GameManager.Singleton.LevelWon();
            }
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
            GameManager.deathsAllowed--;
            if (GameManager.deathsAllowed == -1)
            {
                GameManager.Singleton.EndGame();
                return;
            }
            CurrentPeopleCount--;
            GameManager.Singleton.currentInventory.LowerCount();
        }
    }
    //I hate this -Tim
    public static void ResetCurrentPeopleCount()
    {
        currentPeopleCount = 0;
    }
}
