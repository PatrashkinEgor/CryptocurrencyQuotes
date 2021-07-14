using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CryptocurrencyQuotes.Logger;

namespace CryptocurrencyQuotes.Controllers
{
    [Authorize(Roles = "admin")]
    public class ExceptionDetailsController : Controller
    {
        private LoggerContext db = new LoggerContext();

        // GET: ExceptionDetails
        public ActionResult Index()
        {
            return View(db.ExceptionDetails.ToList());
        }

        // GET: ExceptionDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExceptionDetail exceptionDetail = db.ExceptionDetails.Find(id);
            if (exceptionDetail == null)
            {
                return HttpNotFound();
            }
            return View(exceptionDetail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
