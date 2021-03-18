using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace UrlShortener
{
    class ShortUrl : TableEntity
    {
        public string Extension { get; set; }
        public string RedirectUrl { get; set; }
    }
}
