using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using RampUpProjectBE.Utils;

namespace RampUpProjectBE.Filters {
    public class ExceptionFilter : ExceptionFilterAttribute {

        // private Logger logger = LogManager.GetLogger("ExceptionFilter");

        /// <summary>
        /// Exception handler method.
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context) {
            if (context.Exception is UnauthorizedAccessException) {
                LogErrorException(context);

                context.Response = new HttpResponseMessage() {
                    Content = new StringContent("Error during authentication operation: " + context.Exception.Message),
                    StatusCode = HttpStatusCode.Unauthorized
                };
            } else if (context.Exception is ArgumentNullException || context.Exception is FormatException) {
                LogErrorException(context);

                context.Response = new HttpResponseMessage() {
                    Content = new StringContent("Please check you are including all required parameters and format."),
                    StatusCode = HttpStatusCode.BadRequest
                };
            } else if (context.Exception is NotImplementedException) {
                LogErrorException(context);

                context.Response = new HttpResponseMessage() {
                    Content = new StringContent(string.Format("Exception: {0} InnerException: {1}", context.Exception.Message, context.Exception.InnerException == null ? "N/A" : context.Exception.InnerException.Message)),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            } else {
                LogErrorException(context);

                context.Response = new HttpResponseMessage() {
                    Content = new StringContent(string.Format("An unexpected exception was raised. Please try again. Exception: {0} InnerException: {1}", context.Exception.Message, context.Exception.InnerException == null ? "N/A" : context.Exception.InnerException.Message)),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        private void LogErrorException(HttpActionExecutedContext context) {
            Logger.Error(context.ActionContext.ActionDescriptor.ActionName, context.Exception);
        }
    }
}