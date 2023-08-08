using Microsoft.Data.SqlClient;
using System.Data;
using TechStack.API.Interfaces;
using TechStack.API.Models;

namespace TechStack.API.Service
{
    public class CategoryService : ICategory
    {
		public IConfiguration Configuration { get; set; }
		private readonly string connectionString;
        public CategoryService(IConfiguration configuration)
        {
            Configuration = configuration;
            connectionString = Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>().BlogDB;
        }
        public bool AddCategories(CategoryModel cm)
        {
            using (SqlConnection sqlComm = new SqlConnection(connectionString))
            {
                sqlComm.Open();
                try
                {
                    using (SqlCommand cmd = new SqlCommand("USP_I_Categories",sqlComm))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name",cm.Name);
                        cmd.Parameters.AddWithValue("URLHandle", cm.URLHandle);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }


        public  List<CategoryModel> GetAllCategories()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlComm = new SqlCommand("USP_TECHSTACK_G_GetAllCategory", con);
                sqlComm.CommandType = CommandType.StoredProcedure;
                List<CategoryModel> catList = new List<CategoryModel>();
                SqlDataAdapter sda = new SqlDataAdapter(sqlComm);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    CategoryModel cat = new CategoryModel();
                    cat.CategoryId = (long)(dr["CategotyId"]);
                    cat.Name = dr["Name"].ToString();
                    cat.URLHandle = dr["URLHandle"].ToString();
                    catList.Add(cat);
                }
                return catList;
            }
        }
        public List<CategoryModel> GetCategoryById(int id) { 
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            List<CategoryModel> catList = new List<CategoryModel>();
            SqlCommand sqlComm = new SqlCommand("USP_TECHSTACK_G_GetCategoryById", con);
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.Parameters.AddWithValue("@Id", id);
            SqlDataAdapter sda = new SqlDataAdapter(sqlComm);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                CategoryModel cat = new CategoryModel();
                cat.Name = dr["Name"].ToString();
                cat.URLHandle = dr["URLHandle"].ToString();
                catList.Add(cat);
            }
            return catList;
        }
    }
}
