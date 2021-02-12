using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZAPP
{
    class UserActivity
    {
        public string id;
        public string _id;
        public string activityName;
        public bool isCompleted;

        public UserActivity(int id, string _id, string activityName, bool isCompleted)
        {
            this.id = id.ToString();
            this._id = _id;
            this.activityName = activityName;
            this.isCompleted = isCompleted;
        }
    }
}