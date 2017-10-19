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
using AsistentePagos.Core.Service;
using AsistentePagos.Core.Models;
using AsistentePagos.Core.Utils;


namespace AsistentePagos.Activities
{
    [Activity(Label = "LoginActivity", MainLauncher = true)]
    public class LoginActivity : Activity
    {
        private EditText usernameInput, passwordInput;
        //private Button button;
        ApiService apiService;
        Response usersResult;
        List<User> users;
        SqLiteHelper database;
        User user;
        string dbpath;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginUser);
            InitComponents();
            apiService = new ApiService();

            Button button = FindViewById<Button>(Resource.Id.BtnAceptar);
            button.Click += OnLogin;


            // Create your application here
        }

        public void OnLogin(object sender, EventArgs e)
        {
           

            GetUsers();
            //titleLabel.Text = "Aqui vamos";
            // ObtenerToken();
            var intent = new Intent(this, typeof(VoiceActivity));
            intent.PutExtra("userName", user.Name);
            StartActivity(intent);


        }

        async void GetUsers()
        {
            var strFilter = "?filter[where][username]="+ usernameInput.Text;
            usersResult = await apiService.Get<User>("https://api.us.apiconnect.ibmcloud.com/",
                "/playgroundbluemix-dev/hackathon/api/", "users",strFilter);

            users = (List<User>)usersResult.Result;

            user = users[0];
            user.PassUser = passwordInput.Text;

            database = new SqLiteHelper();
            dbpath = System.IO.Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ormdemo.db3");
            database.createDatabase(dbpath);

            await database.insertUpdateData(user, dbpath);


            /*var intent = new Intent(this, typeof(ProtectedActivity));
            StartActivity(intent);*/


            User user1 = new User();
            user1.Id = "1";
            user1.Name = "Yefry";
            user1.DocumentId = "123445"; 

            //await database.insertUpdateData(user, dbpath);
            //var response = database.FindUser(dbpath);
            //Toast.MakeText(this, response.Name, ToastLength.Long);

        }

        private void InitComponents()
        {
            
            usernameInput = FindViewById<EditText>(Resource.Id.usernameInput);
            passwordInput = FindViewById<EditText>(Resource.Id.passwordInput);
        }
    }
}