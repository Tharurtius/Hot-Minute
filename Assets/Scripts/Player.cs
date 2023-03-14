using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamage
{
    [SerializeField] private int health;
    private float lastTimeDamaged;
    [SerializeField] private float damageCoolDown;
    private Color originalColour;
    private SpriteRenderer spriteRenderer;
    #region IDamage variables
    int IDamage.health { get => health; set => health = value; }
    float IDamage.lastTimeDamaged { get => lastTimeDamaged; set => lastTimeDamaged = value; }
    float IDamage.damageCoolDown { get => damageCoolDown; set => damageCoolDown = value; }
    #endregion
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColour = spriteRenderer.color;
    }
    void IDamage.OnDamageTaken()
    {
        StartCoroutine(Flash());
    }
    private IEnumerator Flash()
    {
        spriteRenderer.color = Color.white;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForFixedUpdate();
            spriteRenderer.color = Color.Lerp(Color.white, originalColour, i / 12f);
        }
        spriteRenderer.color = originalColour;
    }
}
