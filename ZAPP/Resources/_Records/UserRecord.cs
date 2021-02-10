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
    class UserRecord
    {
        public int id;
        public string username;
        public string password;
      

        public UserRecord()
        {

        }

        public UserRecord(JsonValue record)
        {
            this.username = (string)record["username"];
            this.password = (string)record["password"];
        }

        public UserRecord(SqliteDataReader record)
        {
            this.id = (int)(Int64)record["id"];
            this.username = (string)record["username"];
            this.password = (string)record["password"];
        }

        public string getPassword()
        {
            return this.password;
        }

        public int getId()
        {
            return this.id;
        }
    }
}