using TechStack.API.Models;

namespace TechStack.API.Interfaces
{
    public interface IBlogContent
    {
        void InsertBlogPostWithContent(BlogPostModel bpModel, List<BlogPostContent> content,List<long> categoryIds);
        IEnumerable<TeckStack_GetAllBlogresponse> GetAllBlogPosts();

        List<TeckStack_GetAllBlogresponse> getBlogContentbyId(Int64 Id);
    }
}
