using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace UrlShortener
{
    public static class AddShortUrl
    {
        [FunctionName("add_form")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            if (Environment.GetEnvironmentVariable("APP_DEBUG_ISRUNLOCALLY") != "true")
            {
                string username = req.Headers["X-MS-CLIENT-PRINCIPAL-NAME"];
                if (string.IsNullOrWhiteSpace(username))
                {
                    return new RedirectResult("/.auth/login/aad", false);
                }
            }

            StaticFileHandler filehandler = new StaticFileHandler(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "static");
            string addform = filehandler.GetFileContent("addform.html");

            addform = addform.Replace("##APPCONFIG_DOMAINNAME##", Environment.GetEnvironmentVariable("APP_DOMAINNAME"));

            return new ContentResult { Content = addform, ContentType = "text/html", StatusCode = 200 };
        }
    }

    public static class AddShortUrlApi
    {
        [FunctionName("add")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "add")] HttpRequest req)
        {
            if (Environment.GetEnvironmentVariable("APP_DEBUG_ISRUNLOCALLY") != "true")
            {
                string username = req.Headers["X-MS-CLIENT-PRINCIPAL-NAME"];
                if (string.IsNullOrWhiteSpace(username))
                {
                    return new RedirectResult("/.auth/login/aad", false);
                }
            }

            ShortUrl addurl = new ShortUrl(req.Form["slug"].ToString(), req.Form["redirecturl"].ToString());

            UrlManager db = new UrlManager(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "shorturl");

            await db.AddOrReplaceEntityAsync(addurl);

            return new RedirectResult("/");
        }
    }
}

