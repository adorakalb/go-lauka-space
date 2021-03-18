using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace UrlShortener
{
    class DatabaseManager
    {
        private CloudTable _table;
        public DatabaseManager( string connectionstring, string dbname )
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionstring);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference(dbname);
        }
    }

}
