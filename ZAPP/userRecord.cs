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
    class userRecord
    {
        public int id;
        public string code;
        public string description;

        public userRecord()
        {

        }

        public userRecord(JsonValue record)
        {
            this.code = (string)record["username"];
            this.description = (string)record["password"];
        }

        public userRecord(SqliteDataReader record)
        {
            this.id = (int)(Int64)record["id"];
            this.code = (string)record["username"];
            this.description = (string)record["password"];
        }
    }
}