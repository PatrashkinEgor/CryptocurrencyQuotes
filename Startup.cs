using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CryptocurrencyQuotes.Startup))]
namespace CryptocurrencyQuotes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
