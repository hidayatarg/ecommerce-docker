using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebMvc.Models;
using Microsoft.Extensions.Options;
using WebMvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebMvc.Services
{
    public class CatalogService: ICatalogService
    {
        private readonly IOptionsSnapshot<AppSettings> _settings;
        private readonly IHttpClient _apiClient;
        private readonly ILogger<CatalogService> _logger;

        private readonly string _remoteServiceBaseUrl;

        // Dependency Injection
        public CatalogService(IOptionsSnapshot<AppSettings> settings, IHttpClient httpClient, ILogger<CatalogService> logger)
        {
            this._settings = settings;
            this._apiClient = httpClient;
            this._logger = logger;

            _remoteServiceBaseUrl = $"{_settings.Value.CatalogUrl}/api/catalog";
        }

        public async Task<Catalog> GetCatalogItems(int page, int take, int? brand, int? type)
        {
            var allCatalogItemsUri = ApiPaths.Catalog.GetAllCatalogItems(_remoteServiceBaseUrl, page, take, brand, type);
            // return full path
            var dataString = await _apiClient.GetStringAsync(allCatalogItemsUri);
            // catalog data
            var response = JsonConvert.DeserializeObject<Catalog>(dataString);
            return response;
        }

        public async Task<IEnumerable<SelectListItem>> GetBrands()
        {
            // call to api
            var getBrandsUri = ApiPaths.Catalog.GetAllBrands(_remoteServiceBaseUrl);
            // data
            var dataString = await _apiClient.GetStringAsync(getBrandsUri);
            // view used in razor view
            var items = new List<SelectListItem>
            {
                new SelectListItem() { Value = null, Text = "All", Selected = true }
            };
            // parsing data
            var brands = JArray.Parse(dataString);

            foreach (var brand in brands.Children<JObject>())
            {
                items.Add(new SelectListItem()
                {
                    Value = brand.Value<string>("id"),
                    Text = brand.Value<string>("brand")
                });
            }

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetTypes()
        {
            var getTypesUri = ApiPaths.Catalog.GetAllTypes(_remoteServiceBaseUrl);

            var dataString = await _apiClient.GetStringAsync(getTypesUri);

            var items = new List<SelectListItem>
            {
                new SelectListItem() { Value = null, Text = "All", Selected = true }
            };
            var brands = JArray.Parse(dataString);
            foreach (var brand in brands.Children<JObject>())
            {
                items.Add(new SelectListItem()
                {
                    Value = brand.Value<string>("id"),
                    Text = brand.Value<string>("type")
                });
            }

            return items;
        }
    }
}
