using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    Database db;
    public int rotationSpeed = 60;
    public float respawnTime = 20;
    float respawnTimer;

    BoxCollider coll;

    private void Awake()
    {
        db = FindObjectOfType<Database>();
        coll = GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Player")
        {
            int i = Random.Range(0,100);

            if(i<100) //Common
            {
                if (i < 55) //Uncommon
                {
                    if (i < 25) //rare
                    {
                        if (i < 5) // legendary
                        {
                            db.GetDrop("Legendary");
                            coll.enabled = false;
                            transform.GetChild(0).gameObject.SetActive(false);
                            return;
                        }
                        db.GetDrop("Rare");
                        coll.enabled = false;
                        transform.GetChild(0).gameObject.SetActive(false);
                        return;
                    }
                    db.GetDrop("Uncommon");
                    coll.enabled = false;
                    transform.GetChild(0).gameObject.SetActive(false);
                    return;
                }
                db.GetDrop("Common");
                coll.enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);
                return;
            }
        }
    }

    private void Update()
    {

        if(transform.GetChild(0).gameObject.active == false)
        {
            respawnTimer += Time.deltaTime;
            if(respawnTimer >= respawnTime)
            {
                coll.enabled = true;
                transform.GetChild(0).gameObject.SetActive(true);
                respawnTimer = 0;
            }
        }
        else
        {
            transform.eulerAngles = transform.eulerAngles + new Vector3(0, rotationSpeed * Time.deltaTime, 0);
        }
    }
}
