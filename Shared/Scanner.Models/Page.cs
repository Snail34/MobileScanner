using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Scanner.Models
{
    public class Page
    {
        public async static Task<Page> CreatePageAsync(PhotoCapturedData data)
        {
            Page page = new Page();

            page.FilePath = page.GetFilePath();

            page.BackUpFilePath = page.GetBackUpFilePath();

            page.ProcessingFilePath = page.GetProcessingFilePath();

            page.FilteredFilePath = page.GetFilteredFilePath();

            page.IsFromCamera = data.IsFromCamera;

            page.DateCreated = page.DateToString();

            page.TimeCreated = page.TimeOfDayToString();

            page.Number = 1;

            page.Size = data.ImageFileSize;

            page.PageName = "Page" + page.Number.ToString();

            page.DocumentId = Guid.Empty;

            return page;
        }

        public Page()
        {
            this.ID = Guid.NewGuid();
        }

        public Guid ID { get; set; }

        public string PageName { get; set; }

        public Guid DocumentId { get; set; }

        public string FilePath { get; set; }

        public string BackUpFilePath { get; set; }

        public string ProcessingFilePath { get; set; }

        public string FilteredFilePath { get; set; }

        public string SmallPreviewFilePath { get; set; }

        public string PreviewFilePath { get; set; }

        public string DateCreated { get; set; }

        public string TimeCreated { get; set; }

        public int Number { get; set; }

        public ulong Size { get; set; }

        public bool IsFromCamera { get; set; }

        public double MbSize 
        {
            get
            {
                return Math.Round((double)this.Size / 1048576.0, 2);
            }
        }

        public string FileName
        {
            get
            {
                return this.ID.ToString() + ".jpg";
            }
        }

        public string BackupFileName
        {
            get
            {
                return this.ID.ToString() + "_backup" + ".jpg";
            }
        }

        public string ProcessingFileName
        {
            get
            {
                return this.ID.ToString() + "_processing" + ".jpg";
            }
        }

        public string FilteredFileName
        {
            get
            {
                return this.ID.ToString() + "_filtered" + ".jpg";
            }
        }

        private string GetFilePath()
        {
            return "ms-appdata:///local/" + this.FileName;
        }

        private string GetBackUpFilePath()
        {
            return "ms-appdata:///local/" + this.BackupFileName;
        }

        private string GetProcessingFilePath()
        {
            return "ms-appdata:///local/" + this.ProcessingFileName;
        }

        private string GetFilteredFilePath()
        {
            return "ms-appdata:///local/" + this.FilteredFileName;
        }

        private string TimeOfDayToString()
        {
            string timeString = DateTime.Now.TimeOfDay.ToString();

            string[] s = timeString.Split('.');

            return s[0];
        }

        private string DateToString()
        {
            string dateString = DateTime.Now.Date.ToString();

            string[] s = dateString.Split(' ');

            return s[0];
        }

        private bool ShouldSerializeFileName()
        {
            return false;
        }

        private bool ShouldSerializeBackupFileName()
        {
            return false;
        }

        private bool ShouldSerializeProcessingFileName()
        {
            return false;
        }

        private bool ShouldSerializeFilteredFileName()
        {
            return false;
        }
    }
}
