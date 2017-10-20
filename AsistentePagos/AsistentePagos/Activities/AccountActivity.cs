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
using Felipecsl.GifImageViewLibrary;
using Android.Webkit;
using AsistentePagos.Core.Models;
using AsistentePagos.Adapter;
using Android.Graphics;
using AsistentePagos.Core.Service;
using Android.Speech.Tts;
using Android.Speech;

namespace AsistentePagos.Activities
{
    [Activity(Label = "AccountActivity", MainLauncher = false)]
    public class AccountActivity : Activity, TextToSpeech.IOnInitListener
    {
        GifImageView gif;
        ListView accountListView;
        WebView avatarWebView;
        ApiService apiService;
        Response response;
        TextToSpeech tts;
        List<Account> accountList;
        private bool isRecording;
        private string textInput;

        private readonly int VOICE = 10;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Accounts);
            ActionBar.Hide();
            InitComponents();

            List<Account> accountList = new List<Account>()
           {
                new Account()
                {
                    AccountName = "Debito",
                    AccountType = "SAVING",
                    AccountNumber = "123445"
                }
        };
            
            LoadAnimatedGif();
            LoadUserAccount();
        }

        async void LoadUserAccount()
        {
            response = await apiService.Get<Account>("https://api.us.apiconnect.ibmcloud.com/",
                "/playgroundbluemix-dev/hackathon/api/", "accounts", "juagomez", "vinula");
            accountList = (List<Account>)response.Result;
            accountListView.Adapter = new AccountListAdapter(this, accountList);
            tts = new TextToSpeech(this, this);
        }


        private void InitComponents()
        {
            apiService = new ApiService();
            accountListView = FindViewById<ListView>(Resource.Id.listViewAccounts);
            avatarWebView = FindViewById<WebView>(Resource.Id.webViewAvatar);
        }

        void LoadAnimatedGif()
        {
            // expects to find the 'loading_icon_small.gif' file in the 'root' of the assets folder, compiled as AndroidAsset.
            avatarWebView.LoadUrl(string.Format("file:///android_asset/merlin.webp"));
            // this makes it transparent so you can load it over a background
            avatarWebView.SetBackgroundColor(new Color(0, 0, 0, 0));
            avatarWebView.SetLayerType(LayerType.Software, null);
        }

        #region TextToSpeech Methods
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
            string userName = "Juan";

            Speak("");
            Speak(userName + " porfa elige la cuenta con la que deseas realizar tus pagos");
            
            Speak("Por favor dime los últimos 3 numeros de la cuenta que quieres como favorito");
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
                        if (textInput.Length >= 3)
                        {
                            textInput = textInput.Substring(0, 3);
                            Account acountSelected = FindAccountFromSpeech(textInput);
                        }
                            
                    }
                    else
                        textInput = "Disculpa Juan Gabriel, no te entendí";

                }
            }

            base.OnActivityResult(requestCode, resultVal, data);
        }
        #endregion

        Account FindAccountFromSpeech(string text)
        {
            Account accountAux = null;
            string lastNumber = "";
            for (var i = 0; i < accountList.Count; i++)
            {
                accountAux = accountList[i];
                lastNumber = accountAux.AccountNumber.Substring(accountAux.AccountNumber.Length - 3, 3);
                if(string.Equals(lastNumber, text, StringComparison.OrdinalIgnoreCase))
                {
                    accountAux = accountList[i];
                    return accountAux;
                }

            }
            return null;
        }
    }

 
}