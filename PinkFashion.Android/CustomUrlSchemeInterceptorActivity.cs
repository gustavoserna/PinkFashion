using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using PinkFashion.AuthHelpers;

namespace PinkFashion.Droid
{
    [Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataSchemes = new[] { "com.googleusercontent.apps.669401120691-sgsi4m7e41p4ris3lobp1l5j371p88nn" },
        DataPath = "/oauth2redirect")]
    public class CustomUrlSchemeInterceptorActivity: Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var uri = new Uri(Intent.Data.ToString());

            AuthenticationState.Authenticator.OnPageLoading(uri);
            Finish();
        }
    }
}
