using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using YorkDayOfCode.Api.Messages;
using YorkDayOfCode.Api.Models;

namespace YorkDayOfCode.Api
{
    public interface ICanvasIdProvider
    {
        string GetNext();
    }

    public class CanvasIdProvider : ICanvasIdProvider
    {
        public string GetNext()
        {
            return Guid.NewGuid().ToString();
        }
    }
    public static class HttpPostCanvasFunction
    {
        [FunctionName("HttpPostCanvas")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "canvasses")]
            Canvas canvas,
            [Queue("create-canvas")]
            ICollector<CreateCanvasImage> createCanvasQueue,
            TraceWriter log)
        {
            var idProvider = new CanvasIdProvider();
            var bytes = Convert.FromBase64String(canvas.Image);

            var canvasId = idProvider.GetNext();
            var createCanvasImage = new CreateCanvasImage() {CanvasId = canvasId, Image = bytes};
            createCanvasQueue.Add(createCanvasImage);

            return new OkObjectResult(new {id = canvasId});
        }
    }
}
