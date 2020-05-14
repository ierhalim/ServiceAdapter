using ServiceAdapter.ExternalAccess.OutputModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceAdapter.ExternalAccess.Service
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryOutputModel>> GetCategories();
    }
}
