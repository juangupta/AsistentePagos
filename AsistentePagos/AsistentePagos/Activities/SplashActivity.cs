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
using AsistentePagos.Core.Utils;
using AsistentePagos.Core.Models;

namespace AsistentePagos.Activities
{
    [Activity(Theme = "@style/Theme.Splash", //Indicates the theme to use for this activity
             MainLauncher = true, //Set it as boot activity
             NoHistory = true)]
    public class SplashActivity : Activity
    {
        SqLiteHelper database;
        User user;
        string dbpath;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            System.Threading.Thread.Sleep(5000); //Let's wait awhile...

            database = new SqLiteHelper();
            dbpath = System.IO.Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ormdemo.db3");

            user = database.FindUser(dbpath);

            if(user != null)
            {
                this.StartActivity(typeof(LoginVoice));
            }
            else
            {
                this.StartActivity(typeof(LoginActivity));
            }

            

        }
    }
}