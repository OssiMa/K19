using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;

public class Database : MonoBehaviour
{
    IDataReader reader;
    IDbCommand dbCommand;


    // Start is called before the first frame update
    void Start()
    {
        string conn = "URI=file:" + Application.dataPath + "/Loot.db";
        IDbConnection dbConnection;
        dbConnection = (IDbConnection)new SqliteConnection(conn);
        dbConnection.Open();
        dbCommand = dbConnection.CreateCommand();

        string sqlQuery = "SELECT LootName, Weight " + "FROM Commons";
        dbCommand.CommandText = sqlQuery;

    }

    // Update is called once per frame
    void Update()
    {
        reader = dbCommand.ExecuteReader();
        while (reader.Read())
        {
            string name = reader.GetString(0);
            print(name);
        }
        reader.Close();

    }
}
