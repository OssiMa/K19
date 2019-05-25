using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class Database : MonoBehaviour
{
    public Text log;
    public Text scoreScreen;
    public GameObject GameUI;
    public GameObject GameOverUI;

    IDataReader reader;
    IDbCommand dbCommand;

    List<String> collectedDrops = new List<string>();
    List<String> possibleDrops = new List<string>();
    List<int> dropWeights = new List<int>();
    int totalWeight;
    string newText;


    // Start is called before the first frame update
    void Start()
    {
        string conn = "URI=file:" + Application.dataPath + "/Loot.db";
        IDbConnection dbConnection;
        dbConnection = (IDbConnection)new SqliteConnection(conn);
        dbConnection.Open();
        dbCommand = dbConnection.CreateCommand();
    }


    public void GetDrop(string table)
    {
        string sqlQuery = "SELECT LootName, Weight " + "FROM " + table;
        dbCommand.CommandText = sqlQuery;

        reader = dbCommand.ExecuteReader();
        while(reader.Read())
        {
            possibleDrops.Add(reader.GetString(0));
            dropWeights.Add(reader.GetInt32(1));
            totalWeight += reader.GetInt32(1);
        }
        reader.Close();

        int i = Random.Range(0, totalWeight);
        totalWeight = 0;

        for(int d = 0; d < possibleDrops.Count; d++)
        {
            totalWeight += dropWeights[d];
            if(i>totalWeight)
            {
                continue;
            }
            collectedDrops.Add(possibleDrops[d] + " (" + table + ")");
            UpdateDropLog();
            break;
        }

        totalWeight = 0;
        possibleDrops = new List<string>();
        dropWeights = new List<int>();
    }

    void UpdateDropLog()
    {
        newText = "Loot:\n";
        foreach(String s in collectedDrops)
        {
            newText += s + "\n";
        }
        log.text = newText;
    }

    public float GetSpeedMod()
    {
        return collectedDrops.Count * 0.05f;
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        GameUI.SetActive(false);
        GameOverUI.SetActive(true);
        scoreScreen.text = "You looted " + collectedDrops.Count + " items!";
    }
}
