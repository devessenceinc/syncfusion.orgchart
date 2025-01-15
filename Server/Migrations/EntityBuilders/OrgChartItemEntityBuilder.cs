using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace Devessence.Module.OrgChart.Migrations.EntityBuilders
{
    public class OrgChartItemEntityBuilder : AuditableBaseEntityBuilder<OrgChartItemEntityBuilder>
    {
        private const string _entityTableName = "OrgChartItem";
        private readonly PrimaryKey<OrgChartItemEntityBuilder> _primaryKey = new("PK_OrgChartItem", x => x.OrgChartItemId);
        private readonly ForeignKey<OrgChartItemEntityBuilder> _moduleForeignKey = new("FK_OrgChartItem_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public OrgChartItemEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
        }

        protected override OrgChartItemEntityBuilder BuildTable(ColumnsBuilder table)
        {
            OrgChartItemId = AddAutoIncrementColumn(table, "OrgChartItemId");
            ModuleId = AddIntegerColumn(table,"ModuleId");
            Name = AddStringColumn(table, "Name", 256);
            Description = AddStringColumn(table, "Description", 512);
            ParentId = AddIntegerColumn(table, "ParentId");
            ImageFileId = AddIntegerColumn(table, "ImageFileId");
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> OrgChartItemId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> Name { get; set; }
        public OperationBuilder<AddColumnOperation> Description { get; set; }
        public OperationBuilder<AddColumnOperation> ParentId { get; set; }
        public OperationBuilder<AddColumnOperation> ImageFileId { get; set; }
    }
}
