using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using YorkDayOfCode.Api.Messages;
using YorkDayOfCode.Api.Models;
using YorkDayOfCode.Api.SuggestCanvasID;

namespace YorkDayOfCode.Api
{
    public static class HttpPostCanvasFunction
    {
        private static readonly RandomWordSuggester RandomWordSuggester;

        static HttpPostCanvasFunction()
        {
            var rnd = new RandomUsingSystemRandom();
            RandomWordSuggester = new RandomWordSuggester(rnd);
        }

        [FunctionName("HttpPostCanvas")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "canvasses")]
            Canvas canvas,
            [Queue("create-canvas")]
            ICollector<CreateCanvasImage> createCanvasQueue,
            [Table("canvas")]
            CloudTable canvassesTable,
            TraceWriter log)
        {
            var canvasMetaStorage = new CanvasMetaStorage(canvassesTable, RandomWordSuggester);
            var canvasId = await canvasMetaStorage.Store(canvas.Code);

            var bytes = Convert.FromBase64String(canvas.Image);
            var createCanvasImage = new CreateCanvasImage() { CanvasId = canvasId, Image = bytes};
            createCanvasQueue.Add(createCanvasImage);

            return new OkObjectResult(new {id = canvasId});
        }
    }
}
