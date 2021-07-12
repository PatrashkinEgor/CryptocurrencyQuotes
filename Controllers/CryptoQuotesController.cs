using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CryptocurrencyQuotes.Controllers
{
    public class CryptoQuotesController : Controller
    {
        // GET: CryptoQuotes
        public ActionResult Index()
        {
            return View();
        }
    }
}