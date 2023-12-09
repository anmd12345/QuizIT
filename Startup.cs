using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(QUIZ_IT.Startup))]
namespace QUIZ_IT
{
    public partial class Startup
    {
        public void Configuration (IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}