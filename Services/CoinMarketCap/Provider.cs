using CryptocurrencyQuotes.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;


namespace CryptocurrencyQuotes.Services.CoinMarketCap
{ 
    class ConnectionSettings
    {
        public string ApiKey { get; set; }
        public string LatestURI { get; set; }
        public string InfoURI { get; set; }
    }

    public class Provider : IQuotesProvider
    {
        private int NumbOfCurrencies{ get; set; } = 50;
        private ConnectionSettings connectionSettings;
        static readonly HttpClient client = new HttpClient();

        public Provider()
        {
            string path = HostingEnvironment.MapPath("/Services/CoinMarketCap/ConnectionSettings.json");
            using (FileStream fs = File.OpenRead(path))
            {
                byte[] array = new byte[fs.Length];
                fs.Read(array, 0, array.Length);
                connectionSettings = JsonConvert.DeserializeObject<ConnectionSettings>(System.Text.Encoding.Default.GetString(array)); 
            }
            
            client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", connectionSettings.ApiKey);
            client.DefaultRequestHeaders.Add("Accepts", "application/json");
        }

        public IEnumerable<CryptoQuotesModel> GetList()
        {
            var quotesData = JsonConvert.DeserializeObject<LatestDataResponse>(MakeAPICall());
            List<CryptoQuotesModel> cryptoQuotes = new List<CryptoQuotesModel>();
            foreach(Cryptocurrency c in quotesData.Data)
            {
                CryptoQuotesModel cq = new CryptoQuotesModel
                {
                    Symbol = c.Symbol,
                    Market_cap = c.Quote.USD.Market_cap,
                    Percent_change_1h = c.Quote.USD.Percent_change_1h,
                    Percent_change_24h = c.Quote.USD.Percent_change_24h,
                    Price = c.Quote.USD.Price,
                    Name = c.Name,
                    Last_updated = c.Last_updated
                };
                cryptoQuotes.Add(cq);
            }
            return cryptoQuotes;
        }

        string MakeAPICall()
        {
            var URI = new UriBuilder(connectionSettings.LatestURI);
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["start"] = "1";
            queryString["limit"] = NumbOfCurrencies.ToString();
            queryString["convert"] = "USD";
            URI.Query = queryString.ToString();

            HttpResponseMessage response =  client.GetAsync(URI.ToString()).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}