using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace UrlShortener
{
    public static class redirect
    {
        [FunctionName("redirect")]
        public static async Task<IActionResult> Redirect(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            string query = req.Query["sl"];

            UrlManager db = new UrlManager(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "shorturl");
            ShortUrl url = await db.GetEntityFromTableByKey(query);

            //return new ContentResult { Content = url.RedirectUrl, ContentType = "text/plain", StatusCode = 200 };

            if (String.IsNullOrWhiteSpace(url.RedirectUrl))
            {
                return new NotFoundResult();
            }
            else
            {
                return new RedirectResult(url.RedirectUrl);
            }
            
        }
    }
}
