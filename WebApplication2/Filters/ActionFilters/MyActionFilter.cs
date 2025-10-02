using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication2.Models;

namespace WebApplication2.Filters.ActionFilters
{
    public class MyActionFilter : IActionFilter
    {
        private readonly ILogger<MyActionFilter> _logger;

        public MyActionFilter(ILogger<MyActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
            {
                // Action executed successfully
                if (context.Result is ObjectResult objectResult)
                {
                    _logger.LogInformation("Action executed successfully with status code: {statusCode}", objectResult.StatusCode);

                    // You can log the type of result returned
                    _logger.LogInformation("Returned result type: {resultType}", objectResult.Value?.GetType().Name ?? "null");

                    // Example: log number of employees if action returns a collection
                    if (objectResult.Value is IEnumerable<Employee> employees)
                    {
                        _logger.LogInformation("Number of employees returned: {count}", employees.Count());
                    }
                }
                else
                {
                    _logger.LogInformation("Action executed successfully but result is not an ObjectResult." +
                        " Type: {resultType}", context.Result?.GetType().Name);
                }
            }
            else
            {
                // Action threw an exception
                _logger.LogError(context.Exception, "An exception occurred while executing the action.");
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
           if (context.ActionArguments.ContainsKey("filterOn"))
            {
                string? filterOn = Convert.ToString(context.ActionArguments["filterOn"]);

                _logger.LogInformation("Actual Value for Fillter On{filterOn}",filterOn);

                if (!string.IsNullOrEmpty(filterOn))
                {
                    var filterOnOption = new List<string>()
                    {
                        nameof(Employee.Name),
                        nameof(Employee.DepartmentId),
                    };

                    if (filterOnOption.Any(option => option == filterOn) == false)
                    {
                        context.ActionArguments["filterOn"]=nameof(Employee.Name);

                        _logger.LogInformation("The Updated Value for Filter On{filterOn}", context.ActionArguments["filterOn"]);
                    }

                }

            }
            
        }
    }
}
