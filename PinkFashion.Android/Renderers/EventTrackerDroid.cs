using System.Collections.Generic;
using PinkFashion.Droid.Renderers;
using Xamarin.Forms;
using Firebase.Analytics;
using Android.OS;
using Plugin.CurrentActivity;
using PinkFashion.Renderers;

[assembly: Dependency(typeof(EventTrackerDroid))]
namespace PinkFashion.Droid.Renderers
{
    public class EventTrackerDroid : IEventTracker
    {
        public void SendEvent(string eventId)
        {
            SendEvent(eventId, null);
        }

        public void SendEvent(string eventId, string paramName, string value)
        {
            SendEvent(eventId, new Dictionary<string, string>
            {
                {paramName, value}
            });
        }

        public void SendEvent(string eventId, IDictionary<string, string> parameters)
        {
            
          var firebaseAnalytics = FirebaseAnalytics.GetInstance(CrossCurrentActivity.Current.AppContext);

            if (parameters == null)
            {
                firebaseAnalytics.LogEvent(eventId, null);
                return;
            }

            var bundle = new Bundle();
            foreach (var param in parameters)
            {
                bundle.PutString(param.Key, param.Value);
            }

            firebaseAnalytics.LogEvent(eventId, bundle);
            
        }

        public void SendScreen(string ScreenName, string ScreenClass)
        {
            
            var firebaseAnalytics = FirebaseAnalytics.GetInstance(CrossCurrentActivity.Current.AppContext);

            var bundle = new Bundle();
            bundle.PutString(FirebaseAnalytics.Param.ScreenName, ScreenName);
            bundle.PutString(FirebaseAnalytics.Param.ScreenClass, ScreenClass);

            firebaseAnalytics.LogEvent(FirebaseAnalytics.Event.ScreenView, bundle);
        }

    }
}
