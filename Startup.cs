using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrainingManagementSystem.Startup))]
namespace TrainingManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
