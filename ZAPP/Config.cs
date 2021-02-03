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

namespace ZAPP // bij het aanmaken van splash activity =====> zet me in deze klasse een lijst met gebruikers --> kan je overal aanroepen
    // alles wat ik overal nodig heb kan ik hier zetten en ophalen
{
    static class Config
    {
        //  public static List<T> users { get; set; };
        private static _database db;

        //  private static List<users> users;

        public static void callDatabase(_database _db)
        {
            db = _db;
        }

        public static _database getDB()
        {
            return db;
        }
        
        public static void getAllTasks()
        {

        }

        public static void createUsers()
        {
           // users = db.getAllUsers(); user list is then filled and u can call this everywhere else in the application.
           // to call this ==> Config.createUsers;
           // EG for updateTask ==> loop over activities;
        }

        // for task & activity for example ==> make sure that every task has a list of activities //


        public static void log(string text)
        {
            Console.WriteLine(text);
        }
    }
}