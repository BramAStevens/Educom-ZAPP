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
    class ActivityRecord
    {
        public int id;
        public string task_id;
        public string isCompleted;
        public string activityName;

        public ActivityRecord()
        {

        }

        public ActivityRecord(JsonValue record)
        {
            this.task_id = (string)record["task_id"];
            this.isCompleted = (string)record["isCompleted"];
            this.activityName = (string)record["activityName"];
        }

        public ActivityRecord(SqliteDataReader record)
        {
            this.id = (int)(Int64)record["id"];
            this.task_id = (string)record["task_id"];
            this.isCompleted = (string)record["isCompleted"];
            this.activityName = (string)record["activityName"];
        }

        public string getIsCompleted()
        {
            return this.isCompleted;
        }
    }
}