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
        private string url = "https://webservices.educom.nu/services/first/";
        private string pathToDatabase;


        // Database maken
        public void createDatabase()
        {
            Resources res = this.context.Resources;
            string app_name = res.GetString(Resource.String.app_name);
            string app_version = res.GetString(Resource.String.app_version);
            string createTableData = res.GetString(Resource.String.createTableData);

            Console.WriteLine(createTableData);

            string dbname = "_db_" + app_name + "_" + app_version + ".sqlite";
            Console.WriteLine(dbname);

            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            this.pathToDatabase = Path.Combine(documentsPath, dbname);

            if (!File.Exists(pathToDatabase))
            {

                SqliteConnection.CreateFile(this.pathToDatabase);
                var connectionString = String.Format("Data Source ={0}; Version = 3;", this.pathToDatabase);
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
                this.downloadData();
            }
        }

        public void downloadData()
        {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;

            try
            {
                byte[] myDataBuffer = webClient.DownloadData(this.url);
                string download = Encoding.ASCII.GetString(myDataBuffer);
                JsonValue value = JsonValue.Parse(download);


                foreach (JsonObject result in value)
                {
                    Console.WriteLine(result["code"] + " = " + result["description"]);
                    this.dataToDatabase(result["code"], result["description"]);
                    Console.WriteLine("DATA DOWNLOADED");
                }
            }
            catch (WebException)
            {
                // Doe vooralsnog niks, straks wellicht een boolean terug
                // geven of e.e.a. gelukt is of niet
            }
        }

        public void dataToDatabase(string code, string description)
        {
            var connectionString = String.Format("Data Source ={0}; Version = 3;", this.pathToDatabase);
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

        public ArrayList getAllData()
        {
            ArrayList dataRecords = new ArrayList();
            var connectionString = String.Format("Data Source={0};Version=3;", this.pathToDatabase);
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
            this.createDatabase();
        }
        public class dataRecord
        {
            public int id;
            public string code;
            public string description;

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
}