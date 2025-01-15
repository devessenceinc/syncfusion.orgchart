using Devessence.Module.OrgChart.Models;
using System.Collections.Generic;

namespace Devessence.Module.OrgChart.Repository
{
    public interface IOrgChartRepository
    {
        IEnumerable<OrgChartItem> GetOrgChartItems(int ModuleId);
        OrgChartItem GetOrgChartItem(int OrgChartItemId);
        OrgChartItem GetOrgChartItem(int OrgChartItemId, bool tracking);
        OrgChartItem AddOrgChartItem(OrgChartItem OrgChartItem);
        OrgChartItem UpdateOrgChartItem(OrgChartItem OrgChartItem);
        void DeleteOrgChartItem(int OrgChartItemId);
    }
}
