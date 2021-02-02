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

namespace ZAPP
{
    [Activity(Label = "Detail")]
    public class Detail : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Detail);

            _database db = new _database(this);
            ArrayList result = db.getAllData(); // arraylist
            List<ListRecord> records = new List<ListRecord>();  // define new list
            foreach (_database.dataRecord value in result) // copy from results into records
            {
                ListRecord row = new ListRecord(value.id, value.code, value.description);
                records.Add(row);
            }
            var id = Intent.GetStringExtra("ID");
            var listRecord = records[Int32.Parse(id)];
            FindViewById<TextView>(Resource.Id.textViewId).Text = listRecord.id;
            FindViewById<TextView>(Resource.Id.textViewCode).Text = listRecord.code;
            FindViewById<TextView>(Resource.Id.textViewDefinition).Text = listRecord.description;

            Console.WriteLine("Got ID: " + id);
        }
    }
}