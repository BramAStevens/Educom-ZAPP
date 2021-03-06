﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
namespace ZAPP
{
    [Activity(Label = "HomeListViewAdapter")]
    class HomeListViewAdapter : BaseAdapter<Task>
    {
        List<Task> items;
        Activity context;

        public HomeListViewAdapter(Activity context, List<Task> items)
        : base()
        {
            this.context = context;
            this.items = items;
        }
        public override Task this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View view, ViewGroup parent)
        {
            var item = items[position];
           
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.ListRow, null);
            }
            ClientRecord client = Config.getClient(item.client_id, Config.getActivity(view));
            if (client != null)
            {
                view.FindViewById<TextView>(Resource.Id.Text1).Text = item.taskName;
            }
            view.FindViewById<TextView>(Resource.Id.Text2).Text = client.getAddress();
            view.FindViewById<TextView>(Resource.Id.Text3).Text = item.taskDate;
            return view;
        }
    } 
} 