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
        public string _id;
        public string task_id;
        public bool isCompleted;
        public string activityName;

        public ActivityRecord()
        {

        }

        public ActivityRecord(JsonValue record)
        {
            this.task_id = (string)record["task_id"];
            this._id = (string)record["_id"];
            string s = (string)record["isCompleted"];
            if (s == "true" || s == "false")
            {
                this.isCompleted = bool.Parse(s);
            }
            else
            {
                this.isCompleted = Int32.Parse(s) == 1;
            }
            this.activityName = (string)record["activityName"];
        }

        public ActivityRecord(SqliteDataReader record)
        {
            this.id = (int)(Int64)record["id"];
            this._id = (string)record["_id"];
            this.task_id = (string)record["task_id"];
            string s = (string)record["isCompleted"];
            if (s == "true" || s == "false")
            {
                this.isCompleted = bool.Parse(s);
            } else
            {
                this.isCompleted = Int32.Parse(s) == 1;
            }
            this.activityName = (string)record["activityName"];
        }

        public bool getIsCompleted()
        {
            return this.isCompleted;
        }
    }
}