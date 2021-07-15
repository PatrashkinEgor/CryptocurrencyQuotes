using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptocurrencyQuotes.Services
{
    [Serializable]
    public class ServiceResponceException : ApplicationException
    {
        public ServiceResponceException() { }
        public ServiceResponceException(string message) : base(message) { }
        public ServiceResponceException(string message, Exception ex) : base(message) { }
        // Конструктор для обработки сериализации типа
        protected ServiceResponceException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext contex)
            : base(info, contex) { }
    }
}