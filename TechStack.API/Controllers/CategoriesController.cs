using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using TechStack.API.Interfaces;
using TechStack.API.Models;

namespace TechStack.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategory category;
        public CategoriesController(ICategory _category)
        {
            category = _category;
        }

        [HttpPost]
        [Route("AddCategories")]
        [Description("Method for Adding categories")]
        public IActionResult AddCategories(CategoryModel cm)
        {
            try
            {
                bool insertResult = category.AddCategories(cm);
                if (insertResult)
                {
                    return Ok("Inserted Succesfully");
                }
                else { return BadRequest(); }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured while processing " + ex.Message);
            }
            

        }
    }
}
