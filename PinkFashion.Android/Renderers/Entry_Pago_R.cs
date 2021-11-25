using System;
using Android.Content;
using PinkFashion.Droid.Renderers;
using PinkFashion.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry_Pago), typeof(Entry_Pago_R))]
namespace PinkFashion.Droid.Renderers
{
    public class Entry_Pago_R : EntryRenderer
    {
        public Entry_Pago_R(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.ParseColor("#ffffff"));
                Control.SetPadding(0, 0, 0, 0);
                //Control.SetHeight(150);
            }
        }
    }
}

