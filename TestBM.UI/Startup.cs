using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestBM.UI.Startup))]
namespace TestBM.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
