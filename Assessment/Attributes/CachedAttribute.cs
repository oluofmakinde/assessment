using Assessment.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Assessment.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;
        private readonly bool _isUpdate;

        public CachedAttribute(int timeToLiveInSeconds = 30, bool isUpdate = false)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
            _isUpdate = isUpdate;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var cacheservice = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cacheresponse = cacheservice.GetCacheResponse(Constant.ContactCacheKey);

            if (!string.IsNullOrEmpty(cacheresponse) && !_isUpdate)
            {
                var contentResult = new ContentResult
                {
                    Content = cacheresponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                context.Result = contentResult;

                return;
            }

            var executedContext = await next();

            if (executedContext.Result is OkObjectResult || executedContext.Result is OkResult)
            {
                if (_isUpdate)
                    cacheservice.Remove(Constant.ContactCacheKey);
                else
                    cacheservice.CacheResponse(Constant.ContactCacheKey, ((OkObjectResult)executedContext.Result).Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }

        }
    }
}
