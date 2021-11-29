using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Chalecas.SQLiteDB;
using Chalecas.Models;
using System.Linq;
using Android;
using Android.Gms.Common;
using Xamarin.Essentials;

namespace Chalecas.Droid
{
    [Activity(Label = "H. Ayuntamiento Nanacamilpa", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public Usuario user;
        private UserDB userdb;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            //Iniciamos PlayService
            IsPlayServicesAvailable();
            //----------------------
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
            //Add Token Local
            //CONSULTAR BD
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
                //userW.token = token;
                userW.status = 0;
                userdb.AddMember(userW);
            }
            else if (RowCount == 1)
            {
                //userdb.UpdateMemberToken(user_exista[0].id, token);
            }
            else
            {
                /*
                 * if (token == null || token == "")
                {
                    FinishAffinity();
                }
                */
                //userW.token = token;
                userW.status = 0;
                userdb.AddMember(userW);
            }
            //----------------------------------
        }

        //----------Play Services
        public void IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            bool isGooglePlayServce = resultCode != ConnectionResult.Success;
            Preferences.Set("isGooglePlayServce", isGooglePlayServce);
        }
        //--------------------------

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        const int RequestLocationId = 0;

        readonly string[] LocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };

        protected override void OnStart()
        {
            base.OnStart();
            base.OnStart();

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    RequestPermissions(LocationPermissions, RequestLocationId);
                }
                else
                {
                    // Permissions already granted - display a message.
                }
            }
        }
        //-----------------------------
    }
}