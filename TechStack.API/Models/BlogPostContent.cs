namespace TechStack.API.Models
{
    public class BlogPostContent
    {
        public  Int64    Id            { get; set; }
        public  Int64    BlogPostId    { get; set; }
        public  string   Content       { get; set; }
        public  Int64    CreatedBy     { get; set; }
        public  DateTime CreatedDate   { get; set; }
        public Int64     ModifiedBy    { get; set; }
        public DateTime  ModifiedDate  { get; set; }
    }


}
