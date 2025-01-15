using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using Devessence.Module.OrgChart.Repository;
using Devessence.Module.OrgChart.Services;
using Syncfusion.Blazor;

namespace Devessence.Module.OrgChart.Startup
{
    public class ServerStartup : IServerStartup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NDaF5cWWtCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH5fcXVRRmlfV0N+WUA=");
        }

        public void ConfigureMvc(IMvcBuilder mvcBuilder)
        {
            // not implemented
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IOrgChartService, ServerOrgChartService>();
            services.AddDbContextFactory<OrgChartContext>(opt => { }, ServiceLifetime.Transient);
            services.AddSyncfusionBlazor();
        }
    }
}
