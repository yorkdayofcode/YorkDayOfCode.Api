using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;
using YorkDayOfCode.Api.Messages;
using YorkDayOfCode.Api.Models;

namespace YorkDayOfCode.Api
{
    public static class QueueCanvasFunction
    {
        [FunctionName("QueueCanvas")]
        public static async Task QueueTrigger(
            [QueueTrigger("create-canvas")] CreateCanvasImage message,
            [Blob("canvasses")] CloudBlobContainer cloudBlobContainer,
            TraceWriter log)
        {
            await cloudBlobContainer.CreateIfNotExistsAsync();

            var imageBlob = cloudBlobContainer.GetBlockBlobReference(message.CanvasId);
            imageBlob.Properties.ContentType = "image/jpeg";

            await imageBlob.UploadFromByteArrayAsync(message.Image, 0, message.Image.Length);
        }
    }
}
