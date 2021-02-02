using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

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
            _database db = new _database(this);
            Thread.Sleep(1000);
            StartActivity(typeof(Home));
        }
    }
}