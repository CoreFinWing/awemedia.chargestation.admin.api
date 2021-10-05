using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class CountryService:ICountryService
    {
        private readonly IBaseService<DAL.DataContracts.Country> _baseService;

        public CountryService(IBaseService<DAL.DataContracts.Country> baseService)
        {
            _baseService = baseService;
        }

        public IEnumerable<Country> GetCountries()
        {
            IEnumerable<Country> _country= _baseService.GetAll().Select(t => new Country
            {
                CountryId = t.CountryId,
                CountryName = t.CountryName,
                Currency = t.Currency
            }).ToList();

            return _country.ToList();
        }
    }
}
