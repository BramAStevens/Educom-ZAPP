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
    public class ClientRecord
    {
        public int id;
        public string map;
        public string address;
        public string telephone;
        public string planning;

        public ClientRecord()
        {

        }

        public ClientRecord(JsonValue record)
        {
            this.map = (string)record["map"];
            this.address = (string)record["address"];
            this.telephone = (string)record["telephone"];
            this.planning = (string)record["planning"];
        }

        public ClientRecord(SqliteDataReader record)
        {
            this.id = (int)(Int64)record["id"];
            this.map = (string)record["map"];
            this.address = (string)record["address"];
            this.telephone = (string)record["telephone"];
            this.planning = (string)record["planning"];
        }
        public string getAddress()
        {
            return this.address;
        }
        public string getAddress(int id)
        {
            return this.address;
        }

        public string getTelephone()
        {
            return this.telephone;
        }

        public string getPlanning()
        {
            return this.planning;
        }

        public string getMap()
        {
            return this.map;
        }
    }
}