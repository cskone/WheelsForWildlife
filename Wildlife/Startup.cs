using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Wildlife.Startup))]
namespace Wildlife
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
