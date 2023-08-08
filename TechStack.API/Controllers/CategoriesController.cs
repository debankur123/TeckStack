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
                    return Ok();
                }
                else { return BadRequest(); }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured while processing " + ex.Message);
            }
        }


        // Route : https://localhost:44307/api/Categories/GetAllCategories
        // "node_modules/font-awesome/css/font-awesome.min.css",
        [HttpGet]
        [Route("GetAllCategories")]
        [Description("Get all category list")]
        public IActionResult GetAllCategories()
        {
            try
            {
                var catList =  category.GetAllCategories();
                return Ok(catList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured while processing " + ex.Message);
            }
        }


        [HttpGet]
        [Route("GetCategoryById")]
        [Description("Display category by Id")]
        public ActionResult GetCategoryById(int id)
        {
            try
            {
                var catListById = category.GetCategoryById(id);
                if (catListById == null || catListById.Count == 0)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(catListById);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured while processing " + ex.Message);
            }
        }
    }
}
