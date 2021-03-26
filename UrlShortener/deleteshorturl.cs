using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener
{
    public static class DeleteShortUrlApi
    {
        [FunctionName("delete")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "delete")] HttpRequest req)
        {
            if (Environment.GetEnvironmentVariable("APP_DEBUG_ISRUNLOCALLY") != "true")
            {
                string username = req.Headers["X-MS-CLIENT-PRINCIPAL-NAME"];
                if (string.IsNullOrWhiteSpace(username))
                {
                    return new RedirectResult("/.auth/login/aad", false);
                }
            }

            UrlManager db = new UrlManager(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "shorturl");
            ShortUrl url = await db.GetEntityFromTableByKeyAsync(req.Query["slug"]);
            await db.DeleteEntityAsync(url);

            return new RedirectResult("/");
        }
    }
}
