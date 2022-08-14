using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Attendance.Web.Startup))]
namespace Attendance.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
