using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace PinkFashion.iOS
{
    internal sealed class CustomItemsViewController : GroupableItemsViewController<GroupableItemsView>
    {
        // protected override Boolean IsHorizontal => false;

        public CustomItemsViewController(GroupableItemsView itemsView, ItemsViewLayout itemsLayout)
            : base(itemsView, itemsLayout)
        {
        }

        protected override UICollectionViewDelegateFlowLayout CreateDelegator()
        {
            return new CustomItemsViewDelegator(ItemsViewLayout, this as CustomItemsViewController);
        }
    }
}
