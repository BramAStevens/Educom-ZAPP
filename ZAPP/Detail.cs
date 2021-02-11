using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;

namespace ZAPP
{
    [Activity(Label = "Detail")]
    public class Detail : Activity
    {
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Detail);

            makeTabs();
            
            displayClientForTask(Intent.GetStringExtra("CLIENT_ID"));
            makeActivityList(Intent.GetStringExtra("TASK_ID"));
            ImageView iconClick = FindViewById<ImageView>(Resource.Id.imageClick);
            iconClick.Click += delegate
            {
             goToHome();
            };
        }

        protected void goToHome()
        {
            StartActivity(typeof(Home));
        }

        private Android.Graphics.Bitmap GetImageBitmapFromUrl(string url)
        {
            Android.Graphics.Bitmap imageBitmap = null;
            
            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        private void displayClientForTask(string clientId)
        {
            ClientRecord client = Config.getClient(clientId, this);
            
            var imageBitmap = GetImageBitmapFromUrl(client.getMap());
            
            FindViewById<TextView>(Resource.Id.textViewAddress).Text = "🗺 - " + client.getAddress();
            FindViewById<TextView>(Resource.Id.textViewTelephone).Text = "📞 - " + client.getTelephone();
            FindViewById<TextView>(Resource.Id.textViewPlanning).Text = "📝 - " + client.getPlanning();
            FindViewById<ImageView>(Resource.Id.imageViewMap).SetImageBitmap(imageBitmap);
        }

        private void makeTabs()
        {
            TabHost tabs = (TabHost)FindViewById(Resource.Id.tabhost);
            tabs.Setup();
            TabHost.TabSpec spec = tabs.NewTabSpec("tag1");
            spec.SetContent(Resource.Id.tab1);
            spec.SetIndicator("Tasks");
            tabs.AddTab(spec);
            spec = tabs.NewTabSpec("tag2");
            spec.SetContent(Resource.Id.tab2);
            spec.SetIndicator("Address");
            tabs.AddTab(spec);
            spec = tabs.NewTabSpec("tag3");
            spec.SetContent(Resource.Id.tab3);
            spec.SetIndicator("Map");
            tabs.AddTab(spec);
        }
        private void makeActivityList(string taskId)
        {
            
            ArrayList activityList = Config.getActivitiesByTask(taskId);
            List<UserActivity> records = new List<UserActivity>(); 
            foreach (ActivityRecord value in activityList) // copy from results into records
            {
                UserActivity row = new UserActivity(value.id, value.activityName, value.isCompleted);
                records.Add(row);
            }
            // these 3 lines can be considered as a form in html

            ListView listView = FindViewById<ListView>(Resource.Id.OverviewDetail); // link to overview in xml
            listView.Adapter = new UserActivityListViewAdapter(this, records); // contains all stuff inside of listView => records are added here
          //  listView.ItemClick += OnListItemClick; // when user clicks on list , the click is executed
        }
    }
}