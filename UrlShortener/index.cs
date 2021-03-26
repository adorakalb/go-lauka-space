using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;

namespace UrlShortener
{
    public static class index
    {
        [FunctionName("index")]
        public static async Task<IActionResult> Index(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "index")] HttpRequest req,
            ILogger log)
        {
            if (Environment.GetEnvironmentVariable("APP_DEBUG_ISRUNLOCALLY") != "true")
            {
                string username = req.Headers["X-MS-CLIENT-PRINCIPAL-NAME"];
                if (string.IsNullOrWhiteSpace(username))
                {
                    return new RedirectResult("/.auth/login/aad", false);
                }
            }
            

            UrlManager urlmanager = new UrlManager(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "shorturl");
            IEnumerable<ShortUrl> urls = urlmanager.GetAll();

            StaticFileHandler filehandler = new StaticFileHandler(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "static");
            string index = filehandler.GetFileContent("index.html");
            index = index.Replace("##APPCONFIG_DOMAINNAME##", Environment.GetEnvironmentVariable("APP_DOMAINNAME"));


            string table = "";
            int counter = 1;

            table += "<table class='table table-striped'>\n";
            table += "<thead>\n";
            table += "<tr>\n";
            table += "<th scope='col'>#</th>\n";
            table += "<th scope='col'>Slug</th>\n";
            table += "<th scope='col'>URL</th>\n";
            table += "<th scope='col'>Delete</th>\n";
            table += "</tr>\n";
            table += "</thead>\n";
            table += "<tbody>\n";

            foreach (ShortUrl url in urls)
            {
                table += "<tr>\n";
                table += $"<th scope='row'>{counter}</th>\n";
                table += $"<td>{url.PartitionKey}</td>\n";
                table += $"<td>{url.RedirectUrl}</td>\n";
                table += $"<td><a href='/api/delete?slug={url.PartitionKey}'><button type='button' class='btn btn-sm btn-danger'>Delete</button></a></td>\n";
                table += "</tr>\n";
                counter++;  
            }
            table += "</tbody>\n";
            table += "</table>\n";

            index = index.Replace("##URLTABLE##", table);
            return new ContentResult { Content = index, ContentType = "text/html", StatusCode = 200 };
        }
    }
}
