using System;
using Android.Content;
using PinkFashion.Droid.Renderers;
using PinkFashion.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry_R), typeof(Entry_Renderer))]

namespace PinkFashion.Droid.Renderers
{
    public class Entry_Renderer : EntryRenderer
    {
        public Entry_Renderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.ParseColor("#ffffff"));
                Control.SetPadding(20, 20, 20, 0);
            }
        }
    }
}
