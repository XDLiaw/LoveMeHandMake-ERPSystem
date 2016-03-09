using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LoveMeHandMake2.Startup))]
namespace LoveMeHandMake2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
