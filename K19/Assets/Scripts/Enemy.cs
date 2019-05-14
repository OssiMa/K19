using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour
{
    List<Tile> path;
    int current = 0;
    bool reached = false;


    // Update is called once per frame
    void Update()
    {
        if (path != null && !reached)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[current].worldPos, 0.1f);
            if(Vector3.Distance(transform.position, path[current].worldPos) < 0.1)
            {
                if (path[current].worldPos != path.Last().worldPos)
                {
                    current++;
                }
                else
                {
                    reached = true;
                }
                
            }
        }
    }

    public void SetNewPath(List<Tile> path)
    {
        current = 0;
        this.path = path;
        reached = false;
    }
}
