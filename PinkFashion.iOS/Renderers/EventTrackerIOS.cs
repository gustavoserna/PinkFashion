using System;
using System.Collections.Generic;
using Firebase.Analytics;
using Firebase.Core;
using Foundation;
using PinkFashion.iOS.Renderers;
using PinkFashion.Renderers;
using Xamarin.Forms;

[assembly: Dependency(typeof(EventTrackerIOS))]
namespace PinkFashion.iOS.Renderers
{
    public class EventTrackerIOS : IEventTracker
    {
        public void SendEvent(string eventId)
        {
            SendEvent(eventId, (IDictionary<string, string>)null);
        }

        public void SendEvent(string eventId, string paramName, string value)
        {
            SendEvent(eventId, new Dictionary<string, string>
            {
                { paramName, value }
            });
        }

        public void SendEvent(string eventId, IDictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                Analytics.LogEvent(eventId, (Dictionary<object, object>)null);
                return;
            }

            var keys = new List<NSString>();
            var values = new List<NSString>();
            foreach (var item in parameters)
            {
                keys.Add(new NSString(item.Key));
                values.Add(new NSString(item.Value));
            }

            var parametersDictionary =
                NSDictionary<NSString, NSObject>.FromObjectsAndKeys(values.ToArray(), keys.ToArray(), keys.Count);
            Analytics.LogEvent(eventId, parametersDictionary);
        }

        public void SendScreen(string ScreenName, string ScreenClass)
        {

            Analytics.SetScreenNameAndClass(ScreenName, ScreenClass);
        }
    }
}
