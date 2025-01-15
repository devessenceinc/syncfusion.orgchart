using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using Devessence.Module.OrgChart.Models;

namespace Devessence.Module.OrgChart.Repository
{
    public class OrgChartRepository : IOrgChartRepository, ITransientService
    {
        private readonly IDbContextFactory<OrgChartContext> _factory;

        public OrgChartRepository(IDbContextFactory<OrgChartContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<OrgChartItem> GetOrgChartItems(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return db.OrgChartItem.Where(item => item.ModuleId == ModuleId).ToList();
        }

        public OrgChartItem GetOrgChartItem(int OrgChartItemId)
        {
            return GetOrgChartItem(OrgChartItemId, true);
        }

        public OrgChartItem GetOrgChartItem(int OrgChartItemId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.OrgChartItem.Find(OrgChartItemId);
            }
            else
            {
                return db.OrgChartItem.AsNoTracking().FirstOrDefault(item => item.OrgChartItemId == OrgChartItemId);
            }
        }

        public OrgChartItem AddOrgChartItem(OrgChartItem OrgChartItem)
        {
            using var db = _factory.CreateDbContext();
            db.OrgChartItem.Add(OrgChartItem);
            db.SaveChanges();
            return OrgChartItem;
        }

        public OrgChartItem UpdateOrgChartItem(OrgChartItem OrgChartItem)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(OrgChartItem).State = EntityState.Modified;
            db.SaveChanges();
            return OrgChartItem;
        }

        public void DeleteOrgChartItem(int OrgChartItemId)
        {
            using var db = _factory.CreateDbContext();
            OrgChartItem OrgChart = db.OrgChartItem.Find(OrgChartItemId);
            db.OrgChartItem.Remove(OrgChart);
            db.SaveChanges();
        }
    }
}
