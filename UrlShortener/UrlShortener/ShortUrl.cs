using System;
using System.Collections.Generic;
using System.Text;

namespace UrlShortener
{
    class ShortUrl : Table
    {
        public string Extension { get; set; }
        public string RedirectUrl { get; set; }
    }
}
