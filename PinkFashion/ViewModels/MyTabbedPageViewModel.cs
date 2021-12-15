using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PinkFashion.ViewModels
{
    class MyTabbedPageViewModel : BaseViewModel
    {
        int _badge = 0;
        public int Badge
        {
            get { return _badge; }
            set { SetProperty(ref _badge, value); }
        }

        public MyTabbedPageViewModel()
        {
            _badge = 0;
            MessagingCenter.Subscribe<CarritoViewModel, int>(this, "Badge", (sender, arg) =>
            {
                if (!(arg == -1 && Badge <= 0))
                {
                    Badge = Badge + (arg);
                }
            });
            MessagingCenter.Subscribe<ProductoViewModel, int>(this, "Badge", (sender, arg) =>
            {
                Badge += arg;
            });
        }
    }
}
