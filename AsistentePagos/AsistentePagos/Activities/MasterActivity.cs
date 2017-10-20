
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

namespace AsistentePagos.Activities
{
    [Activity(Label = "MasterActivity", MainLauncher = false)]
    public class MasterActivity : Activity
    {
        ListView masterList;
        string[] listItem = new string[] { "Consultas", "Transferencias", "Pagos", "Notificaciones" }; 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainLayout);
            // Create your application here
            ActionBar.Hide();
            masterList = (ListView) FindViewById(Resource.Id.listView1);

            ArrayAdapter adapter = new ArrayAdapter(this,Android.Resource.Layout.SimpleListItem1, listItem);
            masterList.Adapter = adapter;
            masterList.ItemClick += masterList_ItemClick;
        }

        private void masterList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this,listItem[e.Position],ToastLength.Short).Show();
        }
    }
}
