using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Devessence.Module.OrgChart.Models;
using Oqtane.Services;
using Oqtane.Shared;

namespace Devessence.Module.OrgChart.Services
{
    public class OrgChartService : ServiceBase, IOrgChartService
    {
        public OrgChartService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("OrgChart");

        public async Task<List<OrgChartItem>> GetOrgChartItemsAsync(int ModuleId)
        {
            List<OrgChartItem> OrgCharts = await GetJsonAsync<List<OrgChartItem>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId), Enumerable.Empty<OrgChartItem>().ToList());
            return OrgCharts.OrderBy(item => item.Name).ToList();
        }

        public async Task<OrgChartItem> GetOrgChartItemAsync(int OrgChartItemId, int ModuleId)
        {
            return await GetJsonAsync<OrgChartItem>(CreateAuthorizationPolicyUrl($"{Apiurl}/{OrgChartItemId}", EntityNames.Module, ModuleId));
        }

        public async Task<OrgChartItem> AddOrgChartItemAsync(OrgChartItem OrgChart)
        {
            return await PostJsonAsync<OrgChartItem>(CreateAuthorizationPolicyUrl($"{Apiurl}", EntityNames.Module, OrgChart.ModuleId), OrgChart);
        }

        public async Task<OrgChartItem> UpdateOrgChartItemAsync(OrgChartItem OrgChart)
        {
            return await PutJsonAsync<OrgChartItem>(CreateAuthorizationPolicyUrl($"{Apiurl}/{OrgChart.OrgChartItemId}", EntityNames.Module, OrgChart.ModuleId), OrgChart);
        }

        public async Task DeleteOrgChartItemAsync(int OrgChartItemId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{OrgChartItemId}", EntityNames.Module, ModuleId));
        }
    }
}
