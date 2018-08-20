using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using YorkDayOfCode.Api.Models.Users;
using YorkDayOfCode.Api.TableEntities;
using YorkDayOfCode.Api.Validation;

namespace YorkDayOfCode.Api
{
    public static class HttpPostUser
    {
        private static readonly AccessTokenGenerator AccessTokenGenerator;

        private static readonly UserValidator UserValidator;

        static HttpPostUser()
        {
            AccessTokenGenerator = new AccessTokenGenerator(new SymmetricSigningKey());
            UserValidator = new UserValidator();
        }

        [FunctionName("HttpPostUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")]
            User user,
            [Table("users")]
            ICollector<UserTableEntity> users,
            TraceWriter log)
        {
            var validationResult = await UserValidator.ValidateAsync(user).ConfigureAwait(false);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(new
                {
                    errors = validationResult.Errors
                            .Select(x => new {x.PropertyName, x.ErrorMessage})
                            .ToArray()
                }); 
            }

            var userId = Guid.NewGuid().ToString();
            users.Add(new UserTableEntity()
            {
                PartitionKey = userId,
                Gender = user.Gender.ToString(),
                Age = user.Age.ToString(),
                Area = user.Area.ToString(),
                Experience = user.Experience.ToString()
            });

            var expiresIn = TimeSpan.FromHours(1);

            var accessToken = AccessTokenGenerator.Generate(userId, expiresIn);

            return new OkObjectResult(new
            {
                access_token = accessToken,
                token_type = "bearer",
                expires_in = expiresIn.TotalSeconds
            });
        }
    }
}
