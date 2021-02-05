using System;
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
    [Activity(Label = "UserActivityListViewAdapter")]
    class UserActivityListViewAdapter : BaseAdapter<UserActivity>
    {
        List<UserActivity> items;
        Activity context;
        public UserActivityListViewAdapter(Activity context, List<UserActivity> items)
        : base()
        {
            this.context = context;
            this.items = items;
        }

        public override UserActivity this[int position]
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
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.ListRow, null);
            }
            view.FindViewById<TextView>(Resource.Id.Text1).Text = item.id;
            view.FindViewById<TextView>(Resource.Id.Text2).Text = item.activityName;
            view.FindViewById<TextView>(Resource.Id.Text3).Text = item.isCompleted;
            return view;
        }
    } // end Class
} // end NameSpace