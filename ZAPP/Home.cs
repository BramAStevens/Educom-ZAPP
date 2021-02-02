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

    class Home : Activity
    {
        ListView listView;
        List<ListRecord> records;
        ArrayList result;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Home);
            
            _database db = new _database(this);
            result = db.getAllData(); // arraylist
            records = new List<ListRecord>();  // define new list
            foreach (_database.dataRecord value in result) // copy from results into records
            {
                ListRecord row = new ListRecord(value.id, value.code, value.description);
                records.Add(row);
            }
            // these 3 lines can be considered as a form in html
            listView = FindViewById<ListView>(Resource.Id.Overview); // link to overview in xml
            listView.Adapter = new HomeListViewAdapter(this, records); // contains all stuff inside of listView => records are added here
            listView.ItemClick += OnListItemClick; // when user clicks on list , the click is executed

        }
        protected void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            var selectedRecord = records[e.Position];
            var intent = new Intent(this, typeof(Detail));
            intent.PutExtra("ID", selectedRecord.id.ToString()); // puts info as to which record was selected in the communcication of android
            StartActivityForResult(intent, 0); // sends you to the page
        }
    }
}