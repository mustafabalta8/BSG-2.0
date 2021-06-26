using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mono.Data.Sqlite;
using System.Data;
using System;


//examples

//read:
//string query = ...
//IDataReader reader = dbManager.ReadRecords(query);

//while (reader.Read())
//{
//    string employeeName = reader.GetString(0);
//    Debug.Log("employee name: " + employeeName);
//}

//dbManager.CloseConnection();




//insert

//string query = ...

//dbManager.ReadRecords(query);

//dbManager.CloseConnection();



public class DbManager : MonoBehaviour
{
    public IDbConnection CreateConnection()
    {
        string conn = "URI=file:" + Application.dataPath + "/StreamingAssets/Database.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        return dbconn;
    }

    public IDataReader ReadRecords(string sqlQuery = null)
    {
        if (sqlQuery != null)
        {
            IDbConnection dbconn = CreateConnection();

            IDbCommand dbcmd = dbconn.CreateCommand();

            dbcmd.CommandText = sqlQuery;

            IDataReader reader = dbcmd.ExecuteReader();

            return reader;
        }
        else
        {
            IDbConnection dbconn = CreateConnection();

            IDbCommand dbcmd = dbconn.CreateCommand();

            dbcmd.CommandText = "";

            IDataReader reader = dbcmd.ExecuteReader();

            return reader;
        }
  
    }

    public IDataReader InsertRecords(string query)
    {
        IDbConnection dbconn = CreateConnection();
        IDbCommand dbcmd = dbconn.CreateCommand();

        dbcmd.CommandText = query;

        IDataReader reader = dbcmd.ExecuteReader();
        return reader;
    }


    public IDataReader DeleteRecords(string query)
    {
        IDbConnection dbconn = CreateConnection();
        IDbCommand dbcmd = dbconn.CreateCommand();

        dbcmd.CommandText = query;

        IDataReader reader = dbcmd.ExecuteReader();
        return reader;
    }


    public void CloseConnection()
    {
        IDbConnection dbconn = CreateConnection();
        IDataReader reader = ReadRecords();
        IDbCommand dbcmd = dbconn.CreateCommand();

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
}
