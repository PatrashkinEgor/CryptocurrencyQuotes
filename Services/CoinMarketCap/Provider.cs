using CryptocurrencyQuotes.Models;
using System;
using System.IO;
using System.Collections.Generic;
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
        public const int MAX_NUMB_OF_QUOTES = 200;  //Максимальное количество элементов для запроса стоящего 1 кредит в free coinmarketcap = 200
        public const int MIN_NUMB_OF_QUOTES = 1;
        private ConnectionSettings connectionSettings;
        private HttpClient client;

        /// <summary>
        /// Requests quotes and converts them to the format of CryptoQuoteModel
        /// </summary>
        /// <param name="numbOfQuotes">Ammount of quotes to be requested from coinmarketcap.</param>
        /// <returns>
        /// Return list of quotes. </returns>
        public IEnumerable<CryptoQuoteModel> GetList(int numbOfQuotes = 50)
        {

            if (numbOfQuotes > MAX_NUMB_OF_QUOTES)
                numbOfQuotes = MAX_NUMB_OF_QUOTES;

            if (numbOfQuotes < MIN_NUMB_OF_QUOTES)
                numbOfQuotes = 1;

            List<CryptoQuoteModel> cryptoQuotes = new List<CryptoQuoteModel>();

            using (client = new HttpClient())
            {
                //Получаем настройки подключения(API-key и пути) из JSON
                string path = HostingEnvironment.MapPath("/Services/CoinMarketCap/ConnectionSettings.json");    
                connectionSettings = JsonConvert.DeserializeObject<ConnectionSettings>(File.ReadAllText(path));
                //Готовим заголовок
                client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", connectionSettings.ApiKey);
                client.DefaultRequestHeaders.Add("Accepts", "application/json");
                //Запрос котировок в USD, десериализация Json
                var quotesData = JsonConvert.DeserializeObject<LatestDataResponse>(GetLatestQuotes(numbOfQuotes, "USD"));

                ///TODO: Check quotesData.status and handle errors

                //Так как конкретные полученные валюты заранее не известны, создаем строку для записи запроса логотипов полученных валют
                StringBuilder logoRequest = new StringBuilder();

                //Складываем интересующие нас даннае в коллекцию
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

                    //формируем запрос логотипов
                    if (logoRequest.Length != 0)
                        logoRequest.Append(',');
                    logoRequest.Append(c.Id.ToString());
                    cryptoQuotes.Add(cq);
                }

                // Запрос логотипов
                JObject jObject = JObject.Parse(GetCryptosInfo(logoRequest.ToString()));

                ///TODO: Check status and handle errors

                // Раскладываем логотипы
                foreach (CryptoQuoteModel cqm in cryptoQuotes)
                {
                    path = "data." + cqm.Id.ToString() + ".logo";
                    cqm.Logo = jObject.SelectToken(path).ToString();
                }
            }
            return cryptoQuotes;
        }

        /// <summary>
        /// Requests the latest data on quotes.
        /// </summary>
        /// <param name="numbOfQuotes">Ammount of quotes to be requested from coinmarketcap.</param>
        /// <param name="convertTo">Currency in which quotes will be requested .</param>
        /// <returns>
        /// Returns a paginated list of all active cryptocurrencies with latest market data in JSON format. 
        /// </returns>
        private string GetLatestQuotes(int numbOfQuotes, string convertTo)
        {
            var URI = new UriBuilder(connectionSettings.LatestQuotesURI);
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["limit"] = numbOfQuotes.ToString();
            queryString["convert"] = convertTo;
            URI.Query = queryString.ToString();
            return MakeRequest(URI);
        }

        /// <summary>
        /// Requests common information about cryptocurrencies.
        /// </summary>
        /// <param name="requestedId">One or more comma-separated CoinMarketCap cryptocurrency IDs. Example: "1,2".</param>
        /// <returns>
        /// Return information about cryptocurrencies in JSON. 
        /// </returns>
        private string GetCryptosInfo(string requestedId)
        {
            var URI = new UriBuilder(connectionSettings.CryptoInfoURI);
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["id"] = requestedId;
            URI.Query = queryString.ToString();
            return MakeRequest(URI);
        }

        /// <summary>
        /// Send request whith HttpClient.
        /// </summary>
        /// <param name="URI"> URI to request.</param>
        /// <returns>
        /// Return response in string. 
        /// </returns>
        private string MakeRequest(UriBuilder URI)
        {
            ///TODO: Exception Handling
            HttpResponseMessage response = client.GetAsync(URI.ToString()).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}