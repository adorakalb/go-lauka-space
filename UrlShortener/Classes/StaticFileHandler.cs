using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener
{
    class StaticFileHandler
    {
        private CloudStorageAccount _saccount;
        private CloudBlobClient _blobclient;
        private CloudBlobContainer _blobcontainer;


        public StaticFileHandler(string connectionstring, string blobname)
        {
            _saccount = CloudStorageAccount.Parse(connectionstring);
            _blobclient = _saccount.CreateCloudBlobClient();
            _blobcontainer = _blobclient.GetContainerReference(blobname);
        }

        public string GetFileContent( string path)
        {
            CloudBlockBlob blob = _blobcontainer.GetBlockBlobReference(path);
            return blob.DownloadTextAsync().Result;
        }
    }
}
