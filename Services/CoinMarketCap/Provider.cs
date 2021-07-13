using CryptocurrencyQuotes.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CryptocurrencyQuotes.Services.CoinMarketCap
{ 

    public class Provider : IQuotesProvider
    {
        private int NumbOfCurrencies{ get; set; } = 50;
        private ConnectionSettings connectionSettings;
        static readonly HttpClient client = new HttpClient();

        public Provider()
        {
            string path = HostingEnvironment.MapPath("/Services/CoinMarketCap/ConnectionSettings.json");
            connectionSettings = JsonConvert.DeserializeObject<ConnectionSettings>(File.ReadAllText(path));
            client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", connectionSettings.ApiKey);
            client.DefaultRequestHeaders.Add("Accepts", "application/json");
        }

        public IEnumerable<CryptoQuoteModel> GetList()
        {
 //           var quotesData = JsonConvert.DeserializeObject<LatestDataResponse>(GetLatestQuotes());
            List<CryptoQuoteModel> cryptoQuotes = new List<CryptoQuoteModel>();
            cryptoQuotes.Add(new CryptoQuoteModel
            {
                Id = 1,
                Symbol = "BTC",
                Logo = "https://s2.coinmarketcap.com/static/img/coins/64x64/1.png",
                MarketCap = 617861871024.7037M,
                PercentChange1h = 0.32842557M,
                PercentChange24h = -3.78034997M,
                Price = 32943.87093015334M,
                Name = "Bitcoin",
                LastUpdated = DateTime.Parse("2021 - 07 - 13T06:20:02.000Z")
            });
            /*
            StringBuilder logoRequest = new StringBuilder();
            foreach (Cryptocurrency c in quotesData.Data)
            {
                CryptoQuoteModel cq = new CryptoQuoteModel
                {
                    Id = c.Id,
                    Symbol = c.Symbol,
                    MarketCap = c.Quote.USD.MarketCap,
                    PercentChange1h = c.Quote.USD.PercentChange1h,
                    PercentChange24h = c.Quote.USD.PercentChange24h,
                    Price = c.Quote.USD.Price,
                    Name = c.Name,
                    LastUpdated = c.LastUpdated
                };
                if (logoRequest.Length != 0)
                    logoRequest.Append(',');
                logoRequest.Append(c.Id.ToString());
                cryptoQuotes.Add(cq);
            }
            JObject jObject = JObject.Parse(GetCryptoLogos(logoRequest.ToString()));
            foreach (CryptoQuoteModel cqm in cryptoQuotes)
            {
                string path = "data." + cqm.Id.ToString() + ".logo";
                cqm.Logo = jObject.SelectToken(path).ToString();
            }*/
            return cryptoQuotes;
        }

        private string GetLatestQuotes()
        {
            var URI = new UriBuilder(connectionSettings.LatestQuotesURI);
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["start"] = "1";
            queryString["limit"] = NumbOfCurrencies.ToString();
            queryString["convert"] = "USD";
            URI.Query = queryString.ToString();
            return MakeRequest(URI);
        }

        private string GetCryptoLogos(string requestedId)
        {
            var URI = new UriBuilder(connectionSettings.CryptoInfoURI);
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["id"] = requestedId;
            URI.Query = queryString.ToString();
            return MakeRequest(URI);
        }

        private string MakeRequest(UriBuilder URI)
        {
            HttpResponseMessage response = client.GetAsync(URI.ToString()).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}