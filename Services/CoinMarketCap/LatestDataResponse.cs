using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptocurrencyQuotes.Services.CoinMarketCap
{
    class LatestDataResponse
    {
        public Status Status { get; set; }
        public Cryptocurrency[] Data { get; set; }
    }

    class Status
    {
        public DateTime Timestamp { get; set; }
        public int Error_code { get; set; }
        public string Error_message { get; set; }
        public int Elapsed { get; set; }
        public int Credit_count { get; set; }
    }

    class Quote
    {
        public decimal Price { get; set; }
        public decimal Percent_change_1h { get; set; }
        public decimal Percent_change_24h { get; set; }
        public decimal Market_cap { get; set; }
        public DateTime Last_updated { get; set; }
    }
    class Quotes
    {
        public Quote USD { get; set; }
    }
    class Cryptocurrency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public DateTime Last_updated { get; set; }
        public Quotes Quote { get; set; }
    }
}