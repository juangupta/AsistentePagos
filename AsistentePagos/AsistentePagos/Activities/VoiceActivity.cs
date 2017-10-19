
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech.Tts;
using Android.Speech;
using Android.Views;
using Android.Widget;


namespace AsistentePagos.Activities
{
    [Activity(Label = "VoiceActivity", MainLauncher = false)]
    public class VoiceActivity : Activity, TextToSpeech.IOnInitListener

    {
        TextToSpeech tts;
        private bool isRecording;
        private readonly int VOICE1 = 10;
        private readonly int VOICE2 = 20;
        private string textInput;
        string userName;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //tts = new TextToSpeech(this, this);
            // Create your application here
            InitSpeech(); 
        }

        protected override void OnResume()
        {
            base.OnResume();

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

        void InitSpeech(){
            tts = new TextToSpeech(this, this);
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

        public void Speech()
        {
            
            userName = "Anibal";

            string[] speaks = {" ", "Hola"+userName+"para autenticar repite las siguientes oraciones", "La vaca en la torre es de color rojo"};

            for (var i = 0; i < speaks.Length; i++)
            {
                Speak(speaks[i]);
            }
                
            Listen(VOICE1);

        }

        public void Listen(int VOICE)
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
            if (requestCode == VOICE1)
            {
                if (resultVal == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        textInput = matches[0];

                        // limit the output to 500 characters
                        if (textInput.Length > 30)
                            textInput = textInput.Substring(0, 30);
                        
                        Speak("El gato se tomo la leche del abuelo");
                    }
                    else
                        Speak("En este momento el sistema no se encuentra disponible");

                }
                Listen(VOICE2);
            }

            if (requestCode == VOICE2)
            {
                if (resultVal == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        textInput = matches[0];

                        // limit the output to 500 characters
                        if (textInput.Length > 30)
                            textInput = textInput.Substring(0, 30);

                        Speak("La validación de tu voz ha sido exitosamente");
                        CallAccountActivity();
                    }
                    else
                        Speak("No se logro realizar tu validación de voz");

                }
            }

            base.OnActivityResult(requestCode, resultVal, data);
        }

        private void CallAccountActivity()
        {
            Intent intent = new Intent(this, typeof(AccountActivity));
            Intent.PutExtra("userName", userName);
            StartActivity(intent);
        }
    }
}
