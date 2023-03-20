using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    int health { get; set; }
    float lastTimeDamaged { get; set; }
    float damageCoolDown { get; set; }
    void TakeDamage()
    {
        if (lastTimeDamaged + damageCoolDown > Time.time)
        {
            return;
        }
        health--;
        lastTimeDamaged = Time.time;
        if (health <= 0 && this is MonoBehaviour)
        {
            GameObject.Destroy(((MonoBehaviour)this).gameObject);
        }
        OnDamageTaken();
    }
    void OnDamageTaken() { }
}
public interface IFlash
{
    Color damageColour { get; set; }
    void Flash(SpriteRenderer spriteRenderer)
    {
        if (this is MonoBehaviour)
        {
            ((MonoBehaviour)this).StartCoroutine(FlashCorutine(spriteRenderer));
        }
    }
    private IEnumerator FlashCorutine(SpriteRenderer spriteRenderer)
    {
        Color originalColour = spriteRenderer.color;
        spriteRenderer.color = damageColour;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForFixedUpdate();
            spriteRenderer.color = Color.Lerp(damageColour, originalColour, i / 12f);
        }
        spriteRenderer.color = originalColour;
    }
}
