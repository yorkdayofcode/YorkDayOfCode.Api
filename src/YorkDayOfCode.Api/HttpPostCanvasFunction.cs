using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using YorkDayOfCode.Api.Messages;
using YorkDayOfCode.Api.Models;
using YorkDayOfCode.Api.SuggestCanvasID;

namespace YorkDayOfCode.Api
{
    public static class HttpPostCanvasFunction
    {
        private static readonly RandomWordSuggester _randomWordSuggester;

        static HttpPostCanvasFunction()
        {
            var rnd = new RandomUsingSystemRandom();
            _randomWordSuggester = new RandomWordSuggester(rnd);
        }
        [FunctionName("HttpPostCanvas")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "canvasses")]
            Canvas canvas,
            [Queue("create-canvas")]
            ICollector<CreateCanvasImage> createCanvasQueue,
            TraceWriter log)
        {
            log.Info("Called");
            var bytes = Convert.FromBase64String(canvas.Image);

            var canvasId = _randomWordSuggester.Suggest();

            var createCanvasImage = new CreateCanvasImage() {CanvasId = canvasId, Image = bytes};
            createCanvasQueue.Add(createCanvasImage);

            return new OkObjectResult(new {id = canvasId});
        }
    }
}
