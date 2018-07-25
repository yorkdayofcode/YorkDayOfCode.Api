using Microsoft.WindowsAzure.Storage.Table;
using YorkDayOfCode.Api.Models.Users;

namespace YorkDayOfCode.Api.TableEntities
{
    public class UserTableEntity : TableEntity
    {
        public UserTableEntity()
        {
            RowKey = string.Empty;
        }

        public string Gender { get; set; }

        public string Age { get; set; }

        public string Area { get; set; }

        public string Experience { get; set; }
    }
}