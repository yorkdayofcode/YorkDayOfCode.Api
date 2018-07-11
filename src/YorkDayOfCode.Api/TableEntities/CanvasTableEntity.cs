using Microsoft.WindowsAzure.Storage.Table;

namespace YorkDayOfCode.Api.TableEntities
{
    public class CanvasTableEntity : TableEntity
    {
        public CanvasTableEntity()
        {
            RowKey = string.Empty;
        }

        public string Code { get; set; }
    }
}
