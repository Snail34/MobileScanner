using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Scanner.Models
{
    public class Category
    {
        public Category()
        {
            this.CategoryDocuments = new ObservableCollection<Document>();

            this.IsPinned = false;

            this.ID = Guid.NewGuid();
        }

        public Category(string categoryName)
        {
            this.CategoryName = categoryName;

            this.CategoryDocuments = new ObservableCollection<Document>();

            this.IsPinned = false;

            this.ID = Guid.NewGuid();
        }

        public ObservableCollection<Document> CategoryDocuments { get; set; }

        public bool IsPinned { get; set; }

        public string CategoryName { get; set; }

        public Guid ID { get; set; }

        private bool ShouldSerializeCategoryDocuments()
        {
            return false;
        }
    }
}
