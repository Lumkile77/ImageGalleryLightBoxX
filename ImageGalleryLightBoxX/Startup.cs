using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImageGalleryLightBoxX.Startup))]
namespace ImageGalleryLightBoxX
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
