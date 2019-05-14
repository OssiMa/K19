using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform player;
    public Enemy[] enemies;

    Map grid;

    private void Awake()
    {
        grid = GetComponent<Map>();
    }

    private void Update()
    {
        foreach(Enemy enemy in enemies)
        {
            FindPath(enemy.transform.position, player.position, enemy);
        }

    }

    void FindPath(Vector3 start, Vector3 target, Enemy enemy)
    {
        Tile startTile = grid.TileFromPos(start);
        Tile targetTile = grid.TileFromPos(target);

        List<Tile> openSet = new List<Tile>();
        HashSet<Tile> closedSet = new HashSet<Tile>();
        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            Tile current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].f < current.f || openSet[i].f == current.f && openSet[i].h < current.h)
                {
                    current = openSet[i];
                }
            }

            openSet.Remove(current);
            closedSet.Add(current);

            if(current == targetTile)
            {
                GetPath(startTile, targetTile, enemy);
                return;
            }

            foreach (Tile adjacent in grid.GetAdjacent(current))
            {
                if(!adjacent.walkable || closedSet.Contains(adjacent))
                {
                    continue;
                }

                int NewMoveCostToAdjacent = current.g + GetDistance(current, adjacent);
                if (NewMoveCostToAdjacent < adjacent.g || !openSet.Contains(adjacent))
                {
                    adjacent.g = NewMoveCostToAdjacent;
                    adjacent.h = GetDistance(adjacent, targetTile);
                    adjacent.parent = current;

                    if (!openSet.Contains(adjacent))
                    {
                        openSet.Add(adjacent);
                    }
                }
            }
        }
    }

    void GetPath(Tile start, Tile end, Enemy enemy)
    {
        List<Tile> path = new List<Tile>();
        Tile current = end;
        while(current != start)
        {
            path.Add(current);
            current = current.parent;
        }
        path.Reverse();

        grid.path = path;
        enemy.SetNewPath(path);

    }

    int GetDistance(Tile tileA, Tile tileB)
    {
        int distanceX = Mathf.Abs(tileA.gridX - tileB.gridX);
        int distanceY = Mathf.Abs(tileA.gridY - tileB.gridY);

        if (distanceX > distanceY)
        {
            return 14*distanceY + 10*(distanceX-distanceY);
        }
        return 14* distanceX +10 * (distanceY - distanceX);
    }

}
