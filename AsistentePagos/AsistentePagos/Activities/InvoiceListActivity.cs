using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AsistentePagos.Core.Models;
using Felipecsl.GifImageViewLibrary;
//using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Webkit;
using AsistentePagos.Core.Service;
using Android.Speech.Tts;
using Android.Speech;
using AsistentePagos.Core.Utils;

namespace AsistentePagos.Activities
{
    [Activity(Label = "InvoiceListActivity", MainLauncher = false)]
    public class InvoiceListActivity : Activity, TextToSpeech.IOnInitListener
    {
        ImageView avatarImageView;
        ListView invoiceListView;
        GifImageView gif;
        WebView webview;
        ApiService apiService;
        Response invoicesResult;
        Response paymentResult;
        TextToSpeech tts;
        List<InvoiceModel> invoices;
        private string textInput;
        private bool isRecording;
        User user;
        private readonly int VOICE = 10;
        private readonly int VOICE2 = 20;
        SqLiteHelper database;
        string dbpath;
        InvoiceModel invoice;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.InvoicesList);
            InitComponents();
            apiService = new ApiService();
            LoadAnimatedGif();

            GetInvoices();

            database = new SqLiteHelper();
            dbpath = System.IO.Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),"ormdemo.db3");

            database.createDatabase(dbpath);

        }

        async void GetInvoices()
        {
            user = new User();
            user.Id = "beacc222da5086d625ed5e8515eba3c7";
            user.Name = "Juan Gómez";
            user.DocumentId = "1037002002";
            user.DocumentType = "CC";
            user.Password = "vinula";
            user.AccountNumber = "17232144607";
            user.AccountType = "SAVING";
            user.AccountName = "Nómina";
            user.Username = "juagomez";

            invoicesResult = await apiService.Get<InvoiceModel>("https://api.us.apiconnect.ibmcloud.com/",
                "/playgroundbluemix-dev/hackathon/api/", "invoices", user.Username, user.Password);
            invoices = (List<InvoiceModel>)invoicesResult.Result;
            invoiceListView.Adapter = new InvoiceListItemAdapter(this, invoices);
            tts = new TextToSpeech(this, this);



            //await database.insertUpdateData(user, dbpath);
            //user = database.FindUser(dbpath);
            //Toast.MakeText(this, response.Name, ToastLength.Long);

        }

        void LoadAnimatedGif()
        {
            //webview = view.FindViewById<WebView>(Resource.Id.webView1);
            // expects to find the 'loading_icon_small.gif' file in the 'root' of the assets folder, compiled as AndroidAsset.
            webview.LoadUrl(string.Format("file:///android_asset/sin_fondo_mujer.gif"));
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

        public void OnInit(OperationResult status)
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
            int invoicesCount = invoices.Count;

            Speak("Tienes " + invoicesCount + " facturas próximas a vencer. ¿Cuál factura deseas pagar?");

            //for (var i = 0; i < invoicesCount; i++)
            //{
            //    Speak(invoices[i].description + " de " + invoices[i].merchantName + " por valor de " + invoices[i].amount + " y debes pagar antes de " + invoices[i].dueDate.Month + " " +invoices[i].dueDate.Day + " " + invoices[i].dueDate.Year);
            //}

           Listen(VOICE);
            
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

        public void Listen(int requestCode)
        {
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
                StartActivityForResult(voiceIntent, requestCode);
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
                        processPayment();
                        //hablar(textInput);
                    }
                    else
                        textInput = "Disculpa, no te entendí";

                }
            }
            else if (requestCode == VOICE2)
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
                        confirmPayment();
                        //hablar(textInput);
                    }
                    else
                        textInput = "Disculpa, no te entendí";

                }
            }

            base.OnActivityResult(requestCode, resultVal, data);
        }

        void processPayment()
        {
            invoice = new InvoiceModel();
            for (var i = 0; i < invoices.Count; i++)
            {
                if (string.Equals(textInput, invoices[i].description, StringComparison.OrdinalIgnoreCase))
                {
                    invoice = invoices[i];
                    break;
                }
            }

            if (string.IsNullOrEmpty(invoice.id))
            {
                Speak(user.Name + " No pude escuchar el nombre de la factura");
                Listen(VOICE);
                return;
            }

            Speak("Vas a pagar la factura " + invoice.description + " de la empresa " + invoice.merchantName + " por un valor de" + invoice.amount + " desde tu cuenta llamada " + user.AccountName + " ¿Deseas continuar?");
            Listen(VOICE2);


            
        }

        async void confirmPayment()
        {
            if (string.Equals(textInput, "si", StringComparison.OrdinalIgnoreCase) || 
                string.Equals(textInput, "sí", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(textInput, "de acuerdo", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(textInput, "continuar", StringComparison.OrdinalIgnoreCase))
            {

                Payment payment = new Payment();
                payment.sourceAccount = user.AccountNumber;
                payment.amountPayment = invoice.amount;
                payment.description = invoice.description;
                payment.sourceTypeAccount = user.AccountType;
                payment.currency = "COP";
                payment.invoiceId = invoice.id;


                paymentResult = await apiService.Post<Payment>("https://api.us.apiconnect.ibmcloud.com/",
               "/playgroundbluemix-dev/hackathon/api/", "payments", payment, user.Username, user.Password);

                if (paymentResult.IsSuccess)
                {
                    payment = (Payment)paymentResult.Result;
                    Intent intent = new Intent(this, typeof(PaymentActivity));
                    intent.PutExtra("invoiceId", invoice.id);
                    intent.PutExtra("invoiceDesc", invoice.description);
                    intent.PutExtra("invoiceMerchant", invoice.merchantName);
                    intent.PutExtra("invoiceAmount", invoice.amount);
                    intent.PutExtra("userName", user.Name);
                    intent.PutExtra("paymentId", payment.id);
                    StartActivity(intent);

                }
                else
                {
                    Speak(paymentResult.Message);
                }

            }


        }
    }
}