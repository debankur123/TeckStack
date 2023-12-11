using Microsoft.Data.SqlClient;
using System.Data;
using TechStack.API.Interfaces;
using TechStack.API.Models;

namespace TechStack.API.Service
{
    public class BlogContentService : IBlogContent
    {
        public IConfiguration Configuration { get; set; }
        private readonly string connectionString;
        public BlogContentService(IConfiguration configuration)
        {
            Configuration = configuration; 
            connectionString = Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>().BlogDB;
        }
        public void InsertBlogPostWithContent(
            BlogPostModel bpModel,
            List<BlogPostContent> content,
            List<long> categoryIds)
        {
            using (SqlConnection _connection = new SqlConnection(connectionString))
            {
                _connection.Open();
                SqlCommand cmd = new SqlCommand("USP_I_TechStack_BlogContent", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Title",           bpModel.Title);
                cmd.Parameters.AddWithValue("@ShortDescription",bpModel.ShortDescription);
                cmd.Parameters.AddWithValue("@FeaturedImageURL",bpModel.FeaturedImageURL);
                cmd.Parameters.AddWithValue("@URLHandle",       bpModel.URLHandle);
                cmd.Parameters.AddWithValue("@PublishedDate",   bpModel.PublishedDate);
                cmd.Parameters.AddWithValue("@Author",          bpModel.Author);
                cmd.Parameters.AddWithValue("@IsVisible",       bpModel.IsVisible);
                cmd.Parameters.AddWithValue("@CreatedBy",       bpModel.CreatedBy);
                cmd.Parameters.AddWithValue("@CreatedDate",     bpModel.CreatedDate);

                // Create a DataTable to pass the content as a structured parameter
                DataTable udtContentTable = new DataTable();
                udtContentTable.Columns.Add("Content",     typeof(string));
                udtContentTable.Columns.Add("CreatedBy",   typeof(long));
                udtContentTable.Columns.Add("CreatedDate", typeof(DateTime));
                udtContentTable.Columns.Add("ModifiedBy",  typeof(long));
                udtContentTable.Columns.Add("ModifiedDate",typeof(DateTime));
                 
                foreach (var item in content)
                {
                    udtContentTable.Rows.Add(
                        item.Content,
                        bpModel.CreatedBy,
                        bpModel.CreatedDate,
                        null,
                        null
                    );
                }

                SqlParameter udtParameter = cmd.Parameters.AddWithValue("@UDT_ContentTable", udtContentTable);
                udtParameter.SqlDbType = SqlDbType.Structured;
                udtParameter.TypeName = "dbo.BlogContentUDT";

                // Create a DataTable to pass the categoryIds as a structured parameter
                DataTable udtCategoryIdsTable = new DataTable();
                udtCategoryIdsTable.Columns.Add("CategoryId", typeof(long));

                foreach (var categoryId in categoryIds)
                {
                    udtCategoryIdsTable.Rows.Add(categoryId);
                }

                SqlParameter udtCategoryIdsParameter = cmd.Parameters.AddWithValue("@CategoryIds", udtCategoryIdsTable);
                udtCategoryIdsParameter.SqlDbType = SqlDbType.Structured;
                udtCategoryIdsParameter.TypeName  = "dbo.TVP_BlogCategoryIds";
                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<TeckStack_GetAllBlogresponse> GetAllBlogPosts()
        {
            var blogPostList = new List<TeckStack_GetAllBlogresponse>();
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("USP_TECHSTACK_T_GetAllListOfContent", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var blogPost = new BlogPostModel
                {
                    Id               = reader.GetInt64(reader.GetOrdinal("BlogPostId")),
                    Title            = reader.GetString(reader.GetOrdinal("BLogTitle")),
                    ShortDescription = reader.GetString(reader.GetOrdinal("ShortBlogDescription")),
                    FeaturedImageURL = reader.GetString(reader.GetOrdinal("ImageURL")),
                    URLHandle        = reader.GetString(reader.GetOrdinal("URLHandle")),
                    PublishedDate    = reader.GetDateTime(reader.GetOrdinal("DateOfPublish")),
                    Author           = reader.GetString(reader.GetOrdinal("BlogAuthor")),
                    IsVisible        = reader.GetBoolean(reader.GetOrdinal("BlogVisibleOrNot"))
                };

                var content = new BlogPostContent
                {
                    Id        = reader.GetInt64(reader.GetOrdinal("ContentId")),
                    Content   = reader.GetString(reader.GetOrdinal("BlogContent")),
                    CreatedBy = reader.GetInt64(reader.GetOrdinal("CreatedById"))
                };

                var categoryId       = reader.GetInt64(reader.GetOrdinal("CategoryId"));
                var categoryName     = reader.GetString(reader.GetOrdinal("CategoryName")); 
                var existingResponse = blogPostList.FirstOrDefault(r => r.BlogModule.Id == blogPost.Id);

                if (existingResponse != null)
                {
                    existingResponse.CategoryIds.Add(categoryId);
                    existingResponse.CategoryNames.Add(categoryName);
                }
                else
                {
                    var categoryIds      = new List<long> { categoryId };
                    var categoryNames    = new List<string> { categoryName };
                    var blogPostResponse = new TeckStack_GetAllBlogresponse
                    {
                        BlogModule    = blogPost,
                        Content       = new List<BlogPostContent> { content },
                        CategoryIds   = categoryIds,
                        CategoryNames = categoryNames
                    };
                    blogPostList.Add(blogPostResponse);
                }
            }
            return blogPostList;
        }

        /// <summary>
        /// getBlogContentbyId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 

        public List<TeckStack_GetAllBlogresponse> getBlogContentbyId(Int64 Id)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd   = new SqlCommand("USP_G_BlogContentById", con);
            cmd.CommandType  = CommandType.StoredProcedure;
            var blogPostList = new List<TeckStack_GetAllBlogresponse>();
            cmd.Parameters.AddWithValue("@BlogPostId", Id);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var blogPost = new BlogPostModel
                {
                    Id               = reader.GetInt64(reader.GetOrdinal("BlogPostId")),
                    Title            = reader.GetString(reader.GetOrdinal("BLogTitle")),
                    ShortDescription = reader.GetString(reader.GetOrdinal("ShortBlogDescription")),
                    FeaturedImageURL = reader.GetString(reader.GetOrdinal("ImageURL")),
                    URLHandle        = reader.GetString(reader.GetOrdinal("URLHandle")),
                    PublishedDate    = reader.GetDateTime(reader.GetOrdinal("DateOfPublish")),
                    Author           = reader.GetString(reader.GetOrdinal("BlogAuthor")),
                    IsVisible        = reader.GetBoolean(reader.GetOrdinal("BlogVisibleOrNot"))
                };

                var content = new BlogPostContent
                {
                    Id        = reader.GetInt64(reader.GetOrdinal("ContentId")),
                    Content   = reader.GetString(reader.GetOrdinal("BlogContent")),
                    CreatedBy = reader.GetInt64(reader.GetOrdinal("CreatedById"))
                };

                var categoryId       = reader.GetInt64(reader.GetOrdinal("CategoryId"));
                var categoryName     = reader.GetString(reader.GetOrdinal("CategoryName"));
                var existingResponse = blogPostList.FirstOrDefault(r => r.BlogModule.Id == blogPost.Id);

                if (existingResponse != null)
                {
                    existingResponse.CategoryIds.Add(categoryId);
                    existingResponse.CategoryNames.Add(categoryName);
                }
                else
                {
                    var categoryIds      = new List<long> { categoryId };
                    var categoryNames    = new List<string> { categoryName };
                    var blogPostResponse = new TeckStack_GetAllBlogresponse
                    {
                        BlogModule    = blogPost,
                        Content       = new List<BlogPostContent> { content },
                        CategoryIds   = categoryIds,
                        CategoryNames = categoryNames
                    };
                    blogPostList.Add(blogPostResponse);
                }
            }
            return blogPostList;
        }

        

        //public List<GetBlogContentById> getBlogContentbyId(Int64 Id)
        //{
        //    SqlConnection con = new SqlConnection(connectionString);
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand("USP_G_BlogContentById", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    List<GetBlogContentById> list = new List<GetBlogContentById>();
        //    cmd.Parameters.AddWithValue("@BlogPostId", Id);
        //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
        //    adapter.Fill(dt);
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        GetBlogContentById response = new GetBlogContentById();
        //        response.BlogPostId           = (Int64)(dr["BlogPostId"]);
        //        response.CategoryId           = (Int64)(dr["CategoryId"]);
        //        response.CategoryName         = (string)(dr["CategoryName"]);
        //        response.BLogTitle            = (string)(dr["BLogTitle"]);
        //        response.ShortBlogDescription = (string)(dr["ShortBlogDescription"]);
        //        response.ImageURL             = (string)(dr["ImageURL"]);
        //        response.URLHandle            = (string)(dr["URLHandle"]);
        //        response.DateOfPublish        = Convert.ToDateTime(dr["DateOfPublish"]);
        //        response.BlogAuthor           = (string)(dr["BlogAuthor"]);
        //        response.BlogVisibleOrNot     = (Boolean)(dr["BlogVisibleOrNot"]);
        //        response.ContentId            = (Int64)(dr["ContentId"]);
        //        response.BlogContent          = (string)(dr["BlogContent"]);
        //        response.CreatedById          = (Int64)(dr["CreatedById"]);

        //        list.Add(response);
        //    }
        //    return list;
        //}


    }
}
