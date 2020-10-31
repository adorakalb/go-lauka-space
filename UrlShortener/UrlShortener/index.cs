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
    public static class index
    {
        [FunctionName("index")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "index")] HttpRequest req,
            ILogger log)
        {
            string username = req.Headers["X-MS-CLIENT-PRINCIPAL-NAME"];

            if(string.IsNullOrWhiteSpace(username))
            {
                return new RedirectResult("/.auth/login/aad", false);
            }


            string test = @$"<!DOCTYPE html>
<html>
<head>
    <meta charset = 'utf-8'>
 
     <meta http - equiv = 'X-UA-Compatible' content = 'IE=edge'>
      
          <title> Hi </title>
      
          <meta name = 'viewport' content = 'width=device-width, initial-scale=1'>
         </head>
         <body>
             User: {username} <br/>
            <a href='/.auth/login/aad'>Log in with Azure AD</a>
</body>
</html> ";

            return new ContentResult { Content = test, ContentType = "text/html", StatusCode = 200 };
        }
    }
}
