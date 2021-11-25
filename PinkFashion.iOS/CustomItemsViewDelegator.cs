using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace PinkFashion.iOS
{
    internal sealed class CustomItemsViewDelegator : ItemsViewDelegator<GroupableItemsView, CustomItemsViewController>
    {
        public CustomItemsViewDelegator(ItemsViewLayout itemsLayout, CustomItemsViewController itemsController)
            : base(itemsLayout, itemsController)
        {
        }

        /// <summary>
        /// Per default this method returns the Estimated size when its not overriden. This method is called before
        /// the rendering process and sizes the cell correctly before it is displayed in the CollectionView.
        /// Calling the base implementation of this method will throw an exception when overriding the method.
        /// </summary>
        public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout,
            NSIndexPath indexPath)
        {
            // CellForItem() is not reliable here because when the cell at indexPath is not visible it will return null.
            UICollectionViewCell cell = collectionView.CellForItem(indexPath);
            if (cell is ItemsViewCell itemsViewCell)
            {
                return itemsViewCell.Measure(); // Get the real cell size.
            }
            else
            {
                return ItemsViewLayout.EstimatedItemSize; // This is basically a fallback when CellForItem() returns null.
            }
        }
    }
}
