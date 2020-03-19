using Awemedia.Admin.AzureFunctions.Business.Helpers;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class EventService : IEventService
    {
        private readonly IBaseService<DAL.DataContracts.Events> _baseService;
        public EventService(IBaseService<DAL.DataContracts.Events> baseService)
        {
            _baseService = baseService;

        }
        public IEnumerable<object> Get(BaseSearchFilter eventSearchFilter, out int totalRecords)
        {
            IQueryable<Event> _events = new List<Event>().AsQueryable();
            totalRecords = 0;
            IQueryable<DAL.DataContracts.Events> events = _baseService.GetAll("EventType").AsQueryable();
            _events = events.Select(t => MappingProfile.MapEventsResponseObject(t)).AsQueryable();
            totalRecords = _events.Count();
            if (eventSearchFilter != null)
            {

                if (!string.IsNullOrEmpty(eventSearchFilter.Search) && !string.IsNullOrEmpty(eventSearchFilter.Type))
                {
                    _events = _events.Search(eventSearchFilter.Type, eventSearchFilter.Search);
                    totalRecords = _events.Count();
                }
                _events = _events.OrderBy(eventSearchFilter.Order + (Convert.ToBoolean(eventSearchFilter.Dir) ? " descending" : ""));
                if (!Convert.ToBoolean(eventSearchFilter.Export))
                {
                    _events = _events.Skip((Convert.ToInt32(eventSearchFilter.Start) - 1) * Convert.ToInt32(eventSearchFilter.Size)).Take(Convert.ToInt32(eventSearchFilter.Size));
                }
                else
                {
                    var dataToExport = _events.Select(e => new { e.Id, e.EventName, e.EventData, e.ChargeStationId, e.DateTime }).AsQueryable();
                    return dataToExport.ToList();
                }
            }
            return _events.ToList();
        }
    }
}
