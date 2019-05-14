using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Transform player;
    public LayerMask uwMask;
    public Vector2 gridSize;
    public float tileRadius;
    Tile[,] grid;

    float tileDiameter;
    int gridSizeX;
    int gridSizeY;

    private void Start()
    {
        tileDiameter = tileRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x / tileDiameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y / tileDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Tile[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;

        for(int x = 0; x< gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 pos = worldBottomLeft + Vector3.right * (x * tileDiameter + tileRadius) + Vector3.forward * (y * tileDiameter + tileRadius);
                bool walkable = !(Physics.CheckSphere(pos, tileRadius, uwMask));
                grid[x, y] = new Tile(walkable, pos, x, y);
            }
        }
    }

    public List<Tile> GetAdjacent(Tile tile)
    {
        List<Tile> adjacent = new List<Tile>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = tile.gridX + x;
                int checkY = tile.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY <gridSizeY)
                {
                    adjacent.Add(grid[checkX, checkY]);
                }
            }
        }

        return adjacent;
    }


    public Tile TileFromPos(Vector3 pos)
    {
        float px = (pos.x + gridSize.x / 2) / gridSize.x;
        float py = (pos.z + gridSize.y / 2) / gridSize.y;
        px = Mathf.Clamp01(px);
        py = Mathf.Clamp01(py);

        int x = Mathf.RoundToInt((gridSizeX - 1) * px);
        int y = Mathf.RoundToInt((gridSizeY - 1) * py);
        return grid[x,y];
    }

    public List<Tile> path;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1 , gridSize.y));

        if (grid != null)
        {
            Tile pTile = TileFromPos(player.position);
            foreach(Tile t in grid)
            {
                Gizmos.color = (t.walkable) ? Color.white : Color.red;
                if(path != null)
                {
                    if(path.Contains(t))
                    {
                        Gizmos.color = Color.black;
                    }
                }

                if(pTile == t)
                {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(t.worldPos, Vector3.one * (tileDiameter-.1f));
            }
        }
    }
}
