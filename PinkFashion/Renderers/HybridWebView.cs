using System;
using Xamarin.Forms;

namespace PinkFashion.Renderers
{
    public class HybridWebView : WebView
    {
        private Action<string> _action;

        public static readonly BindableProperty UriProperty = BindableProperty.Create(
            propertyName: "Uri",
            returnType: typeof(string),
            declaringType: typeof(HybridWebView),
            defaultValue: default(string));

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }


        /// <summary>
        /// The bindable property for the source URL
        /// </summary>
        public static readonly BindableProperty SourceURLProperty = BindableProperty.Create("SourceURL", typeof(string), typeof(HybridWebView), string.Empty);

        /// <summary>
        /// The URL of the current page in the WebView
        /// </summary>
        public string SourceURL
        {
            get => GetValue(SourceURLProperty).ToString();
            set => SetValue(SourceURLProperty, value);
        }
        
        public void RegisterAction(Action<string> callback)
        {
            _action = callback;
        }

        public void Cleanup()
        {
            _action = null;
        }

        public void InvokeAction(string data)
        {
            if (_action == null || data == null)
            {
                return;
            }
            _action.Invoke(data);
        }
    }
}
