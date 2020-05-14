using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceAdapter.ExternalAccess.Service;

namespace ServiceAdapter.API.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet("/api/category")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetCategories();
            return Ok(categories);
        }
    }
}