using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CareHomeMock.Startup))]
namespace CareHomeMock
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
