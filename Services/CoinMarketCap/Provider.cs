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
        private HttpClient client;

        public IEnumerable<CryptoQuoteModel> GetList()
        {
            List<CryptoQuoteModel> cryptoQuotes = new List<CryptoQuoteModel>();
            using (client = new HttpClient())
            {
                string path = HostingEnvironment.MapPath("/Services/CoinMarketCap/ConnectionSettings.json");
                connectionSettings = JsonConvert.DeserializeObject<ConnectionSettings>(File.ReadAllText(path));
                client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", connectionSettings.ApiKey);
                client.DefaultRequestHeaders.Add("Accepts", "application/json");

                var quotesData = JsonConvert.DeserializeObject<LatestDataResponse>(GetLatestQuotes());
                StringBuilder logoRequest = new StringBuilder();
                foreach (Cryptocurrency c in quotesData.Data)
                {
                    CryptoQuoteModel cq = new CryptoQuoteModel
                    {
                        Id = c.Id,
                        Symbol = c.Symbol,
                        MarketCap = Decimal.Round(c.Quote.USD.MarketCap, 3),
                        PercentChange1h = Decimal.Round(c.Quote.USD.PercentChange1h, 3),
                        PercentChange24h = Decimal.Round(c.Quote.USD.PercentChange24h, 3),
                        Price = Decimal.Round(c.Quote.USD.Price, 3),
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
                    path = "data." + cqm.Id.ToString() + ".logo";
                    cqm.Logo = jObject.SelectToken(path).ToString();
                }
            }
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