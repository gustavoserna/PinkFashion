using Xamarin.Forms;

namespace PinkFashion.Controls
{
    public class TabbedPage_R : Xamarin.Forms.TabbedPage
    {
        public static readonly BindableProperty BadgeProperty =
            BindableProperty.Create(nameof(Badge), typeof(int), typeof(TabbedPage_R), 0);

        public int Badge
        {
            get
            {
                return (int)GetValue(BadgeProperty);
            }
            set
            {
                base.SetValue(BadgeProperty, value);
            }
        }
    }
}