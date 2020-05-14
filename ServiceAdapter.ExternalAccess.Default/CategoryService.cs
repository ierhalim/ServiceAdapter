using Newtonsoft.Json;
using ServiceAdapter.ExternalAccess.OutputModel;
using ServiceAdapter.ExternalAccess.Service;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServiceAdapter.ExternalAccess.Default
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        // TODO: Default implementasyon birden fazla müşteride çalışacağı için parametrik olmalı.
        private const string _getCategoriesUri = "https://my.api.mockaroo.com/NorthwindCategory?key=35003520";

        public CategoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<CategoryOutputModel>> GetCategories()
        {
            using (var http = _httpClientFactory.CreateClient())
            {
                var result = await http.GetAsync(_getCategoriesUri);
                if (!result.IsSuccessStatusCode)
                {
                    // TODO: Handle error.
                    throw new HttpRequestException();
                }
                var content = await result.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<IEnumerable<CategoryOutputModel>>(content);
               
                return data;
            }
        }
    }
}
