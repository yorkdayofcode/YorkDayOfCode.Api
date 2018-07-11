
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using YorkDayOfCode.Api.TableEntities;

namespace YorkDayOfCode.Api
{
    public static class HttpGetCanvas
    {
        [FunctionName("HttpGetCanvas")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = @"canvasses/{id:regex(^[a-z]+\.[a-z]+\.[a-z]+$)}")]
            HttpRequest req,
            [Table("canvas", "{id}", "")]
            CanvasTableEntity cancasTableEntity,
            string id,
            TraceWriter log)
        {
           return cancasTableEntity != null
                ? (ActionResult)new OkObjectResult(new {id, code = cancasTableEntity.Code })
                : new NotFoundObjectResult($"Could not find canvas with an id of '{id}'");
        }
    }
}
