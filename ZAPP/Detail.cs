using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            clientRecord client = Config.getClientByTask("1"); 
            
            FindViewById<TextView>(Resource.Id.textViewCode).Text = client.getAddress();
            FindViewById<TextView>(Resource.Id.textViewDefinition).Text = client.getTelephone();
            
            var id = Intent.GetStringExtra("ID");
            // var listRecord = result[Int32.Parse(id) - 1];
            Console.WriteLine("Got ID: " + id);
            this.makeActivityList();
        }

        private void makeActivityList()
        {
            ArrayList activityList = Config.getActivitiesByTask("1");
            List<UserActivity> records = new List<UserActivity>(); 
            foreach (activityRecord value in activityList) // copy from results into records
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