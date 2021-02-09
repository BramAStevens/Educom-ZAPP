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
       
        private static _database db;
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
    
        public static ClientRecord getClient(string clientId) // for detail page (map and address)
        {
            return db.getClient(dbPath, clientId);
        }

        public static ArrayList getActivitiesByTask(string task_id) // to show all activities per task in detail // WORKS CORRECTLY
        {

            return db.getActivitiesByTask(dbPath, task_id);
            // loop here 
          //  tasks = db.getAllactivities
            // loop over it in other functions with foreach

        }

        public static List<TaskRecord> getTasksByUser(string user_id) // WORKS CORRECTLY
        {
            return db.getTasksByUser(dbPath, user_id);
        }

        public static ArrayList getAllActivities() // is still hardcoded in _database class
        {
           ArrayList activities = db.getAllActivities(dbPath);
           return(activities);
        }

        public static ArrayList getAllUsers() // is still hardcoded in _database class
        {
           return db.getAllUsers(dbPath);
 
           // EG for updateTask ==> loop over activities;
        }

        // for task & activity for example ==> make sure that every task has a list of activities //

        public static List<ClientRecord> getAllClients() // is still hardcoded in _database class
        {
            return db.getAllClients(dbPath);
        }

        public static void log(string text)
        {
            Console.WriteLine(text);
        }
    }
}