using Awemedia.Admin.AzureFunctions.Business.Helpers;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Newtonsoft.Json;
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
            int count = Convert.ToInt32(Environment.GetEnvironmentVariable("EventsListCountToDisplay"));
            totalRecords = 0;
            IQueryable<DAL.DataContracts.Events> events = null;
            IQueryable<Event> _events = null;
            if (eventSearchFilter != null)
            {
                _events = _baseService.GetAll("EventType").OrderByDescending(a => a.Id).Take(count).AsQueryable().Select(t => MappingProfile.MapEventsResponseObject(t));
                totalRecords = _events.Count();
                if (!string.IsNullOrEmpty(eventSearchFilter.FromDate) && !string.IsNullOrEmpty(eventSearchFilter.ToDate))
                {
                    DateTime fromDate = DateTime.Now.ToUniversalTime();
                    DateTime toDate = DateTime.Now.ToUniversalTime();
                    fromDate = Utility.ParseStartAndEndDates(eventSearchFilter, ref toDate);
                    events = _baseService.GetAll("EventType").Where(a => Convert.ToDateTime(GetFormattedDate(a)).Date >= fromDate && Convert.ToDateTime(GetFormattedDate(a)).Date <= toDate).AsQueryable();
                    _events = events.Select(t => MappingProfile.MapEventsResponseObject(t));
                    totalRecords = _events.Count();
                }

                if (Convert.ToBoolean(eventSearchFilter.Export))
                {
                    var dataToExport = _events.Select(e => new { e.Id, e.EventName, e.EventData, e.ChargeStationId, e.DateTime }).AsQueryable();
                    return dataToExport.ToList();
                }

                if (!string.IsNullOrEmpty(eventSearchFilter.Search) && !string.IsNullOrEmpty(eventSearchFilter.Type))
                {
                    _events = _events.Search(eventSearchFilter.Type, eventSearchFilter.Search);
                    totalRecords = _events.Count();
                }
                _events = _events.Skip((Convert.ToInt32(eventSearchFilter.Start) - 1) * Convert.ToInt32(eventSearchFilter.Size)).Take(Convert.ToInt32(eventSearchFilter.Size)).AsQueryable();
                _events = _events.OrderBy(eventSearchFilter.Order + (Convert.ToBoolean(eventSearchFilter.Dir) ? " descending" : ""));
            }
            else
            {
                events = _baseService.GetAll("EventType").AsQueryable();
                _events = events.Select(t => MappingProfile.MapEventsResponseObject(t));
                totalRecords = _events.Count();
            }
            return _events;
        }

        private static string GetFormattedDate(DAL.DataContracts.Events a)
        {
            return DateTime.ParseExact(a.DateTime.Split(' ')[0], "yyyy:MM:dd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
        }

        public object GetById(int id)
        {
            var data = _baseService.GetAll("EventType").Where(e => e.Id == id).AsQueryable().FirstOrDefault();
            if (data != null)
            {
                object eventData = DBNull.Value;
                if (data.EventType.Name == "video started" || data.EventType.Name == "video ended" || data.EventType.Name == "video failed")
                {
                    eventData = JsonConvert.DeserializeObject(data.EventData);
                }
                else
                {
                    eventData = data.EventData;
                }
                return new { data.Id, data.EventType.Name, data.DeviceId, data.ChargeStationId, data.DateTime, data.IsActive, EventData = eventData };
            }
            return DBNull.Value;
        }
    }
}
