using System;
using Com.OneSignal;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using PanCardView.Droid;
using Sharpnado.Presentation.Forms.Droid;
using Plugin.CurrentActivity;
using Firebase.Analytics;
using FFImageLoading.Forms.Platform;

namespace PinkFashion.Droid
{
    [Activity(Label = "PinkFashion", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    //Invite App Link
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
                  DataScheme = "https",
                  DataHost = "pinkfashionstore.com",
                  DataPathPrefix = "/hello",
                  AutoVerify = true,
                  Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
                  DataScheme = "http",
                  DataHost = "pinkfashionstore.com",
                  AutoVerify = true,
                  DataPathPrefix = "/hello",
                  Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        /**
         * The FirebaseAnalytics used to record screen views.
         */
        // [START declare_analytics]
        //FirebaseAnalytics firebaseAnalytics;
        // [END declare_analytics]

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            CachedImageRenderer.Init(true);
            //Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Forms.Forms.SetFlags("RadioButton_Experimental");
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            // [START shared_app_measurement]
            // Obtain the FirebaseAnalytics instance.
            //firebaseAnalytics = FirebaseAnalytics.GetInstance(this);
            // [END shared_app_measurement]

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            SharpnadoInitializer.Initialize(enableInternalLogger: false, enableInternalDebugLogger: false);
            OneSignal.Current.StartInit("5e431b45-a0a3-4076-b948-27070889771e").EndInit();
            
            OneSignal.Current.RegisterForPushNotifications();
            CardsViewRenderer.Preserve();
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        
    }
}