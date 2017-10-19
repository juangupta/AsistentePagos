using Android.App;
using Android.OS;
using Android.Widget;

namespace AsistentePagos
{
    [Activity(Label = "Asistente de Pagos")]
    public class MainActivity : Activity
    {


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource1
            SetContentView(Resource.Layout.Main);
            string invoiceId = Intent.GetStringExtra("invoiceId");
            Toast.MakeText(this, invoiceId, ToastLength.Long).Show();

        }

       
    }
}

