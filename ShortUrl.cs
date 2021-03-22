using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace UrlShortener
{
    class ShortUrl : TableEntity
    {
        public string RedirectUrl { get; set; }
        
        public ShortUrl()
        {

        }

        public ShortUrl( string shorturl, string redirecturl )
        {
            PartitionKey = shorturl;
            RowKey = shorturl;
            RedirectUrl = redirecturl;
        }

        public ShortUrl( string shorturl )
        {
            PartitionKey = shorturl;
            RowKey = shorturl;
            RedirectUrl = "";
        }
    }
    
}
