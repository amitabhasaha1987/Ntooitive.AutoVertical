using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AdminInterface.Startup))]
namespace AdminInterface
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
