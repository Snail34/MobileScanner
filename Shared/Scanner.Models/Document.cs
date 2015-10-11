using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Scanner.Models
{
    public class Document
    {
        public async static Task<Document> CreateDocumentAsync(PhotoCapturedData capturedData)
        {
            var createdDocument = new Document(capturedData.CategoryName);

            var firstPage = await Page.CreatePageAsync(capturedData);

            firstPage.DocumentId = createdDocument.ID;

            createdDocument.Pages.Add(firstPage);

            createdDocument.DocumentSize = firstPage.MbSize;

            return createdDocument;
        }

        public static Document CreateDefaultDocument(string categoryName)
        {
            return new Document(categoryName);
        }

        public static void CopyDocument(Document src, Document dst)
        {
            dst.ID = src.ID;
            dst.DocumentSize = src.DocumentSize;
            dst.DocumentCategory = src.DocumentCategory;
            dst.DateCreated = src.DateCreated;
            dst.DocumentName = src.DocumentName;
            dst.TimeCreated = src.TimeCreated;

            foreach (var page in src.Pages)
            {
                dst.Pages.Add(page);
            }
        }

        public Document(string categoryName)
        {
            this.ID = Guid.NewGuid();

            this.Pages = new ObservableCollection<Page>();

            this.DocumentCategory = categoryName;

            this.IsPinned = false;

            this.DateCreated = DateTime.Now.Date.ToString();

            this.DocumentName = "Document";

            this.TimeCreated = DateTime.Now.TimeOfDay.ToString();

            this.DocumentSize = 0.0f;
        }

        public Document()
        {

        }

        public ObservableCollection<Page> Pages { get; set; }

        public string DocumentName { get; set; }

        public string DateCreated { get; set; }

        public string TimeCreated { get; set; }

        public double DocumentSize { get; set; }

        public int DocumentPageCounter 
        {
            get
            {
                return this.Pages.Count;
            }
        }

        public string DocumentCategory { get; set; }

        public static int DocumentCounter { get; set; }

        public Guid ID { get; set; }

        public bool IsPinned { get; set; }
    }
}
