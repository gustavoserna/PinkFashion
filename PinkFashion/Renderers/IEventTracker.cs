using System;
using System.Collections.Generic;

namespace PinkFashion.Renderers
{
    public interface IEventTracker
    {
        void SendEvent(string eventId);
        void SendEvent(string eventId, string paramName, string value);
        void SendEvent(string eventId, IDictionary<string, string> parameters);
        void SendScreen(string paramName, string ParamClass);
    }
}
