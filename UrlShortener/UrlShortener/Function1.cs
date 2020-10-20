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

namespace UrlShortener
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "baseurl")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");



            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            string test = @"<!DOCTYPE html>
<html>
<head>
    <meta charset = 'utf-8'>
 
     <meta http - equiv = 'X-UA-Compatible' content = 'IE=edge'>
      
          <title> Hi </title>
      
          <meta name = 'viewport' content = 'width=device-width, initial-scale=1'>
         </head>
         <body>
             test!

    asdsadsadsad
</body>
</html> ";

            return new ContentResult { Content = test, ContentType = "text/html", StatusCode = 200 };
        }
    }
}
