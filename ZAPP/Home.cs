using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content.Res;

namespace ZAPP
{
    class Home : Activity
    {
        ListView listView;
        List<Task> taskRecords;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Home);

            List<TaskRecord> taskList = Config.getTasksByUser("1");
           
            taskRecords = new List<Task>();  // define new list

            foreach (TaskRecord value in taskList) // copy from results into records
            {
                Task row = new Task(value.id, value.client_id, value.taskName, value.taskDate);
                taskRecords.Add(row);
            }
           
           // these 3 lines can be considered as a form in html
            listView = FindViewById<ListView>(Resource.Id.Overview); // link to overview in xml
            listView.Adapter = new HomeListViewAdapter(this, taskRecords); // contains all stuff inside of listView => records are added here
            listView.ItemClick += OnListItemClick; // when user clicks on list , the click is executed

        }
        protected void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            var selectedTask = taskRecords[e.Position];
            var intent = new Intent(this, typeof(Detail));
            intent.PutExtra("CLIENT_ID", selectedTask.client_id.ToString()); // puts info as to which record was selected in the communcication of android
            intent.PutExtra("TASK_ID", selectedTask.id.ToString()); 
            StartActivityForResult(intent, 0); // sends you to the page
        }
    }
}