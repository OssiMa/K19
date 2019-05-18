using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour
{
    List<Tile> path;
    enum State {Seek, Stun, Attack }
    State currentState = State.Seek;

    public PlayerController player;

    public float stunLeft;
    public float speed;

    private float attackCD;



    // Update is called once per frame
    void Update()
    {
        switch (currentState) {
            case State.Seek:
                if (path != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, path[0].worldPos, speed * Time.deltaTime);
                }
                CheckDistance();
                break;
            case State.Stun:
                stunLeft -= Time.deltaTime;
                if (stunLeft <= 0)
                {
                    currentState = State.Seek;
                }
                break;
            case State.Attack:
                if(attackCD <= 0)
                {
                    player.TakeHit();
                    attackCD = 1;
                    CheckDistance();
                }
                break;
            default:
                break;
        }
        attackCD -= Time.deltaTime;

    }

    public void SetNewPath(List<Tile> path)
    {
        this.path = path;
    }

    public void TookHit()
    {
        currentState = State.Stun;
        stunLeft = 1;
    }

    void CheckDistance()
    {
        if(Vector3.Distance(transform.position, path.Last().worldPos) > 0.8)
        {
            currentState = State.Seek;
        }
        else
        {
            currentState = State.Attack;
        }
    }
}
