using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        transform.position = position;
    }
    private Tile _tile;
    public Tile tile
    {
        get { return _tile; }
        set { SetPosition(value.positionInt); }
    }
    private Vector2Int tilePosition = Vector2Int.zero;
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
        _tile = Tile.ActiveTiles[tilePosition];
    }
    private void OnDestroy()
    {
        Tile.ActiveTiles[positionInt].attachedObjects.Remove(this);
    }
}
//Tile class to hold info of each tile in the scene
//This is done since multiple scripts can be attached to the same tile/position
public class Tile
{
    public static Dictionary<Vector2Int, Tile> ActiveTiles = new Dictionary<Vector2Int, Tile>();
    public readonly Vector2Int positionInt = Vector2Int.zero;
    public Vector2 position { get => positionInt; }
    public List<TileBehaviour> attachedObjects = new List<TileBehaviour>();
    #region Constructors
    public Tile(int x, int y)
    {
        positionInt = new Vector2Int(x, y);
    }
    public Tile(float x, float y)
    {
        positionInt = new Vector2Int((int)x, (int)y);
    }
    public Tile(Vector2Int vector2Int)
    {
        positionInt = vector2Int;
    }
    #endregion
    public List<Tile> AdjacentTiles()
    {
        Vector2Int[] fourDirections = new Vector2Int[4] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        List<Tile> adjacentTiles = new List<Tile>();
        foreach (Vector2Int direction in fourDirections)
        {
            if (ActiveTiles.TryGetValue(positionInt + direction, out Tile tilecheck))
            {
                adjacentTiles.Add(tilecheck);
            }
        }
        return adjacentTiles;
    }
    public List<TileBehaviour> AdjacentTileBehaviours()
    {
        List<TileBehaviour> adjacentTileBehaviours = new List<TileBehaviour>();
        foreach (Tile tile in AdjacentTiles())
        {
            foreach (TileBehaviour tileBehaviour in tile.attachedObjects)
            {
                adjacentTileBehaviours.Add(tileBehaviour);
            }
        }
        return adjacentTileBehaviours;
    }
}