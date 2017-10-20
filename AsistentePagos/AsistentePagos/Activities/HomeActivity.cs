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
//using Android.Graphics.Drawables;
using Android.Graphics;
using Java.IO;
using System.IO;
using Android.Webkit;
using AsistentePagos.Core.Service;
using AsistentePagos.Core.Models;
using Android.Speech.Tts;
using Android.Speech;
using Android.Content;
using AsistentePagos.Core.Utils;
using System.Threading.Tasks;

namespace AsistentePagos.Activities
{
    [Activity(Label = "Asistente de Pagos", MainLauncher = false)]
    public class HomeActivity : Activity, TextToSpeech.IOnInitListener
    {
        private string textInput;
        private bool isRecording;
        private readonly int VOICE = 10;
        GifImageView gif;
        WebView webview;
        TextToSpeech tts;
        SqLiteHelper database;
        string dbpath;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            // Set our view from the "home" layout resource
            SetContentView(Resource.Layout.Home);
            InitComponents();

            // Cargamos el avatar
            

            database = new SqLiteHelper();
            dbpath = System.IO.Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ormdemo.db3");

            ImageView imageMicrophone = FindViewById<ImageView>(Resource.Id.imageViewMicrophone);
            imageMicrophone.Click += delegate
            {
                Hablar();
            };
            // Empezamos a saludar al usuario
            Hablar();
        }

        public void Hablar()
        {
            //Task.Delay(10).Wait();
            tts = new TextToSpeech(this, this);
        }

        private void InitComponents()
        {
            //avatarImageView = FindViewById<ImageView>(Resource.Id.imageViewAvatar);
            //invoiceListView = FindViewById<ListView>(Resource.Id.listViewInvoices);
            //gif = FindViewById<GifImageView>(Resource.Id.gifImageView);
            webview = FindViewById<WebView>(Resource.Id.webView1);
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

        public void OnInit(OperationResult status)
        {
            LoadAnimatedGif();
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

        public async void Speech()
        {
            
            // Consultamos el nombre de la DB
            //var response = database.FindUser(dbpath);
            var response = database.FindUser(dbpath);
            //Toast.MakeText(this, response.Name, ToastLength.Long);
            string userName = response.Name;

            // Saludamos al usuario
            Speak("");
            Speak("Hola" + userName + ", Bienvenido ha tu Asistente de Pagos, tienes facturas pendientes por pagar        ¿Quieres consultarlas?");
            //Speak("¿Quieres consultarlas?");
            Listen();
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
                while (tts.IsSpeaking)
                {
                    //TODO
                }
            }
        }

        public void Listen()
        {
            //while (tts.IsSpeaking)
            //{
            //    //TODO
            //}

            isRecording = false;
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec != "android.hardware.microphone")
            {
                // no microphone, no recording. Disable the button and output an alert
                Toast.MakeText(this, "No microphone", ToastLength.Long).Show();
            }
            isRecording = !isRecording;
            if (isRecording)
            {
                // create the intent and start the activity
                var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

                // put a message on the modal dialog
                voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Escuchando...");

                // if there is more then 1.5s of silence, consider the speech over
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

                // you can specify other languages recognised here, for example
                // voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.German);
                // if you wish it to recognise the default Locale language and German
                // if you do use another locale, regional dialects may not be recognised very well

                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                StartActivityForResult(voiceIntent, VOICE);
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            if (requestCode == VOICE)
            {
                if (resultVal == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        textInput = matches[0];

                        // limit the output to 500 characters
                        if (textInput.Length > 500)
                            textInput = textInput.Substring(0, 500);
                        // Consultamos las facturas
                        invokeInvoices();
                    }
                    else
                        textInput = "Disculpa, no te entendí";
                }
            }
            base.OnActivityResult(requestCode, resultVal, data);
        }

        void invokeInvoices()
        {
            // Llamamos el activity InvoiceListActivity
            if (string.Equals(textInput, "Sí", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(textInput, "Sí", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(textInput, "de acuerdo", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(textInput, "continuar", StringComparison.OrdinalIgnoreCase))
            {
                Intent intent = new Intent(this, typeof(InvoiceListActivity));
                StartActivity(intent);
            }
        }
    }
}