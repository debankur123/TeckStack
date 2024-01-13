namespace TechStack.API.Models
{
    public class TeckStack_GetAllBlogresponse
    {
        public BlogPostModel BlogModule { get; set; }
        public List<BlogPostContent> Content { get; set; }
        public List<long> CategoryIds { get; set; }
        public List<string> CategoryNames { get; set; }
    }
}
