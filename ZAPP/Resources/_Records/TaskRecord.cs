using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;

namespace ZAPP
{
    class TaskRecord
    {
        public int id;
        public string _id;
        public string client_id;
        public string user_id;
        public string startTask;
        public string stopTask;
        public string taskDate;
        public string taskName;
        public bool isCompleted;

        public TaskRecord()
        {

        }

        public TaskRecord(JsonValue record)
        {
            this._id = (string)record["_id"];
            this.client_id = (string)record["client_id"];
            this.user_id = (string)record["user_id"];
            this.startTask = (string)record["startTask"];
            this.stopTask = (string)record["stopTask"];
            this.taskName = (string)record["taskName"];
            this.taskDate = (string)record["taskDate"];
            this.isCompleted = (bool)record["isCompleted"];
        }

        public TaskRecord(SqliteDataReader record)
        {
            this.id = (int)(Int64)record["id"];
            this._id = (string)record["_id"];
            this.client_id = (string)record["client_id"];
            this.user_id = (string)record["user_id"];
            this.startTask = (string)record["startTask"];
            this.stopTask = (string)record["stopTask"];
            this.taskName = (string)record["taskName"];
            this.taskDate = (string)record["taskDate"];
            this.isCompleted = (bool)record["isCompleted"];
        }
        public string get_id()
        {
            return this._id;
        }
    }
}