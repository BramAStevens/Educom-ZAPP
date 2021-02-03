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

namespace ZAPP // bij het aanmaken van splash activity =====> zet me in deze klasse een lijst met gebruikers --> kan je overal aanroepen
    // alles wat ik overal nodig heb kan ik hier zetten en ophalen
{
    static class Config
    {
        //  public static List<T> users { get; set; };
        private static _database db;
        private static ArrayList users;
        private static ArrayList tasks;
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

        public static ArrayList createUsers()
        {
           return db.getAllUsers(dbPath);
 
           // EG for updateTask ==> loop over activities;
        }

        // for task & activity for example ==> make sure that every task has a list of activities //


        public static void log(string text)
        {
            Console.WriteLine(text);
        }
    }
}