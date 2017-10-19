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
            Speak("Tu pago fue efectuado exitosamente");

            Speak("Actualmente tienes un crédito pre aprobado, ¿estás interesado en adquirirlo?");

            //for (var i = 0; i < invoicesCount; i++)
            //{
            //    Speak(invoices[i].description + " de " + invoices[i].merchantName + " por valor de " + invoices[i].amount + " y debes pagar antes de " + invoices[i].dueDate.Month + " " +invoices[i].dueDate.Day + " " + invoices[i].dueDate.Year);
            //}

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
                while (tts.IsSpeaking)
                {
                    //TODO
                }
            }


        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource1
            SetContentView(Resource.Layout.Main);
            string paymentId = Intent.GetStringExtra("paymentId");
            Toast.MakeText(this, paymentId, ToastLength.Long).Show();
            tts = new TextToSpeech(this, this);

        }

       
    }
}

