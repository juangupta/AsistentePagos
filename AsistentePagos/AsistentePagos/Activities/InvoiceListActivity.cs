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
using AsistentePagos.Core.Models;
using Felipecsl.GifImageViewLibrary;
using System.Net.Http;
using Android.Graphics.Drawables;
using Android.Graphics;
using Java.IO;
using System.IO;
using Android.Webkit;

namespace AsistentePagos.Activities
{
    [Activity(Label = "InvoiceListActivity", MainLauncher = true)]
    public class InvoiceListActivity : Activity
    {
        ImageView avatarImageView;
        ListView invoiceListView;
        GifImageView gif;
        WebView webview;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.InvoicesList);

            InitComponents();

            List<InvoiceModel> invoices = new List<InvoiceModel>()
            {
                new InvoiceModel
                {
                    description = "Descripción de prueba",
                    amount = 70000,
                    dueDate = new DateTime(2017,10,12)
                },
                new InvoiceModel
                {
                    description = "Descripción de prueba 2",
                    amount = 80000,
                    dueDate = new DateTime(2017,11,30)
                }
            };
            
            LoadAnimatedGif();

            invoiceListView.Adapter = new InvoiceListItemAdapter(this, invoices);
        }

        void LoadAnimatedGif()
        {
            //webview = view.FindViewById<WebView>(Resource.Id.webView1);
            // expects to find the 'loading_icon_small.gif' file in the 'root' of the assets folder, compiled as AndroidAsset.
            webview.LoadUrl(string.Format("file:///android_asset/gato_developer.gif"));
            // this makes it transparent so you can load it over a background
            webview.SetBackgroundColor(new Color(0, 0, 0, 0));
            webview.SetLayerType(LayerType.Software, null);
        }

        private void InitComponents()
        {
            //avatarImageView = FindViewById<ImageView>(Resource.Id.imageViewAvatar);
            invoiceListView = FindViewById<ListView>(Resource.Id.listViewInvoices);
            //gif = FindViewById<GifImageView>(Resource.Id.gifImageView);
            webview = FindViewById<WebView>(Resource.Id.webView1);
        }
    }
}