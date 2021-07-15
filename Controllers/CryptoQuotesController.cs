using System;
using System.Web.Mvc;
using CryptocurrencyQuotes.Services;


namespace CryptocurrencyQuotes.Controllers
{
    [Authorize]
    [HandleError(View = "Error")]
    public class CryptoQuotesController : Controller
    {
        private readonly IQuotesProvider provider;

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