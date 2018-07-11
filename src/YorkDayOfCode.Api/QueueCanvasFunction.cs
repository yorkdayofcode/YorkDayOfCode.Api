using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;
using YorkDayOfCode.Api.Models;

namespace YorkDayOfCode.Api
{
    public static class QueueCanvasFunction
    {
        [FunctionName("QueueCanvas")]
        public static async Task QueueTrigger(
            [QueueTrigger("myqueue-items")] Canvas canvas,
            [Blob("sample-images-md/{name}", FileAccess.Write)] ICloudBlob imageBlob,
            TraceWriter log)
        {
            var imageBytes = Convert.FromBase64String(canvas.Image);

            await imageBlob.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);
        }
    }
}
