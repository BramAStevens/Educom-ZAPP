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
            UserActivity item = items[position];
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.ActivityRow, null);
            }
            
            view.FindViewById<TextView>(Resource.Id.activityName).Text = item.activityName;
            CheckBox isChecked = view.FindViewById<CheckBox>(Resource.Id.checkBox);
            isChecked.Enabled = true;
            isChecked.Checked = item.isCompleted;
            return view;
        }
    } // end Class
} // end NameSpace