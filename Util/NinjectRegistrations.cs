using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using CryptocurrencyQuotes.Services;


namespace CryptocurrencyQuotes.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IQuotesProvider>().To<Services.CoinMarketCap.Provider>();
        }   
    }
}