namespace TechStack.API.Models
{
    public class BlogPostModel
    {
        public  Int64        Id                      { get; set; }
        public  string       Title                   { get; set; }
        public  string       ShortDescription        { get; set; }
        public  string       FeaturedImageURL        { get; set; }
        public  string       URLHandle               { get; set; }
        public  DateTime     PublishedDate           { get; set; }
        public  string       Author                  { get; set; }
        public  bool         IsVisible               { get; set; }
        public  Int64        CreatedBy               { get; set; }
        public  DateTime     CreatedDate             { get; set; }
        
    }
}
