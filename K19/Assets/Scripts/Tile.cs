using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public bool walkable;
    public Vector3 worldPos;
    public int gridX;
    public int gridY;

    public int g;
    public int h;
    public Tile parent;

    public Tile(bool walkable, Vector3 worldPos, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPos = worldPos;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int f
    {
        get
        {
            return g + h;
        }
    }
}
