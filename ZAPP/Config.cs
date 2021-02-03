using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ZAPP 
{
    static class Config
    {
        //  public static List<T> users { get; set; };
        private static _database db;
        private static ArrayList users;
        private static ArrayList tasks;
        private static ArrayList clients;
        private static ArrayList activities;
        private static string dbPath;
        public static void callDatabase(_database _db, string _dbPath)
        {
            db = _db;
            dbPath = _dbPath;
        }

        public static _database getDB()
        {
            return db;
        }
        
        public static ArrayList getAllTasks()
        {
            return db.getAllTasks(dbPath);
        }

        public static ArrayList getAllActivities() // is still hardcoded in _database class
        {
            return db.getAllActivities(dbPath);
        }

        public static ArrayList getAllUsers() // is still hardcoded in _database class
        {
           return db.getAllUsers(dbPath);
 
           // EG for updateTask ==> loop over activities;
        }

        // for task & activity for example ==> make sure that every task has a list of activities //

        public static ArrayList getAllClients() // is still hardcoded in _database class
        {
            return db.getAllClients(dbPath);
        }

        public static void log(string text)
        {
            Console.WriteLine(text);
        }
    }
}