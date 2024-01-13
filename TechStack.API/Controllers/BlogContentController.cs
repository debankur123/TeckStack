using Microsoft.AspNetCore.Mvc;
using TechStack.API.Interfaces;
using TechStack.API.Models;

namespace TechStack.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogContentController : ControllerBase
    {
        
        private readonly IBlogContent blogContent;
        public BlogContentController(IBlogContent _blogContent)
        {
            blogContent = _blogContent;
        }

        [HttpPost]
        [Route("InsertBlogPostWithContent")]
        public IActionResult InsertBlogPostWithContent([FromBody] BlogPostRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Invalid request data");
                }

                blogContent.InsertBlogPostWithContent(
                    request.BlogModule,
                    request.Content,
                    request.CategoryIds
                );
                    
                return Ok(request);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetAllBlogPosts")]
        public IActionResult GetBlogPosts()
        {
            try
            {
                List<TeckStack_GetAllBlogresponse> blogPosts = (List<TeckStack_GetAllBlogresponse>)blogContent.GetAllBlogPosts();
                return Ok(blogPosts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing: " + ex.Message);
            }
        }

        /// <summary>
        /// GetBlogContentById
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        [HttpGet]
        [Route("GetBlogContentbyId")]
        public IActionResult GetBlogContentById(Int64 Id)
        {
            try
            {
                var response = blogContent.getBlogContentbyId(Id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
