using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Scanner.Windows.Common
{
    public class GridViewSelectedItemBehavior : Behavior<GridView>
    {
        public IList<object> SelectedItems
        {
            get { return (IList<object>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", 
            typeof(IList<object>), 
            typeof(GridViewSelectedItemBehavior), 
            new PropertyMetadata(0));

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SelectionChanged += OnGridViewSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (this.AssociatedObject != null)
                this.AssociatedObject.SelectionChanged -= OnGridViewSelectionChanged;
        }

        private void OnGridViewSelectionChanged(object sender,
            SelectionChangedEventArgs e)
        {
            GridView grid = sender as GridView;

            if (grid != null)
            {
                this.SelectedItems = grid.SelectedItems;
            }
        }
    }
}
