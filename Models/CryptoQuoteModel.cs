using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptocurrencyQuotes.Models
{
    public class CryptoQuoteModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Logo { get; set; }
        public decimal Price { get; set; }
        public decimal PercentChange1h { get; set; }
        public decimal PercentChange24h { get; set; }
        public decimal MarketCap { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}