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
                    return Ok(insertResult);
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

        /// <summary>
        ///     Display single category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("GetCategoryById")]
        [Description("Display category by Id")]
        public ActionResult GetCategoryById(int id)
        {
            try
            {
                var catListById = category.GetCategoryById(id);
                if (catListById == null || catListById.CategoryId == 0)
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
        /// <summary>
        /// Update method for category update
        /// </summary>
        /// <param name="cm"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("UpdateCategories")]
        [Description("Method for Adding categories")]
        public IActionResult UpdateCategories(int id,[FromBody] CategoryModel model)
        {
            try
            {
                bool updatedResult = category.UpdateCategories(id,model);
                if (updatedResult)
                {
                    return Ok(updatedResult);
                }
                else { return BadRequest(); }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured while processing " + ex.Message);
            }
        }
        /// <summary>
        /// Method for Deleting categories
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 

        [HttpPost]
        [Route("DeleteCategories")]
        [Description("Method for Deleting categories")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                bool deletedResult = category.DeleteCategory(id);
                if (deletedResult)
                {
                    return Ok(deletedResult);
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
