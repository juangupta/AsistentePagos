using Android.App;
using Android.OS;
using Android.Widget;
using Android.Speech.Tts;
using Android.Speech;
using Android.Runtime;
using System;
using AsistentePagos.Core.Models;
using AsistentePagos.Core.Utils;

namespace AsistentePagos
{
    [Activity(Label = "Asistente de Pagos")]
    public class PaymentActivity : Activity, TextToSpeech.IOnInitListener
    {
        TextToSpeech tts;
        User user;
        SqLiteHelper database;
        string dbpath;

        public void OnInit([GeneratedEnum] OperationResult status)
        {
            throw new NotImplementedException();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            string invoiceId = Intent.GetStringExtra("invoiceId");
            Toast.MakeText(this, invoiceId, ToastLength.Long).Show();
            user = new User();
            user.Id = "beacc222da5086d625ed5e8515eba3c7";
            user.Name = "Juan Gómez";
            user.DocumentId = "1037002002";
            user.DocumentType = "CC";
            user.Password = "vinula";
            user.AccountNumer = "12094368771";
            user.AccountType = "SAVING";
            //tts = new TextToSpeech(this, this);

        }

       
    }
}

