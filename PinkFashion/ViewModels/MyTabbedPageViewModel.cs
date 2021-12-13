using System;
using System.Collections.Generic;
using System.Text;

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
        }
    }
}
