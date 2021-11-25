using System;
using PinkFashion.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CollectionView), typeof(CustomCollectionViewRenderer))]

namespace PinkFashion.iOS
{
    internal sealed class CustomCollectionViewRenderer : GroupableItemsViewRenderer<GroupableItemsView,
    CustomItemsViewController>
    {
        protected override CustomItemsViewController CreateController(
            GroupableItemsView itemsView,
            ItemsViewLayout itemsLayout
        )
        {
            return new CustomItemsViewController(itemsView, itemsLayout);
        }

        // protected override ItemsViewLayout SelectLayout()
        // {
        //     LinearItemsLayout itemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical);
        //     return new ListViewLayout(itemsLayout, ItemSizingStrategy.MeasureAllItems);
        // }
    }
}
