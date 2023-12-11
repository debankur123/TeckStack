namespace TechStack.API.Models
{
    public class GetBlogContentById
    {
        public Int64 BlogPostId            { get; set; }
        public Int64 CategoryId            { get; set; }
        public string CategoryName         { get; set; }
        public string BLogTitle            { get; set; }
        public string ShortBlogDescription { get; set; }
        public string ImageURL             { get; set; }
        public string URLHandle            { get; set; }
        public DateTime DateOfPublish      { get; set; }
        public string BlogAuthor           { get; set; }
        public Boolean BlogVisibleOrNot    { get; set; }
        public Int64 ContentId             { get; set; }
        public string BlogContent          { get; set; }
        public Int64 CreatedById           { get; set; }
    }
}
