using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using YorkDayOfCode.Api.Models;

namespace YorkDayOfCode.Api
{
    public static class HttpPostCanvasFunction
    {
        [FunctionName("HttpPostCanvas")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "canvasses/{id:string}")]
            Canvas canvas,
            string id,
            TraceWriter log)
        {



            return (ActionResult)new OkObjectResult($"Hello");
        }
    }
}
