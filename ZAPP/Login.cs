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

namespace ZAPP
{

    public class Login : Activity
    {
       
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Login);

          EditText editUsername = FindViewById<EditText>(Resource.Id.editUsername);
          EditText editPassword = FindViewById<EditText>(Resource.Id.editPassword);
          Button loginButton = FindViewById<Button>(Resource.Id.loginButton);
          Button skipButton = FindViewById<Button>(Resource.Id.skipButton);

            loginButton.Click += delegate
            {
                login(editUsername.Text, editPassword.Text);
            };

            skipButton.Click += delegate
            {
                goToHome();
            };
        }
        public int userId;
        protected void login(string username, string password)
        {
            UserRecord user = Config.getUserByUsername(username);
            string storedPassword = user.getPassword();
            userId = user.getId();
            if (password == storedPassword)
            {
                goToHome();
            } else
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetMessage("Wrong login credentials!");
                Dialog dialog = alert.Create();
                dialog.Show();
            }
        }
        protected void goToHome()
        {
                StartActivity(typeof(Home));
        }
    }
} 