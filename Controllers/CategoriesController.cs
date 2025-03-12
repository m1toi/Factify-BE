using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.BusinessLogic.Services.CategoryService;
using SocialMediaApp.DataAccess.Dtos.CategoryDto;

namespace SocialMediaApp.Controllers
{
    [Route("api/Categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        
        public CategoriesController (ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<CategoryResponseDto>> GetAll()
        {
            return Ok(_categoryService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<CategoryResponseDto> GetById([FromRoute] int id)
        {
            return Ok(_categoryService.GetById(id));
        }

        [HttpPost]
        public IActionResult Create([FromBody] CategoryRequestDto categoryDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _categoryService.Create(categoryDto);

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] CategoryRequestDto updatedCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _categoryService.Update(id, updatedCategoryDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _categoryService.Delete(id);
            return NoContent();
        }
    }
}   
