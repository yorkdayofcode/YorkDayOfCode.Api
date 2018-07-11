using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using YorkDayOfCode.Api.SuggestCanvasID;
using YorkDayOfCode.Api.TableEntities;

namespace YorkDayOfCode.Api
{
    public class CanvasMetaStorage
    {
        private readonly CloudTable _cloudTable;
        private readonly ICanvasIDSuggester _canvasIdSuggester;

        public CanvasMetaStorage(CloudTable cloudTable, ICanvasIDSuggester canvasIdSuggester)
        {
            _cloudTable = cloudTable;
            _canvasIdSuggester = canvasIdSuggester;
        }

        public async Task<string> Store(string code)
        {
            _cloudTable.CreateIfNotExistsAsync();

            var canvasId = await GetUniqueCanvasId();

            var tableEntity = new CanvasTableEntity() { PartitionKey = canvasId, Code = code };

            var tableOperation = TableOperation.Insert(tableEntity);

            await _cloudTable.ExecuteAsync(tableOperation);

            return canvasId;
        }   

        private async Task<string> GetUniqueCanvasId()
        {
            var canvasId = _canvasIdSuggester.Suggest();

            if (await PartitionKeyExists(canvasId))
            {
                canvasId = _canvasIdSuggester.Suggest();
                if (await PartitionKeyExists(canvasId))
                {
                    canvasId = _canvasIdSuggester.Suggest();
                    if (await PartitionKeyExists(canvasId))
                    {
                        throw new NotImplementedException("Could not get a unique canvas id");
                    }
                }
            }

            return canvasId;
        }

        public async Task<bool> PartitionKeyExists(string partitionKey)
        {
            var tableOperation = TableOperation.Retrieve(partitionKey, "", new List<string>() {"PartitionKey"});

            var result = await _cloudTable.ExecuteAsync(tableOperation);

            return result.Result != null;
        }
    }
}