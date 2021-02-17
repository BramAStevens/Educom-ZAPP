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
    class Task
    { 
        public string _id;
        public string client_id;
        public string taskName;
        public string taskDate;
        public Task(string _id, string client_id, string taskName, string taskDate)
        {
            this._id = _id;
            this.client_id = client_id;
            this.taskName = taskName;
            this.taskDate = taskDate;
        }
    }
}