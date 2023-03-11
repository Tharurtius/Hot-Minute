using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TileBehaviour : MonoBehaviour
{
    protected void OnEnable()
    {
        tilePosition = Vector2Int.RoundToInt(transform.position);
        if (!Tile.ActiveTiles.ContainsKey(tilePosition))
        {
            Tile.ActiveTiles.Add(tilePosition, new Tile(tilePosition));
        }
        Tile.ActiveTiles[tilePosition].attachedObjects.Add(this);
        transform.position = (Vector2)tilePosition;
    }
    protected Vector2Int tilePosition = Vector2Int.zero;
    public Vector2 position
    {
        get { return tilePosition; }
        set { SetPosition(Vector2Int.RoundToInt(value)); }
    }
    public Vector2Int positionInt
    {
        get { return tilePosition; }
        set { SetPosition(value); }
    }
    private void SetPosition(Vector2Int newPosition)
    {
        Tile.ActiveTiles[tilePosition].attachedObjects.Remove(this);
        //If the tile is empty, remove it from the list to make sure that the list isn't full of tiles that are empty
        if (Tile.ActiveTiles[tilePosition].attachedObjects.Count == 0)
        {
            Tile.ActiveTiles.Remove(tilePosition);
        }
        tilePosition = newPosition;
        transform.position = (Vector2)newPosition;
        if (!Tile.ActiveTiles.ContainsKey(tilePosition))
        {
            Tile.ActiveTiles.Add(tilePosition, new Tile(tilePosition));
        }
        Tile.ActiveTiles[tilePosition].attachedObjects.Add(this);
    }
}
//Tile class to hold info of each tile in the scene
//This is done since multiple scripts can be attached to the same tile/position
public class Tile
{
    public static Dictionary<Vector2Int, Tile> ActiveTiles = new Dictionary<Vector2Int, Tile>();
    Vector2Int position = Vector2Int.zero;
    public List<TileBehaviour> attachedObjects = new List<TileBehaviour>();
    #region Constructors
    public Tile(int x, int y)
    {
        position = new Vector2Int(x, y);
    }
    public Tile(float x, float y)
    {
        position = new Vector2Int((int)x, (int)y);
    }
    public Tile(Vector2Int vector2Int)
    {
        position = vector2Int;
    }
    #endregion
}