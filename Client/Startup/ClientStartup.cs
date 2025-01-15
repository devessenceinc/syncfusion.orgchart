using Microsoft.Extensions.DependencyInjection;
using Oqtane.Services;
using Devessence.Module.OrgChart.Services;
using Syncfusion.Blazor;

namespace Devessence.Module.OrgChart.Startup
{
    public class ClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IOrgChartService, OrgChartService>();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NDaF5cWWtCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH5fcXVRRmlfV0N+WUA=");        
            services.AddSyncfusionBlazor();
        }
    }
}
