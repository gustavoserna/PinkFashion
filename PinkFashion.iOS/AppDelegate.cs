using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Com.OneSignal;
using FFImageLoading.Forms.Platform;
using Foundation;
using PanCardView.iOS;
using PinkFashion.AuthHelpers;
using Sharpnado.Presentation.Forms.iOS;
using UIKit;

namespace PinkFashion.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public string DeviceToken { get; set; }
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Forms.Forms.SetFlags("RadioButton_Experimental");
            SharpnadoInitializer.Initialize(enableInternalLogger: false, enableInternalDebugLogger: false);
            global::Xamarin.Forms.Forms.Init();
            Firebase.Core.App.Configure();
            CardsViewRenderer.Preserve();
            CachedImageRenderer.Init();
            LoadApplication(new App());
            OneSignal.Current.StartInit("5e431b45-a0a3-4076-b948-27070889771e").EndInit();
            global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();
            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            //DeviceToken = Regex.Replace(deviceToken.ToString(), "[^0-9a-zA-Z]+", "");
            byte[] bytes = deviceToken.ToArray<byte>();
            string[] hexArray = bytes.Select(b => b.ToString("x2")).ToArray();
            DeviceToken = string.Join(string.Empty, hexArray);

        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            var uri = new Uri(url.AbsoluteString);

            AuthenticationState.Authenticator.OnPageLoading(uri);
            return true;
        }
    }
}
