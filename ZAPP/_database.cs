using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Json;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;
using Android.App;
using Android.Content;
using Android.Content.Res;
using ZAPP;

namespace ZAPP
{
   class _database
    {
        // Context definieren
        private Context context;
        private string educomUrl = "https://webservices.educom.nu/services/first/";
        private string userUrl = "http://192.168.0.143/Cockpit-ZAPP/cockpit-master/api/collections/get/user?token=9d9a3b472d501a972c788077b12fb5/";
        private string educomDB;
        private string userDB;
        

        // Database maken
        public string createDatabase(string url, string createTableData, string databaseName, Action<string, string> downloadData) // 
        {
            Resources res = this.context.Resources;
            string app_name = res.GetString(Resource.String.app_name);
            string app_version = res.GetString(Resource.String.app_version);

            Console.WriteLine(createTableData);

            string dbname = $"_db_{app_name}_{app_version}_{databaseName}.sqlite";
            Console.WriteLine(dbname);

            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string databasePath = Path.Combine(documentsPath, dbname);

            if (!File.Exists(databasePath))
            {
                var connectionString = String.Format("Data Source={0};Version=3;", databasePath);
                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        // Table data
                        cmd.CommandText = createTableData;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                downloadData(url, databasePath);
            }
            return databasePath;
        }

        public void createAllDatabases()
        {
            Resources res = this.context.Resources;

            this.educomDB = createDatabase(educomUrl, res.GetString(Resource.String.createTableData), "educomDB", downloadEducomData);
            this.userDB = createDatabase(userUrl, res.GetString(Resource.String.createTableUser), "userDB", downloadUserData);
          //  this.userDB = createDatabase(newTableUrl, res.GetString(Resource.String.createTableNew, "userDB", downloadNewTableData));
        }

        public void downloadUserData(string url, string databasePath)
        {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            try
            {
                byte[] myDataBuffer = webClient.DownloadData(url);
                string download = Encoding.ASCII.GetString(myDataBuffer);
                JsonValue value = JsonValue.Parse(download);
                var entries = value["entries"];

                foreach (JsonObject item in entries)
                {

                    Console.WriteLine($"{item["username"]} = {item["password"]}");
                    this.userToDatabase(item["username"], item["password"], databasePath);

                }

            }
            catch (WebException)
            {

            }
        }
        public void downloadEducomData(string url, string databasePath)
        {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            try
            {
                byte[] myDataBuffer = webClient.DownloadData(url);
                string download = Encoding.ASCII.GetString(myDataBuffer);
                JsonValue value = JsonValue.Parse(download);
                foreach (JsonObject item in value)
                {
                   
                        Console.WriteLine($"{item["code"]} = {item["description"]}");
                        this.dataToDatabase(item["code"], item["description"], databasePath);
          
                }

            }
            catch (WebException)
            {

            }
        }

        public void userToDatabase(string username, string password, string dbPath)
        {
            var connectionString = String.Format("Data Source ={0}; Version = 3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    // Table data
                    cmd.CommandText = "INSERT INTO user (username,password) VALUES (@username, @password)";
                    cmd.Parameters.Add(new SqliteParameter("@username", username));
                    cmd.Parameters.Add(new SqliteParameter("@password", password));
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("USER INSERTED TO DATABASE");
                }
                conn.Close();
            }
        }

        public void dataToDatabase(string code, string description, string dbPath)
        {
            var connectionString = String.Format("Data Source ={0}; Version = 3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    // Table data
                    cmd.CommandText = "INSERT INTO data (code,description) VALUES (@code, @description)";
                    cmd.Parameters.Add(new SqliteParameter("@code", code));
                    cmd.Parameters.Add(new SqliteParameter("@description", description));
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("DATA INSERTED TO DATABASE");
                }
                conn.Close();
            }
        }

        public ArrayList getAllData(string dbPath)
        {
            ArrayList dataRecords = new ArrayList();
            var connectionString = String.Format("Data Source={0};Version=3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM data";
                    cmd.CommandType = CommandType.Text;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("ALL DATA IS BEING READ");
                            dataRecords.Add(new dataRecord(reader));
                        }
                    }
                }
                conn.Close();
            }
            Console.WriteLine("DATA RETURNED TO DATARECORDS");
            return dataRecords;
        }
        // Constructor
        public _database(Context context)
        {
            this.context = context;
            this.createAllDatabases();
        }

    }

    public class dataRecord
    {
        public int id;
        public string code;
        public string description;

        public dataRecord()
        {

        }

        public dataRecord(JsonValue record)
        {
            this.code = (string)record["code"];
            this.description = (string)record["description"];
        }

        public dataRecord(SqliteDataReader record)
        {
            this.id = (int)(Int64)record["id"];
            this.code = (string)record["code"];
            this.description = (string)record["description"];
        }
    }
}