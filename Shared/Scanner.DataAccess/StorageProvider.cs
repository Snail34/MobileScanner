using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Scanner.DataAccess
{
    public class StorageProvider
    {
        private readonly AsyncLock locker = new AsyncLock();

        public StorageProvider()
        {

        }
        //Wtite to file<t>(name, object t) UnauthorizedAccessException
        public async Task WriteToFile<T>(string fileName, T obj) where T : class
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            var file = await localFolder.GetFileAsync(fileName);
            
            string s = JsonConvert.SerializeObject(obj, Formatting.Indented);

            await FileIO.WriteTextAsync(file, s);
        }

        public async Task<T> ReadFromFile<T>(string fileName) where T : class
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            
            string content = string.Empty;

            using (var releaser = await this.locker.LockAsync())
            {
                var file = await localFolder.GetFileAsync(fileName);

                content = await FileIO.ReadTextAsync(file);
            }

            T deserializedData = null;

            if (!string.IsNullOrEmpty(content))
            {
                deserializedData = JsonConvert.DeserializeObject<T>(content);
            }

            return deserializedData;
        }
        
        // rear from file<t>(name);
    }
}
