using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication2.Filters.ResourceFitlers
{
    public class MyResourceFilter : IAsyncResourceFilter
    {
        private readonly bool _isDisabled;
        private readonly ILogger _logger;
        public MyResourceFilter(bool isDisabled, ILogger<MyResourceFilter> logger)
        {
            _isDisabled = isDisabled;
            _logger = logger;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
           if(_isDisabled)
            {
                _logger.LogInformation("My Resource Filter is shortcircuiting");
                context.Result = new NotFoundResult();
            }

           await next();

            _logger.LogInformation("My Resource Filter is Executed");
        }
    }
}
