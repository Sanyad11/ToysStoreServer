using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using Serilog;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using ToysStore.Attributes;

namespace ToysStore.ActionFilters
{
    public class ActionWebApiCacheFilter : ActionFilterAttribute
    {
        private ILogger _logger;

        public ActionWebApiCacheFilter(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var attr = actionContext.ActionDescriptor.GetCustomAttributes<CacheAttribute>().FirstOrDefault();

            if (attr != null)
            {
                var uniqKey = attr.Key + actionContext.Request.RequestUri.ToString() + JsonConvert.SerializeObject(actionContext.Request.GetQueryNameValuePairs());
                _logger.Debug($"OnActionExecuting. Input attribute: {attr}, uniqKey: {uniqKey}");
                MemoryCache memoryCache = MemoryCache.Default;
                try
                {
                    var cachedItem = memoryCache.Get(uniqKey);
                    if (cachedItem != null)
                    {
                        actionContext.Response = cachedItem as HttpResponseMessage;
                    }
                }
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var attr = actionExecutedContext.ActionContext.ActionDescriptor.GetCustomAttributes<CacheAttribute>().FirstOrDefault();
            if (attr != null)
            {
                var uniqKey = attr.Key + actionExecutedContext.Request.RequestUri.ToString() + JsonConvert.SerializeObject(actionExecutedContext.Request.GetQueryNameValuePairs());
                _logger.Debug($"OnActionExecuted. Input attribute: {attr}, uniqKey: {uniqKey}");
                MemoryCache memoryCache = MemoryCache.Default;
                memoryCache.Add(uniqKey, actionExecutedContext.Response, DateTime.Now.AddMinutes(1));
            }
        }
    }

}

