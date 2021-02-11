using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;

namespace ZAPP
{
    using System.Threading;
    using Android.App;
    using Android.OS;
    using ZAPP;

    [Activity(Theme = "@style/splashTheme", MainLauncher = true, NoHistory = true)]
    
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            string databasePath = _database.makeDatabaseName(this);
            Config.callDatabase(new _database(this, !File.Exists(databasePath)), databasePath);
          //  Thread.Sleep(500);
            StartActivity(typeof(Login));
        }
    }
}