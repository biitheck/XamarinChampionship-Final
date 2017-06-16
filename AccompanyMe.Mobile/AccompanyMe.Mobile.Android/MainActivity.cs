using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using TK.CustomMap.Api.Google;
using Plugin.Permissions;
using Plugin.SecureStorage;
using Gcm.Client;

namespace AccompanyMe.Mobile.Droid
{
    [Activity(Label = "AccompanyMe.Mobile", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        static MainActivity instance = null;

        public static MainActivity CurrentActivity
        {
            get
            {
                return instance;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);


            SecureStorageImplementation.StoragePassword = Build.Id;
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            LoadApplication(new App());

			// Set the current instance of MainActivity.
			instance = this;

            try
            {
                GcmClient.CheckDevice(this);
                GcmClient.CheckManifest(this);

                // Nos registramos por notificaciones push
                System.Diagnostics.Debug.WriteLine("Registrando...");
                GcmClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);
            }
            catch (Java.Net.MalformedURLException)
            {
                System.Diagnostics.Debug.WriteLine("Error creando el cliente. Verifica la URL.");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

