using System;
using System.Collections.Generic;
using System.Linq;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Interfaces;
using Oqtane.Enums;
using Oqtane.Repository;
using Devessence.Module.OrgChart.Repository;
using System.Threading.Tasks;
using Devessence.Module.OrgChart.Models;
using System.IO;
using Syncfusion.Blazor.Chart3D.Internal;

namespace Devessence.Module.OrgChart.Manager
{
    public class OrgChartManager : MigratableModuleBase, IInstallable, IPortable, ISearchable
    {
        private readonly IOrgChartRepository _OrgChartRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IDBContextDependencies _DBContextDependencies;

        public OrgChartManager(IOrgChartRepository OrgChartRepository, IFileRepository fileRepository, IDBContextDependencies DBContextDependencies)
        {
            _OrgChartRepository = OrgChartRepository;
            _fileRepository = fileRepository;
            _DBContextDependencies = DBContextDependencies;
        }

        public bool Install(Tenant tenant, string version)
        {
            return Migrate(new OrgChartContext(_DBContextDependencies), tenant, MigrationType.Up);
        }

        public bool Uninstall(Tenant tenant)
        {
            return Migrate(new OrgChartContext(_DBContextDependencies), tenant, MigrationType.Down);
        }

        public string ExportModule(Oqtane.Models.Module module)
        {
            string content = "";
            var items = _OrgChartRepository.GetOrgChartItems(module.ModuleId).ToList();
            if (items != null)
            {
                foreach (var item in items)
                {
                    var parent = items.FirstOrDefault(item => item.OrgChartItemId == item.ParentId);

                    var filepath = "";
                    if (item.ImageFileId != -1)
                    {
                        var file = _fileRepository.GetFile(item.ImageFileId);
                        if (file != null)
                        {
                            filepath = file.Folder.Path + file.Name;
                        }
                    }

                    content += item.Name + "|" 
                        + item.Description + "|" 
                        + ((parent != null) ? parent.Name : "") + "|" 
                        + filepath + "\n";
                }
            }
            return content;
        }

        public void ImportModule(Oqtane.Models.Module module, string content, string version)
        {
            if (!string.IsNullOrEmpty(content))
            {
                var rows = content.Trim().Split("\n", StringSplitOptions.RemoveEmptyEntries);
                foreach (var row in rows)
                {
                    var items = _OrgChartRepository.GetOrgChartItems(module.ModuleId).ToList();

                    var cols = row.Split("|");

                    var parentid = -1;
                    if (!string.IsNullOrEmpty(cols[2]))
                    {
                        var parent = items.FirstOrDefault(item => item.Name == cols[2]);
                        if (parent != null)
                        {
                            parentid = items.First(item => item.Name == cols[2]).OrgChartItemId;
                        }
                    }

                    var fileid = -1;
                    if (!string.IsNullOrEmpty(cols[3]))
                    {
                        var folder = "";
                        var filename = cols[3];
                        var pos = filename.LastIndexOf("/");
                        if (pos != -1)
                        {
                            folder = cols[3].Substring(0, pos);
                            filename = cols[3].Substring(pos + 1);
                        }
                        var file = _fileRepository.GetFile(module.SiteId, folder, filename);
                        if (file != null)
                        {
                            fileid = file.FileId;
                        }
                    }

                    var item = items.FirstOrDefault(item => item.Name == cols[0]);
                    if (item == null)
                    {
                        item = new OrgChartItem();
                        item.ModuleId = module.ModuleId;
                        item.Name = cols[0];
                        item.Description = cols[1];
                        item.ParentId = parentid;
                        item.ImageFileId = fileid;
                        _OrgChartRepository.AddOrgChartItem(item);
                    }
                    else
                    {
                        item.Name = cols[0];
                        item.Description = cols[1];
                        item.ParentId = parentid;
                        item.ImageFileId = fileid;
                        _OrgChartRepository.UpdateOrgChartItem(item);
                    }
                }
            }
        }

        public Task<List<SearchContent>> GetSearchContentsAsync(PageModule pageModule, DateTime lastIndexedOn)
        {
           var searchContentList = new List<SearchContent>();

           foreach (var OrgChartItem in _OrgChartRepository.GetOrgChartItems(pageModule.ModuleId))
           {
               if (OrgChartItem.ModifiedOn >= lastIndexedOn)
               {
                   searchContentList.Add(new SearchContent
                   {
                       EntityName = "OrgChartItem",
                       EntityId = OrgChartItem.OrgChartItemId.ToString(),
                       Title = OrgChartItem.Name,
                       Body = OrgChartItem.Name + " - " + OrgChartItem.Description,
                       ContentModifiedBy = OrgChartItem.ModifiedBy,
                       ContentModifiedOn = OrgChartItem.ModifiedOn
                   });
               }
           }

           return Task.FromResult(searchContentList);
        }
    }
}
