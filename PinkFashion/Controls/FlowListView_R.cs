using System;
using DLToolkit.Forms.Controls;
using Xamarin.Forms;

namespace PinkFashion.Controls
{
    public class FlowListView_R : FlowListView
    {
        public static readonly BindableProperty ScrollEnabledProperty = BindableProperty.Create(nameof(ScrollEnabled), typeof(bool), typeof(FlowListView_R), false);

        public bool ScrollEnabled
        {
            get { return (bool)GetValue(ScrollEnabledProperty); }
            set { SetValue(ScrollEnabledProperty, value); }
        }
    }
}
