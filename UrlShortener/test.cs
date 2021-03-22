using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener
{
    class test
    {
        [FunctionName("test")]
        public static async Task<IActionResult> Test(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
            ILogger log)
        {
            string query = req.Query["id"];

            UrlManager db = new UrlManager(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "shorturl");
            ShortUrl url = await db.GetEntityFromTableByKey(query);
            
            return new ContentResult { Content = url.RedirectUrl, ContentType = "text/plain", StatusCode = 200 };
        }
    }
}
