using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bakery_Shop.Startup))]
namespace Bakery_Shop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
