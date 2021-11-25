using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;
using PinkFashion.iOS.Renderers;
using PinkFashion.Renderers;

[assembly: ExportRenderer(typeof(Entry_R), typeof(Entry_Renderer))]
namespace PinkFashion.iOS.Renderers
{
    public class Entry_Renderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                // do whatever you want to the UITextField here!
                Control.BorderStyle = UITextBorderStyle.None;
                Control.LeftView = new UIView(new CGRect(0, 0, 15, 0));
                Control.LeftViewMode = UITextFieldViewMode.Always;
                Control.RightView = new UIView(new CGRect(0, 0, 15, 0));
                Control.RightViewMode = UITextFieldViewMode.Always;

            }
        }

    }
}
