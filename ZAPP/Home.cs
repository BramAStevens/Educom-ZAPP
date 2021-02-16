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
            Config.deleteTaskTableInDb();
            Config.downloadTaskData();
            List<TaskRecord> taskList = Config.getTasksByUser(Login.userId.ToString());
            taskRecords = new List<Task>(); 
            foreach (TaskRecord value in taskList)
            {
                Task row = new Task(value._id, value.client_id, value.taskName, value.taskDate);
                taskRecords.Add(row);
            }
            listView = FindViewById<ListView>(Resource.Id.Overview);
            listView.Adapter = new HomeListViewAdapter(this, taskRecords); 
            listView.ItemClick += OnListItemClick; 
        }
        protected void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            var selectedTask = taskRecords[e.Position];
            var intent = new Intent(this, typeof(Detail));
            intent.PutExtra("CLIENT_ID", selectedTask.client_id.ToString()); 
            intent.PutExtra("TASK_ID", selectedTask._id.ToString()); 
            StartActivityForResult(intent, 0); 
        }
    }
}