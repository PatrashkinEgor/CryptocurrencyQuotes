using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

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
        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }
        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }
        public int Elapsed { get; set; }
        [JsonProperty("credit_count")]
        public int CreditCount { get; set; }
    }

    class Quote
    {
        public decimal Price { get; set; }
        [JsonProperty("percent_change_1h")]
        public decimal PercentChange1h { get; set; }
        [JsonProperty("percent_change_24h")]
        public decimal PercentChange24h { get; set; }
        [JsonProperty("market_cap")]
        public decimal MarketCap { get; set; }
        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }
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
        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }
        public Quotes Quote { get; set; }
    }
}