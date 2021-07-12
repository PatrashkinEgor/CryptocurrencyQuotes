using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptocurrencyQuotes.Models
{
    public class CryptoQuotesModel
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Logo { get; set; }
        public decimal Price { get; set; }
        public decimal Percent_change_1h { get; set; }
        public decimal Percent_change_24h { get; set; }
        public decimal Market_cap { get; set; }
        public DateTime Last_updated { get; set; }
    }
}