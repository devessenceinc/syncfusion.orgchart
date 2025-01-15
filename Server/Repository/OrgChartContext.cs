using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Oqtane.Infrastructure;
using Oqtane.Repository.Databases.Interfaces;
using Devessence.Module.OrgChart.Models;

namespace Devessence.Module.OrgChart.Repository
{
    public class OrgChartContext : DBContextBase, ITransientService, IMultiDatabase
    {
        public virtual DbSet<OrgChartItem> OrgChartItem { get; set; }

        public OrgChartContext(IDBContextDependencies DBContextDependencies) : base(DBContextDependencies)
        {
            // ContextBase handles multi-tenant database connections
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<OrgChartItem>().ToTable(ActiveDatabase.RewriteName("OrgChartItem"));
        }
    }
}
