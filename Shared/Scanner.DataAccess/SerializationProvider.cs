//using Scanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Category = Scanner.Models.Category;
using Document = Scanner.Models.Document;
using Page = Scanner.Models.Page;
//using Scanner.Core;

namespace Scanner.DataAccess
{
    public sealed class SerializationProvider
    {
        private static readonly Lazy<SerializationProvider> lazy = 
            new Lazy<SerializationProvider>(() => new SerializationProvider(), true);

        private static readonly string FileNameDocuments = "documents.txt";

        private static readonly string FileNameCategories = "categories.txt";

        private static readonly string FileNamePages = "pages.txt";

        public SerializationProvider()
        {

        }

        public static SerializationProvider Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public async Task DeleteCategory(string categoryNameToDelete)
        {
            if (!string.IsNullOrEmpty(categoryNameToDelete))
            {
                var categories = await this.DeserializeCategories();

                var categoryToDelete = categories.FirstOrDefault(x => x.CategoryName == categoryNameToDelete);

                categories.Remove(categoryToDelete);

                await this.SerializeCategories(categories);
            }
        }

        public async Task DeleteDocuments(IEnumerable<Guid> documentIdList)
        {
            var documents = await this.DeserializeDocuments();

            foreach (var documentIdToDelete in documentIdList)
            {
                Document d = documents.FirstOrDefault(x => x.ID == documentIdToDelete);

                if (d != null)
                {
                    LocalStorageHelper.DeleteDocumentPagesFiles(d);

                    documents.Remove(d);
                }
            }

            await this.SerializeDocuments(documents);
        }

        public async Task DeletePages(IEnumerable<Guid> pageIdList, Guid documentId)
        {
            var documents = await this.DeserializeDocuments();

            var document = documents.FirstOrDefault(d => d.ID == documentId);

            foreach (var pageId in pageIdList)
            {
                var pageToDelete = document.Pages.FirstOrDefault(p => p.ID == pageId);

                document.Pages.Remove(pageToDelete);
            }

            //if (document.Pages.Count == 0)
            //{
            //    documents.Remove(document);
            //}

            await this.SerializeDocuments(documents);
        }

        public async Task AddDocument(Document documentToUpdate)
        {
            if (documentToUpdate != null)
            {
                var documents = await this.DeserializeDocuments();

                documents.Add(documentToUpdate);

                await this.SerializeDocuments(documents);
            }
        }

        public async Task UpdateDocument(Document d)
        {
            var documents = await this.DeserializeDocuments();

            var documentToUpdate = documents.FirstOrDefault(doc => doc.ID == d.ID);

            if (documentToUpdate != null)
            {
                documentToUpdate.Pages = d.Pages;

                documentToUpdate.DocumentSize = d.DocumentSize;
            }

            await SerializeDocuments(documents);
        }

        public async Task UpdateDocumentsCategoryName(string oldCategoryName, string newCategoryName)
        {
            var documents = await this.DeserializeDocuments();

            IEnumerable<Document> documentsToUpdate = documents.Where(d => d.DocumentCategory == oldCategoryName);

            foreach (var doc in documentsToUpdate) 
            {
                if (doc != null)
                {
                    doc.DocumentCategory = newCategoryName;
                }
            }

            await this.SerializeDocuments(documents);
        }

        public async Task UpdateDocumentPage(Page page)
        {
            var documents = await this.DeserializeDocuments();

            var documentToUpdate = documents.FirstOrDefault(d => d.ID == page.DocumentId);

            var pageToUpdate = documentToUpdate.Pages.FirstOrDefault(p => p.ID == page.ID);

            pageToUpdate = page;

            await this.SerializeDocuments(documents);

        }

        public async Task UpdateDocumentName(Guid documentIdToUpdate, string documentNewName)
        {
            var documents = await this.DeserializeDocuments();

            var document = documents.FirstOrDefault(d => d.ID == documentIdToUpdate);

            if (document != null)
            {
                document.DocumentName = documentNewName;
            }

            await this.SerializeDocuments(documents);
        }

        public async Task UpdateCategoryName(Guid categoryIdToUpdate, string categoryNewName)
        {
            var categories = await this.DeserializeCategories();

            var categoryToUpdate = categories.FirstOrDefault(c => c.ID == categoryIdToUpdate);

            if (categoryToUpdate != null)
            {
                categoryToUpdate.CategoryName = categoryNewName;
            }

            await this.SerializeCategories(categories);
        }

        public async Task<Category> GetCategoryById(Guid categoryId)
        {
            var categories = await this.DeserializeCategories();

            var documents = await this.DeserializeDocuments();

            var category = categories.FirstOrDefault(c => c.ID == categoryId);

            IEnumerable<Document> documentsToCategory =
                documents.Where(d => d.DocumentCategory.Equals(category.CategoryName));

            if (category != null && documentsToCategory != null)
            {
                foreach (var document in documentsToCategory)
                {
                    category.CategoryDocuments.Add(document);
                }
            }
                
            return category;
        }

        public async Task<Document> GetDocumentById(Guid documentId)
        {
            var documents = await this.DeserializeDocuments();

            var document = documents.FirstOrDefault(d => d.ID == documentId);

            if (document != null)
            {
                return document;
            }

            document = new Document();

            return document;
        }

        public async Task<ObservableCollection<Category>> DeserializeCategories()
        {
            ObservableCollection<Category> deserializedData = null;

            var storageProvider = new StorageProvider();

            deserializedData = 
                await storageProvider.ReadFromFile<ObservableCollection<Category>>(FileNameCategories);

            if (deserializedData == null)
            {
                deserializedData = new ObservableCollection<Category>();
            }

            return deserializedData;
        }

        public async Task<ObservableCollection<Document>> DeserializeDocuments()
        {
            ObservableCollection<Document> deserializedData = null;

            var storageProvider = new StorageProvider();

            deserializedData = await storageProvider.ReadFromFile<ObservableCollection<Document>>(FileNameDocuments);

            if (deserializedData == null)
            {
                deserializedData = new ObservableCollection<Document>();
            }

            return deserializedData;
        }

        public async Task SerializeCategories(object obj)
        {
            var categories = obj as ObservableCollection<Category>;

            var storageProvider = new StorageProvider();

            await storageProvider.WriteToFile<ObservableCollection<Category>>(FileNameCategories, categories);
        }

        public async Task SerializeDocuments(object obj)
        {
            var documents = obj as ObservableCollection<Document>;

            var storageProvider = new StorageProvider();

            await storageProvider.WriteToFile<ObservableCollection<Document>>(
                FileNameDocuments, documents);
        }

        public async Task SerializePages(object obj)
        {
            var pages = obj as ObservableCollection<Page>;

            var storageProvider = new StorageProvider();

            await storageProvider.WriteToFile<ObservableCollection<Page>>(FileNamePages, pages);
        }
    }
}
