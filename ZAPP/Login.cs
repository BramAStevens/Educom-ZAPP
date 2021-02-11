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
        public static int userId;
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
       
        protected void login(string username, string password)
        {
            string storedPassword = null;
            UserRecord user = Config.getUserByUsername(username);
            if (user == null)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetMessage("Please enter a valid username!");
                Dialog dialog = alert.Create();
                dialog.Show();
            }
            else
            {
                storedPassword = user.getPassword();
              //  userId = user.getId();
                if (password == storedPassword)
                {
             
                    goToHome();
                }
                else
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetMessage("Please enter a valid password!");
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
            }          
        }
        protected void goToHome()
        {
                StartActivity(typeof(Home));
                Config.updateActivityInDatabase("601a6551393665aec70001fd");
        }
    }
} 