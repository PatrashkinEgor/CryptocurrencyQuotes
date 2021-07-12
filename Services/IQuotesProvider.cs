using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptocurrencyQuotes.Models;

namespace CryptocurrencyQuotes.Services
{
    public interface IQuotesProvider
    {
        IEnumerable<CryptoQuotesModel> GetList();
    }
}
