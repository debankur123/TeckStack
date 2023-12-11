namespace TechStack.API.Models
{
    public class BlogPostRequest
    {
        public BlogPostModel         BlogModule  { get; set; }
        public List<BlogPostContent> Content     { get; set; }
        public List<long>            CategoryIds { get; set; }

    }
}
