using Awemedia.Admin.AzureFunctions.Business.Helpers;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class EventService : IEventService
    {
        private readonly IBaseService<DAL.DataContracts.Events> _baseService;
        private readonly string[] navigationalProps = { "EventType" };
        public EventService(IBaseService<DAL.DataContracts.Events> baseService)
        {
            _baseService = baseService;

        }
        public IEnumerable<object> Get(BaseSearchFilter eventSearchFilter, out int totalRecords)
        {
            int count = Convert.ToInt32(Environment.GetEnvironmentVariable("EventsListCountToDisplay"));
            totalRecords = 0;
            IEnumerable<Event> _events = null;
            if (eventSearchFilter != null)
            {
                if (!string.IsNullOrEmpty(eventSearchFilter.FromDate) && !string.IsNullOrEmpty(eventSearchFilter.ToDate))
                {
                    DateTime fromDate = DateTime.Now.ToUniversalTime();
                    DateTime toDate = DateTime.Now.ToUniversalTime();
                    fromDate = Utility.ParseStartAndEndDates(eventSearchFilter, ref toDate);
                    _events = _baseService.Where(a => Convert.ToDateTime(GetFormattedDate(a)).Date >= fromDate && Convert.ToDateTime(GetFormattedDate(a)).Date <= toDate, navigationalProps).Select(t => MappingProfile.MapEventsResponseObject(t)).ToList();
                    totalRecords = _events.Count();
                }
                else
                {
                    Expression<Func<DAL.DataContracts.Events, int>> orderByDesc = x => x.Id;
                    _events = _baseService.GetTopNRecords(count, orderByDesc, "EventType").ToList().Select(t => MappingProfile.MapEventsResponseObject(t));
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
                _events = _events.Skip((Convert.ToInt32(eventSearchFilter.Start) - 1) * Convert.ToInt32(eventSearchFilter.Size)).Take(Convert.ToInt32(eventSearchFilter.Size));
                _events = _events.OrderBy(eventSearchFilter.Order, eventSearchFilter.Dir).ToList();
                return _events;
            }
            else
            {
                return _baseService.GetAll("EventType").Select(t => MappingProfile.MapEventsResponseObject(t)).ToList();
            }
        }

        private static string GetFormattedDate(DAL.DataContracts.Events a)
        {
            return DateTime.ParseExact(a.DateTime.Split(' ')[0], "yyyy:MM:dd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
        }

        public object GetById(int id)
        {
            var data = _baseService.GetById(id, navigationalProps);
            if (data != null)
            {
                object eventData = DBNull.Value;
                var ServerDateTime = Convert.ToDateTime(Utility.ConvertUtcToSpecifiedTimeZone(data.ServerDateTime.Value, Environment.GetEnvironmentVariable("MalaysiaTimeZone")));
                if (data.EventType.Name == "video started" || data.EventType.Name == "video ended" || data.EventType.Name == "video failed")
                {
                    eventData = JsonConvert.DeserializeObject(data.EventData);
                }
                else
                {
                    eventData = data.EventData;
                }
                return new { data.Id, data.EventType.Name, data.DeviceId, data.ChargeStationId, data.DateTime, data.IsActive, EventData = eventData, ServerDateTime };
            }
            return DBNull.Value;
        }
    }
}
