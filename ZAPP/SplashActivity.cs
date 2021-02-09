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

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true,
   NoHistory = true)]
    
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            string databasePath = _database.makeDatabaseName(this);
            Config.callDatabase(new _database(this, !File.Exists(databasePath)), databasePath);
            Thread.Sleep(1000);
            StartActivity(typeof(Home));
        }
    }
}