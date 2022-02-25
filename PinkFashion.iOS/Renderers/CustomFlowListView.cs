using System;
using PinkFashion.Controls;
using PinkFashion.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FlowListView_R), typeof(CustomFlowListView))]
namespace PinkFashion.iOS.Renderers
{
    public class CustomFlowListView : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            var flowListview = (FlowListView_R)Element;
            if (flowListview == null)
            {
                System.Diagnostics.Debug.WriteLine("flowlistview element null");
                return;
            }

            if (flowListview.ScrollEnabled)
            {
                Control.ScrollEnabled = true;
            }
            else
            {
                Control.ScrollEnabled = false;
            }

            if (Control != null)
            {
                Control.ShowsVerticalScrollIndicator = false;
            }
        }
    }
}
