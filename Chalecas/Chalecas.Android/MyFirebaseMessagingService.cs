using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Util;
using Chalecas.Models;
using Chalecas.SQLiteDB;
using Firebase.Messaging;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Chalecas.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        private UserDB userdb;
        public Usuario user;
        AndroidNotificationManager androidNotification = new AndroidNotificationManager();

        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: "+ message.From);
            Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
            androidNotification.CrearNotificacionLocal(message.GetNotification().Title, message.GetNotification().Body);

            MessagingCenter.Send<string>(message.GetNotification().Title, "MensajeFirebase");
        }

        public override void OnNewToken(string token)
        {
            base.OnNewToken(token);

            Preferences.Set("TokenFirebase", token);
            sedRegisterToken(token);
        }
        public void sedRegisterToken(string token)
        {
            //Tu código para registrar el token a tu servidor y base de datos
            userdb = new UserDB();
            var userW = new Usuario();
            var user_exista = userdb.GetMembers().ToList();
            var user_exist = userdb.GetMembers();
            int RowCount = 0;
            int usercount = user_exist.Count();
            RowCount = Convert.ToInt32(usercount);
            if (RowCount > 1)
            {
                userdb.DeleteMembers();
                userW.token = token;
                userW.status = 0;
                userdb.AddMember(userW);
            }
            else if (RowCount == 1)
            {
                userdb.UpdateMemberToken(user_exista[0].id, token);
            }
            else
            {
                userW.token = token;
                userW.status = 0;
                userdb.AddMember(userW);
            }
            //------------------------------
        }

    }
}