using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Security;
using Oqtane.Shared;
using Devessence.Module.OrgChart.Repository;
using Devessence.Module.OrgChart.Models;

namespace Devessence.Module.OrgChart.Services
{
    public class ServerOrgChartService : IOrgChartService
    {
        private readonly IOrgChartRepository _OrgChartRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerOrgChartService(IOrgChartRepository OrgChartRepository, IUserPermissions userPermissions, ITenantManager tenantManager, ILogManager logger, IHttpContextAccessor accessor)
        {
            _OrgChartRepository = OrgChartRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public Task<List<OrgChartItem>> GetOrgChartItemsAsync(int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_OrgChartRepository.GetOrgChartItems(ModuleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized OrgChart Get Attempt {ModuleId}", ModuleId);
                return null;
            }
        }

        public Task<OrgChartItem> GetOrgChartItemAsync(int OrgChartItemId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_OrgChartRepository.GetOrgChartItem(OrgChartItemId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized OrgChart Get Attempt {OrgChartItemId} {ModuleId}", OrgChartItemId, ModuleId);
                return null;
            }
        }

        public Task<OrgChartItem> AddOrgChartItemAsync(OrgChartItem OrgChartItem)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, OrgChartItem.ModuleId, PermissionNames.Edit))
            {
                OrgChartItem = _OrgChartRepository.AddOrgChartItem(OrgChartItem);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "OrgChartItem Added {OrgChartItem}", OrgChartItem);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized OrgChartItem Add Attempt {OrgChartItem}", OrgChartItem);
                OrgChartItem = null;
            }
            return Task.FromResult(OrgChartItem);
        }

        public Task<OrgChartItem> UpdateOrgChartItemAsync(OrgChartItem OrgChartItem)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, OrgChartItem.ModuleId, PermissionNames.Edit))
            {
                OrgChartItem = _OrgChartRepository.UpdateOrgChartItem(OrgChartItem);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "OrgChartItem Updated {OrgChartItem}", OrgChartItem);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized OrgChartItem Update Attempt {OrgChartItem}", OrgChartItem);
                OrgChartItem = null;
            }
            return Task.FromResult(OrgChartItem);
        }

        public Task DeleteOrgChartItemAsync(int OrgChartItemId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                _OrgChartRepository.DeleteOrgChartItem(OrgChartItemId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "OrgChartItem Deleted {OrgChartItemId}", OrgChartItemId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized OrgChartItem Delete Attempt {OrgChartItemId} {ModuleId}", OrgChartItemId, ModuleId);
            }
            return Task.CompletedTask;
        }
    }
}
