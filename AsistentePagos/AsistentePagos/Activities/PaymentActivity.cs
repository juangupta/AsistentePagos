using Android.App;
using Android.OS;
using Android.Widget;
using Android.Speech.Tts;
using Android.Speech;
using Android.Runtime;
using System;
using AsistentePagos.Core.Models;
using AsistentePagos.Core.Utils;

using Android.Speech.Tts;
using Android.Speech;
using Android.Webkit;
using Android.Graphics;
using Android.Views;

namespace AsistentePagos
{
    [Activity(Label = "Asistente de Pagos")]
    public class PaymentActivity : Activity, TextToSpeech.IOnInitListener
    {
        TextToSpeech tts;
        User user;
        SqLiteHelper database;
        WebView webview;
        string dbpath;

        public void OnInit([GeneratedEnum] OperationResult status)
        {
            //Verificamos que nuestra variable haya sido inicializada correctamente
            //Toast.MakeText(this, "OnInit", ToastLength.Long).Show();
            if (status.Equals(OperationResult.Success))
            {
                //Asignamos el idima a utlizar en este caso seleccionamos el idioma por default del sistema
                tts.SetLanguage(Java.Util.Locale.Default);
                //Enviamos una notificacion de que se activo nuestro TTS correctamente
                //Toast.MakeText(this, "TTS Activado Correctamente", ToastLength.Long).Show();
            }
            else
            {
                //Enviamos una notificacion si no se activo nuestro TTS correctamente
                Toast.MakeText(this, "Error al activar TTS", ToastLength.Long).Show();
            }
            Speech();
        }

        public void Speech()
        {

            string userName = Intent.GetStringExtra("userName");
            // Saludamos al usuario
            string[] speaks = { "Tu pago fue efectuado exitosamente", "Actualmente tienes un crédito pre aprobado", "¿estás interesado en adquirirlo?" };

            for (var i = 0; i < speaks.Length; i++)
            {
                Speak(speaks[i]);
            }
            //Listen(VOICE);

        }

        public void Speak(string text)
        {
            if (tts != null)
            {
                //En una variable tipo string depositamos el contenido de nuestro textbox
                //String text = "Hola Juan Gabriel, que quieres hacer hoy?";
                //Verificamos que nuestra variable de texto no sea nula para evitar una excepcion
                if (text != null)
                {
                    //Verificamos que nuestro telefono no este usando la funcion de dictado
                    if (!tts.IsSpeaking)
                    {
                        //mandamos a llamar la funcion de dictado que nos pedida tres parametros
                        //1 Texto a dictar
                        //2 Modo
                        //3 Diccionario de parametros el cual ira vacio en esta ocacion
                        tts.Speak(text, QueueMode.Flush, null);
                    }
                }
                //while (tts.IsSpeaking)
                //{
                //    //TODO
                //}
            }


        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource1
            SetContentView(Resource.Layout.Main);
            InitComponents();
            LoadAnimatedGif();
            ActionBar.Hide();
            string paymentId = Intent.GetStringExtra("paymentId");
            Toast.MakeText(this, paymentId, ToastLength.Long).Show();
            tts = new TextToSpeech(this, this);

        }

        private void InitComponents()
        {
            //avatarImageView = FindViewById<ImageView>(Resource.Id.imageViewAvatar);
            //invoiceListView = FindViewById<ListView>(Resource.Id.listViewInvoices);
            //gif = FindViewById<GifImageView>(Resource.Id.gifImageView);
            webview = FindViewById<WebView>(Resource.Id.webViewAvatar);
        }

        void LoadAnimatedGif()
        {
            //webview = view.FindViewById<WebView>(Resource.Id.webView1);
            // expects to find the 'loading_icon_small.gif' file in the 'root' of the assets folder, compiled as AndroidAsset.
            webview.LoadUrl(string.Format("file:///android_asset/merlin.webp"));
            // this makes it transparent so you can load it over a background
            webview.SetBackgroundColor(new Color(0, 0, 0, 0));
            webview.SetLayerType(LayerType.Software, null);

        }


    }
}

