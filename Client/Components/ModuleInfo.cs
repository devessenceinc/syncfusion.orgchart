using Oqtane.Models;
using Oqtane.Modules;
using Oqtane.Shared;
using System.Collections.Generic;

namespace Devessence.Module.OrgChart
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "OrgChart",
            Description = "OrgChart",
            Version = "1.0.0",
            ServerManagerType = "Devessence.Module.OrgChart.Manager.OrgChartManager, Devessence.Module.OrgChart.Server.Oqtane",
            ReleaseVersions = "1.0.0",
            Dependencies = "Devessence.Module.OrgChart.Shared.Oqtane,Syncfusion.Licensing,Syncfusion.Blazor",
            PackageName = "Devessence.Module.OrgChart",
            Resources = new List<Resource>()
            {
                new Resource { ResourceType = ResourceType.Stylesheet, Url = "_content/Syncfusion.Blazor/styles/bootstrap5.css", Location = ResourceLocation.Head },
                new Resource { ResourceType = ResourceType.Script, Url = "_content/Syncfusion.Blazor/scripts/syncfusion-blazor.min.js", Location = ResourceLocation.Head}
            }
        };
    }
}
