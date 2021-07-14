using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CryptocurrencyQuotes.Logger
{
    public class ExceptionLoggerAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            ExceptionDetail exceptionDetail = new ExceptionDetail()
            {
                ExceptionMessage = filterContext.Exception.Message,
                StackTrace = filterContext.Exception.StackTrace,
                ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                ActionName = filterContext.RouteData.Values["action"].ToString(),
                Date = DateTime.Now
            };

            using (LoggerContext db = new LoggerContext())
            {
                db.ExceptionDetails.Add(exceptionDetail);
                db.SaveChanges();
            }
//            filterContext.ExceptionHandled = true;
        }
    }
}