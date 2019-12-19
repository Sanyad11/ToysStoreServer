using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Serilog;

namespace ToysStore.ActionFilters
{
    public class ActionWebApiLoggingFilter : ActionFilterAttribute
    {
        private ILogger _logger;

        public ActionWebApiLoggingFilter(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                var url = actionContext.Request.RequestUri.ToString();
                var method = actionContext.Request.Method.ToString();
                var headers = actionContext.Request.Content.Headers.ToString();
                var tt = HttpContext.Current.Request.InputStream;
                string documentContents;
                using (Stream receiveStream = tt)
                {
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    {
                        documentContents = readStream.ReadToEnd();
                    }
                }
                _logger.Debug("Request details:\nUri: " + url + "\nMethod: " + method + "\nHeaders: " + headers + "\nBody: " + documentContents);
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message);
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                var response = actionExecutedContext.Response;
                if (response == null)
                    _logger.Debug("Exception: " + actionExecutedContext.Exception.Message.ToString());
                else
                {
                    var statusCode = response.StatusCode.ToString();
                    var headersList = response.Headers.ToList();
                    string headers = "";
                    foreach (var item in headersList)
                    {
                        headers += item.Value;
                    }


                    var body = response.Content.ReadAsStringAsync().Result;
                    _logger.Debug("Response: \n Status code: " + statusCode + "\n Headers: " + headers + "\nBody" + body);
                }

            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message);
            }
        }
    }

}