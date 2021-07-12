using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CryptocurrencyQuotes.Services;

namespace CryptocurrencyQuotes.Controllers
{
    public class CryptoQuotesController : Controller
    {
        IQuotesProvider provider;

        public CryptoQuotesController(IQuotesProvider p)
        {
            provider = p;
        }

        // GET: CryptoQuotes
        public ActionResult Index()
        {
            ViewBag.Quotes = provider.GetList();
            return View();
        }
    }
}