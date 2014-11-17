using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CodeHub.Web.Startup))]

namespace CodeHub.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}