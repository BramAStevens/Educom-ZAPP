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
        public string id;
        public string client_id;
        public string user_id;
        public string startTask;
        public string stopTask;
        public string taskName;
        public string taskDate;
        public string isCompleted;

        public Task(int id, string taskName, string taskDate)
        {
            this.id = id.ToString();
            this.taskName = taskName;
            this.taskDate = taskDate;
            
        }
    }
}