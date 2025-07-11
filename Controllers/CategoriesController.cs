﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.BusinessLogic.Services.CategoryService;
using SocialMediaApp.DataAccess.Dtos.CategoryDto;

namespace SocialMediaApp.Controllers
{
    [Authorize]    
	[Route("api/Categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        
        public CategoriesController (ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

		[AllowAnonymous]
		[HttpGet]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<CategoryResponseDto>> GetAll()
        {
            return Ok(_categoryService.GetAll());
        }

		[AllowAnonymous]
		[HttpGet("{id}")]
        public ActionResult<CategoryResponseDto> GetById([FromRoute] int id)
        {
            return Ok(_categoryService.GetById(id));
        }

        [HttpPost]
		public IActionResult Create([FromBody] CategoryRequestDto categoryDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				_categoryService.Create(categoryDto);
				return Ok();
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("already exists"))
					return Conflict(new { error = "Category already exists" });
				return BadRequest(new { error = "Could not create category. Please try again." });
			}
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
