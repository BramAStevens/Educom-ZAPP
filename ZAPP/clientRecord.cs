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
    class clientRecord
    {
        public int id;
        public string map;
        public string address;
        public string telephone;
        public string planning;

        public clientRecord()
        {

        }

        public clientRecord(JsonValue record)
        {
            this.map = (string)record["map"];
            this.address = (string)record["address"];
            this.telephone = (string)record["telephone"];
            this.planning = (string)record["planning"];
        }

        public clientRecord(SqliteDataReader record)
        {
            this.id = (int)(Int64)record["id"];
            this.map = (string)record["map"];
            this.address = (string)record["address"];
            this.telephone = (string)record["telephone"];
            this.planning = (string)record["planning"];
        }
    }
}