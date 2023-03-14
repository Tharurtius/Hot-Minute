using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fire : TileBehaviour
{
    private Vector2Int[] fourDirections = new Vector2Int[4] {Vector2Int.up, Vector2Int.down , Vector2Int.left , Vector2Int.right };//set up in the editor (Tim: DO NOT)
    Vector2 minMaxTime = new Vector2(3f, 5f);
    float timer;
    private void Start()
    {
        ResetTimer();
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Spread();
            ResetTimer();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamage damage))
        {
            damage.TakeDamage();
        }
        if (collision.gameObject.GetComponent<Player>())
        {
            PlayerMovement.Singleton.moveSpeed = PlayerMovement.Singleton.maxMoveSpeed * 0.5f;
        }
        if (collision.gameObject.TryGetComponent(out IDamage damagableObject))
        {
            damagableObject.TakeDamage();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            PlayerMovement.Singleton.moveSpeed = PlayerMovement.Singleton.maxMoveSpeed * 0.5f;
        }
        if (collision.gameObject.TryGetComponent(out IDamage damagableObject))
        {
            damagableObject.TakeDamage();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovement.Singleton.moveSpeed = PlayerMovement.Singleton.maxMoveSpeed;
    }
    private void ResetTimer()
    {
        timer = Random.Range(minMaxTime.x, minMaxTime.y);
    }
    public void Spread()
    {
        foreach (Vector2Int direction in fourDirections)
        {
            if (TrySpread(positionInt + direction))
            {
                return;
            }
        }
    }
    public static bool TrySpread(Vector2Int position)
    {
        if (!CanSpread(position))
        {
            return false;
        }
        //finally try see if RNG is on the player's side
        if (Random.value > 0.25f)
        {
            return false;
        }
        //if all looks good, spawn new fire
        Instantiate(GameManager.Singleton.firePrefab, (Vector2)position, Quaternion.identity);
        return true;
    }
    public static void ForceSpread(Vector2Int position)
    {
        Instantiate(GameManager.Singleton.firePrefab, (Vector2)position, Quaternion.identity);
    }
    public static bool CanSpread(Vector2Int position)
    {
        //get active tile in that position
        if (!Tile.ActiveTiles.TryGetValue(position, out Tile tile))
        {
            //if the tile doesn't exist, skip
            return false;
        }
        if (tile.attachedObjects.OfType<Fire>().Any())
        {
            //if the tile is on fire, skip
            return false;
        }
        if (tile.attachedObjects.OfType<Wall>().Any())
        {
            //if the tile is a wall, skip
            return false;
        }
        return true;
    }
}
