using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Devessence.Module.OrgChart.Repository;
using Oqtane.Controllers;
using System.Net;
using Devessence.Module.OrgChart.Models;

namespace Devessence.Module.OrgChart.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class OrgChartController : ModuleControllerBase
    {
        private readonly IOrgChartRepository _OrgChartRepository;

        public OrgChartController(IOrgChartRepository OrgChartRepository, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _OrgChartRepository = OrgChartRepository;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public IEnumerable<OrgChartItem> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return _OrgChartRepository.GetOrgChartItems(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized OrgChartItem Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public OrgChartItem Get(int id)
        {
            OrgChartItem OrgChartItem = _OrgChartRepository.GetOrgChartItem(id);
            if (OrgChartItem != null && IsAuthorizedEntityId(EntityNames.Module, OrgChartItem.ModuleId))
            {
                return OrgChartItem;
            }
            else
            { 
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized OrgChartItem Get Attempt {OrgChartItemId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public OrgChartItem Post([FromBody] OrgChartItem OrgChartItem)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, OrgChartItem.ModuleId))
            {
                OrgChartItem = _OrgChartRepository.AddOrgChartItem(OrgChartItem);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "OrgChartItem Added {OrgChartItem}", OrgChartItem);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized OrgChart Post Attempt {OrgChartItem}", OrgChartItem);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                OrgChartItem = null;
            }
            return OrgChartItem;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public OrgChartItem Put(int id, [FromBody] OrgChartItem OrgChartItem)
        {
            if (ModelState.IsValid && OrgChartItem.OrgChartItemId == id && IsAuthorizedEntityId(EntityNames.Module, OrgChartItem.ModuleId) && _OrgChartRepository.GetOrgChartItem(OrgChartItem.OrgChartItemId, false) != null)
            {
                OrgChartItem = _OrgChartRepository.UpdateOrgChartItem(OrgChartItem);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "OrgChartItem Updated {OrgChartItem}", OrgChartItem);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized OrgChartItem Put Attempt {OrgChartItem}", OrgChartItem);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                OrgChartItem = null;
            }
            return OrgChartItem;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public void Delete(int id)
        {
            OrgChartItem OrgChartItem = _OrgChartRepository.GetOrgChartItem(id);
            if (OrgChartItem != null && IsAuthorizedEntityId(EntityNames.Module, OrgChartItem.ModuleId))
            {
                _OrgChartRepository.DeleteOrgChartItem(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "OrgChart Deleted {OrgChartItemId}", id);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized OrgChart Delete Attempt {OrgChartItemId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}
