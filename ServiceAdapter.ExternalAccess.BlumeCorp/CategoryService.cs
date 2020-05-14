using Newtonsoft.Json;
using ServiceAdapter.ExternalAccess.BlumeCorp.InputModel;
using ServiceAdapter.ExternalAccess.OutputModel;
using ServiceAdapter.ExternalAccess.Service;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServiceAdapter.ExternalAccess.BlumeCorp
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CategoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<CategoryOutputModel>> GetCategories()
        {
            using (var http = _httpClientFactory.CreateClient())
            {
                var result = await http.GetAsync("https://my.api.mockaroo.com/BlumeCategory?key=35003520");
                if (!result.IsSuccessStatusCode)
                {
                    // TODO: Handle error.
                    throw new HttpRequestException();
                }
                var content = await result.Content.ReadAsStringAsync();
                var input = JsonConvert.DeserializeObject<IEnumerable<BlumeCorpCategoryModel>>(content);
                var output = input.Select(item => new CategoryOutputModel()
                {
                    Id = item.Id.ToString(),
                    Name = item.Title,
                    Description = item.Information
                });
                return output;
            }
        }
    }
}
