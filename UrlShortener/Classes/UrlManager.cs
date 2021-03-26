using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener
{
    class UrlManager
    {
        private CloudTable _table;
        public UrlManager( string connectionstring, string dbname) // connectionstring: AzureWebJobsStorage
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionstring);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference(dbname);
        }

        public async Task<ShortUrl> GetEntityFromTableByKey(string key)
        {
            var shorturl = (ShortUrl)_table.Execute(TableOperation.Retrieve<ShortUrl>(key, key)).Result;
            
            if (shorturl == null)
            {
                //return new ShortUrl(key);
                return null;
            }

            return shorturl;
        }
        public IEnumerable<ShortUrl> GetAll()
        {
            TableQuery<ShortUrl> query = new TableQuery<ShortUrl>(); ;
            return _table.ExecuteQuery(query).OrderBy(o => o.Timestamp);
        }

        public async Task AddOrReplaceEntityAsync(ShortUrl url)
        {
            TableOperation op = TableOperation.InsertOrReplace(url);
            await _table.ExecuteAsync(op);
        }
    }

}
