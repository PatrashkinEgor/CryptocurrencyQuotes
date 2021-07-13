using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptocurrencyQuotes.Services.CoinMarketCap
{
    class ConnectionSettings
    {
        public string ApiKey { get; set; }
        public string LatestQuotesURI { get; set; }
        public string CryptoInfoURI { get; set; }
    }
}