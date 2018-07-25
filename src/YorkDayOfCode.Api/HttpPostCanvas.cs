using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Polly;
using YorkDayOfCode.Api.Messages;
using YorkDayOfCode.Api.Models;
using YorkDayOfCode.Api.SuggestCanvasID;

namespace YorkDayOfCode.Api
{
    public static class HttpPostCanvas
    {
        private static readonly RandomWordSuggester RandomWordSuggester;
        private static readonly HeaderAuthorizer HeaderAuthorizer;

        static HttpPostCanvas()
        {
            var rnd = new RandomUsingSystemRandom();
            RandomWordSuggester = new RandomWordSuggester(rnd);
            HeaderAuthorizer =
                new HeaderAuthorizer(new TokenValidator(new SymmetricSigningKey(), new JwtSecurityTokenHandler()));
        }

        [FunctionName("HttpPostCanvas")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "canvasses")]
            Canvas canvas,
            IDictionary<string, string> headers,
            [Queue("create-canvas")]
            ICollector<CreateCanvasImage> createCanvasQueue,
            [Table("canvas")]
            CloudTable canvassesTable,
            TraceWriter log)
        {
            if (!HeaderAuthorizer.Authorize(headers))
            {
                return new UnauthorizedResult();
            }

            var canvasMetaStorage = new CanvasMetaStorage(canvassesTable, RandomWordSuggester);

            var canvasId = await Policy
                .Handle<StorageException>()
                .RetryAsync(3)
                .ExecuteAsync(async () => await canvasMetaStorage.Store(canvas.Code));
            
            var bytes = Convert.FromBase64String(canvas.Image);
            var createCanvasImage = new CreateCanvasImage() { CanvasId = canvasId, Image = bytes};
            createCanvasQueue.Add(createCanvasImage);

            return new OkObjectResult(new {id = canvasId});
        }
    }
}
