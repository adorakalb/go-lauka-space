using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using RestSharp;
using System.Linq;
using System.Net.Http.Headers;

namespace UrlShortener
{
    public static class staticfile
    {
        [FunctionName("staticfile")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "static")] HttpRequest req,
            ILogger log)
        {
            string file = req.Query["file"];
            var response = new HttpResponseMessage();

            log.LogInformation($"staticfile: GET /static/{file}");


            if (File.Exists($"static/{file}"))
            {
                response.StatusCode = HttpStatusCode.OK;
                var stream = new FileStream($"static/{file}", FileMode.Open);
                response.Content = new StreamContent(stream);

                string ending = file.Split(".").Last();

                switch (ending)
                {
                    case "html":
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                        break;
                    case "css":
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/css");
                        break;
                    case "js":
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/javascript");
                        break;
                    default:
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                        break;
                }

                log.LogInformation($"staticfile: 200 /static/{file}");
                return response;
            }
            else
            {
                log.LogInformation($"staticfile: 404 /static/{file}");

                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }
        }
    }
}
