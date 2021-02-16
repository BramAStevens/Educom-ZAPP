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
using Newtonsoft.Json;

namespace ZAPP
{
   class _database
    {
        // Context definieren
        private Activity activity;
        private string taskUrl = "http://192.168.0.143/Cockpit-ZAPP/cockpit-master/api/collections/get/task?token=9d9a3b472d501a972c788077b12fb5/";
        private string userUrl = "http://192.168.0.143/Cockpit-ZAPP/cockpit-master/api/collections/get/user?token=9d9a3b472d501a972c788077b12fb5/";
        private string clientUrl = "http://192.168.0.143/Cockpit-ZAPP/cockpit-master/api/collections/get/client?token=9d9a3b472d501a972c788077b12fb5/";
        private string activityUrl = "http://192.168.0.143/Cockpit-ZAPP/cockpit-master/api/collections/get/activity?token=9d9a3b472d501a972c788077b12fb5/";
        private string postActivityUrl = "http://192.168.0.143/Cockpit-ZAPP/cockpit-master/api/collections/save/activity?token=9d9a3b472d501a972c788077b12fb5/";
        private string postTaskUrl = "http://192.168.0.143/Cockpit-ZAPP/cockpit-master/api/collections/save/task?token=9d9a3b472d501a972c788077b12fb5/";
        public static string ZAPPDB = "ZAPPDB";

        // Constructor
        
        public _database(Activity activity, bool createDB)
        {
            this.activity = activity;
            if(createDB)
            {
                this.createAllDatabases();
            }
        }

        public static string makeDatabaseName(Activity activity)
        {
            Resources res = activity.Resources;
            string app_name = res.GetString(Resource.String.app_name);
            string app_version = res.GetString(Resource.String.app_version);
            string dbname = $"_db_{app_name}_{app_version}_{_database.ZAPPDB}.sqlite";
            Console.WriteLine(dbname);
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string databasePath = Path.Combine(documentsPath, dbname);
            return databasePath;
        }
     
        public void createDatabase(string url, string createTableData, Action<string, string> downloadData) 
        {
                string databasePath = makeDatabaseName(this.activity); 
                var connectionString = String.Format("Data Source={0};Version=3;", databasePath);
                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = createTableData;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                downloadData(url, databasePath);
        }

        public void createTableTask()
        {
            Resources res = this.activity.Resources;
            createDatabase(taskUrl, res.GetString(Resource.String.createTableTask), downloadTaskData);
        }

        public void createAllDatabases()
        {
            Resources res = this.activity.Resources;
            createDatabase(taskUrl, res.GetString(Resource.String.createTableTask), downloadTaskData);
            createDatabase(activityUrl, res.GetString(Resource.String.createTableActivity), downloadActivityData);
            createDatabase(userUrl, res.GetString(Resource.String.createTableUser), downloadUserData);
            createDatabase(clientUrl, res.GetString(Resource.String.createTableClient), downloadClientData);
        }

        public void downloadClientData(string url, string databasePath)
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
                    this.clientToDatabase(item["map"], item["address"], item["telephone"], item["planning"], databasePath);
                }
            }
            catch (WebException)
            {

            }
        }
        public void uploadActivityData()
        {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            try
            {
                List<ActivityRecord> allActivities = Config.getAllActivities();
                var objAsJson = JsonConvert.SerializeObject(allActivities);
                string result = writeJsonToServer(objAsJson.ToString(), postActivityUrl);
            }
            catch (WebException)
            {

            }
        }

        public void uploadTaskData()
        {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            try
            {
                List<TaskRecord> allTasks = Config.getAllTasks();
                var objAsJson = JsonConvert.SerializeObject(allTasks);
                string result = writeJsonToServer(objAsJson.ToString(), postTaskUrl);
            }
            catch (WebException)
            {

            }
        }

        public string writeJsonToServer(string json, string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write("{\"data\": " + json + " }");
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result.ToString();
            }
        }

        public void downloadActivityData(string url, string databasePath)
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
                    this.activityToDatabase(item["_id"], item["task_id"], item["isCompleted"], item["activityName"], databasePath);
                }
            }
            catch (WebException)
            {

            }
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
                    this.userToDatabase(item["username"], item["password"], databasePath);
                }
            }
            catch (WebException)
            {

            }
        }
        public void downloadTaskData(string url, string databasePath)
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
                    this.taskToDatabase(item["_id"], item["client_id"], item["user_id"], item["startTask"], item["stopTask"], item["taskDate"], item["taskName"], item["isCompleted"], databasePath);
                }
            }
            catch (WebException)
            {

            }
        }

        public void clientToDatabase(string map, string address, string telephone, string planning, string dbPath)
        {
            var connectionString = String.Format("Data Source ={0}; Version = 3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO client (map, address, telephone, planning) VALUES (@map, @address, @telephone, @planning)";
                    cmd.Parameters.Add(new SqliteParameter("@map", map));
                    cmd.Parameters.Add(new SqliteParameter("@address", address));
                    cmd.Parameters.Add(new SqliteParameter("@telephone", telephone));
                    cmd.Parameters.Add(new SqliteParameter("@planning", planning));
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    Config.log("CLIENT INSERTED INTO DB");
                }
                conn.Close();
            }
            this.getAllClients(dbPath);
        }

        public void updateActivityInDatabase(string _id, bool isCompleted, string dbPath)
        {
            var connectionString = String.Format("Data Source ={0}; Version = 3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE activity SET isCompleted = @isCompleted WHERE _id = @_id";
                    cmd.Parameters.Add(new SqliteParameter("@_id", _id));
                    cmd.Parameters.Add(new SqliteParameter("@isCompleted", isCompleted));
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    uploadActivityData();
                }
                conn.Close();   
            }
        }

        public void updateTaskInDatabase(TaskRecord task, string dbPath)
        {
            var connectionString = String.Format("Data Source ={0}; Version = 3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE task SET isCompleted = true, startTask = @startTask, stopTask = @stopTask WHERE _id = @_id";
                    cmd.Parameters.Add(new SqliteParameter("@_id", task._id));
                    cmd.Parameters.Add(new SqliteParameter("@startTask", task.startTask));
                    cmd.Parameters.Add(new SqliteParameter("@stopTask", task.stopTask));
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    uploadTaskData();
                }
                conn.Close();
            }
        }

        public void activityToDatabase(string _id, string task_id, bool isCompleted, string activityName, string dbPath)
        {
            var connectionString = String.Format("Data Source ={0}; Version = 3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO activity (_id, task_id, isCompleted, activityName) VALUES (@_id, @task_id, @isCompleted, @activityName)";
                    cmd.Parameters.Add(new SqliteParameter("@_id", _id));
                    cmd.Parameters.Add(new SqliteParameter("@task_id", task_id));
                    cmd.Parameters.Add(new SqliteParameter("@isCompleted", isCompleted));
                    cmd.Parameters.Add(new SqliteParameter("@activityName", activityName));
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
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
                    cmd.CommandText = "INSERT INTO user (username,password) VALUES (@username, @password)";
                    cmd.Parameters.Add(new SqliteParameter("@username", username));
                    cmd.Parameters.Add(new SqliteParameter("@password", password));
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
            this.getAllUsers(dbPath);
        }

        public void deleteTaskTableInDb(string dbPath)
        {
            var connectionString = String.Format("Data Source ={0}; Version = 3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DROP TABLE task";
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public void deleteActivityTableInDb(string dbPath)
        {
            var connectionString = String.Format("Data Source ={0}; Version = 3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM activity";
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public void taskToDatabase(string _id, string client_id, string user_id, string startTask, string stopTask, string taskDate, string taskName, bool isCompleted, string dbPath)
        {
            var connectionString = String.Format("Data Source ={0}; Version = 3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO task (_id, client_id, user_id, startTask, stopTask, taskDate, taskName, isCompleted) VALUES (@_id, @client_id, @user_id, @startTask, @stopTask, @taskDate, @taskName, @isCompleted)";

                    cmd.Parameters.Add(new SqliteParameter("@_id", _id));
                    cmd.Parameters.Add(new SqliteParameter("@client_id", client_id));
                    cmd.Parameters.Add(new SqliteParameter("@user_id", user_id));
                    cmd.Parameters.Add(new SqliteParameter("@startTask", startTask));
                    cmd.Parameters.Add(new SqliteParameter("@stopTask", stopTask));
                    cmd.Parameters.Add(new SqliteParameter("@taskDate", taskDate));
                    cmd.Parameters.Add(new SqliteParameter("@taskName", taskName));
                    cmd.Parameters.Add(new SqliteParameter("@isCompleted", isCompleted));
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public ActivityRecord getActivityById(string _id, string dbPath)
        {
            ActivityRecord activity = null;
            var connectionString = String.Format("Data Source={0};Version=3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM activity WHERE (_id) = @_id";
                    cmd.Parameters.Add(new SqliteParameter("@_id", _id));
                    cmd.CommandType = CommandType.Text;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            activity = new ActivityRecord(reader);
                        }
                    }
                }
                conn.Close();
            }
            return activity;
        }

        public List<ActivityRecord> getAllActivities(string dbPath)
        {
            List<ActivityRecord> activityRecords = new List<ActivityRecord>();
            var connectionString = String.Format("Data Source={0};Version=3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM activity";
                    cmd.CommandType = CommandType.Text;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            activityRecords.Add(new ActivityRecord(reader));
                        }
                    }
                }
                conn.Close();
            }
                return activityRecords;
        }

        public List<TaskRecord> getAllTasks(string dbPath)
        {
            List<TaskRecord> taskRecords = new List<TaskRecord>();
            var connectionString = String.Format("Data Source={0};Version=3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM task";
                    cmd.CommandType = CommandType.Text;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            taskRecords.Add(new TaskRecord(reader));
                        }
                    }
                }
                conn.Close();
            }
            return taskRecords;
        }

        public UserRecord getUserByUsername(string dbPath, string username)
        {
            UserRecord user = null;
            var connectionString = String.Format("Data Source={0};Version=3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM user WHERE (username) = @username";
                    cmd.Parameters.Add(new SqliteParameter("@username", username));
                    cmd.CommandType = CommandType.Text;

                    using (var reader = cmd.ExecuteReader())
                    {
                        int i = 0;
                        while (reader.Read())
                        {
                            i++;
                            user = new UserRecord(reader);
                        }
                        if (i > 1)
                        {
                            Config.log("ERROR MORE USER RECORDS THAN EXPECTED!");
                        }
                    }
                }
                conn.Close();
            }
            return user;
        }
        public ClientRecord getClient(string dbPath, string client_id)
        {
            ClientRecord client = null;
            var connectionString = String.Format("Data Source={0};Version=3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM client WHERE (id) = @client_id";
                    cmd.Parameters.Add(new SqliteParameter("@client_id", client_id));
                    cmd.CommandType = CommandType.Text;

                    using (var reader = cmd.ExecuteReader())
                    {
                        int i = 0;
                        while (reader.Read()) 
                        {
                            i++;
                            client = new ClientRecord(reader);
                        }
                        if (i>1)
                        {
                            Config.log("ERROR MORE CLIENT RECORDS THAN EXPECTED!!");
                        }
                        if (client == null)
                        {
                            throw new ArgumentException("No such client! ", client_id);
                        }
                    }
                }
                conn.Close();
            }
            return client;
        }

        public ArrayList getActivitiesByTask(string dbPath, string task_id)
        {
            ArrayList activityRecords = new ArrayList();
            var connectionString = String.Format("Data Source={0};Version=3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM activity WHERE (task_id) = @task_id";
                    cmd.Parameters.Add(new SqliteParameter("@task_id", task_id));
                    cmd.CommandType = CommandType.Text;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            activityRecords.Add(new ActivityRecord(reader));
                        }
                    }
                }
                conn.Close();
            }
            return activityRecords;
        }

        public List<UserRecord> getAllUsers(string dbPath)
        {
            List<UserRecord> userRecords = new List<UserRecord>();
            var connectionString = String.Format("Data Source={0};Version=3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM user";
                    cmd.CommandType = CommandType.Text;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userRecords.Add(new UserRecord(reader));
                        }
                    }
                }
                conn.Close();
            }
            return userRecords;   
        }

        public List<TaskRecord> getTasksByUser(string dbPath, string user_id)
        {
            List<TaskRecord> taskRecords = new List<TaskRecord>();
            var connectionString = String.Format("Data Source={0};Version=3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM task WHERE (user_id) = @user_id AND (isCompleted) = false AND (taskDate) >= date('now', '0 day') AND (taskDate) <= date('now', '1 day') ORDER BY taskDate ASC";
                    cmd.Parameters.Add(new SqliteParameter("@user_id", user_id));
                    cmd.CommandType = CommandType.Text;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            taskRecords.Add(new TaskRecord(reader));
                        }
                    }
                }
                conn.Close();
            }
            return taskRecords;
        }

        public TaskRecord getTaskByTaskId(string dbPath, string task_id)
        {
            TaskRecord task = null;
            var connectionString = String.Format("Data Source={0};Version=3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM task WHERE (_id) = @task_id";
                    cmd.Parameters.Add(new SqliteParameter("@task_id", task_id));
                    cmd.CommandType = CommandType.Text;

                    using (var reader = cmd.ExecuteReader())
                    {
                        int i = 0;
                        while (reader.Read())
                        {
                            i++;
                            task = new TaskRecord(reader);
                        }
                        if (i > 1)
                        {
                            Config.log("ERROR MORE CLIENT RECORDS THAN EXPECTED!");
                        }
                        if (task == null)
                        {
                            throw new ArgumentException("No such task! ", task_id);
                        }
                    }
                }
                conn.Close();
            }
            return task;
        }

        public List<ClientRecord> getAllClients(string dbPath)
        {
            List<ClientRecord> clientRecords = new List<ClientRecord>();
            var connectionString = String.Format("Data Source={0};Version=3;", dbPath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM client";
                    cmd.CommandType = CommandType.Text;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clientRecords.Add(new ClientRecord(reader));
                        }
                    }
                }
                conn.Close();
            }
            return clientRecords;
        }
    }
}