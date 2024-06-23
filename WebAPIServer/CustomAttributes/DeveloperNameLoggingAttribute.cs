

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIServer.Services;

namespace WebAPIServer.CustomAttributes
{
    public class DeveloperNameLoggingAttribute : ActionFilterAttribute
    {
        public const string DEVELOPER_HEADER_KEY = "x-developer-name";
        private readonly ILoggerService _logger;

        public DeveloperNameLoggingAttribute(ILoggerService logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(DEVELOPER_HEADER_KEY, out var developerNames))
            {
                _logger.Warning("Developer name not found in request headers.");
                context.Result = new BadRequestObjectResult($"Developer name is required in the '{DEVELOPER_HEADER_KEY}' header.");
                return;
            }

            var developerName = developerNames.FirstOrDefault();
            _logger.Info($"Request from developer: {developerName}");
        }
    }
}
