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
        
            ArrayList result = Config.getAllTasks(); // arraylist
            List<ListRecord> records = new List<ListRecord>();  // define new list
            foreach (taskRecord value in result) // copy from results into records
            {
                ListRecord row = new ListRecord(value.id, value.taskName, value.taskDate);
                records.Add(row);
            }
            var id = Intent.GetStringExtra("ID");
            var listRecord = records[Int32.Parse(id)-1];
            FindViewById<TextView>(Resource.Id.textViewId).Text = listRecord.id;
            FindViewById<TextView>(Resource.Id.textViewCode).Text = listRecord.taskName;
            FindViewById<TextView>(Resource.Id.textViewDefinition).Text = listRecord.taskDate;

            Console.WriteLine("Got ID: " + id);
        }
    }
}