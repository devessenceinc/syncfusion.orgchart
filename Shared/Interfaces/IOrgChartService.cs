using Devessence.Module.OrgChart.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Devessence.Module.OrgChart.Services
{
    public interface IOrgChartService 
    {
        Task<List<OrgChartItem>> GetOrgChartItemsAsync(int ModuleId);

        Task<OrgChartItem> GetOrgChartItemAsync(int OrgChartItemId, int ModuleId);

        Task<OrgChartItem> AddOrgChartItemAsync(OrgChartItem OrgChartItem);

        Task<OrgChartItem> UpdateOrgChartItemAsync(OrgChartItem OrgChartItem);

        Task DeleteOrgChartItemAsync(int OrgChartItemId, int ModuleId);
    }
}
