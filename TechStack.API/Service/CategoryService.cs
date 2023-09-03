using Microsoft.AspNetCore.Http.HttpResults;
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
        public CategoryModel GetCategoryById(int id) { 
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            CategoryModel cm = new CategoryModel();
            SqlCommand sqlComm = new SqlCommand("USP_TECHSTACK_G_GetCategoryById", con);
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.Parameters.AddWithValue("@Id", id);
            SqlDataReader rdr = sqlComm.ExecuteReader();
            while (rdr.Read())
            {
                cm.CategoryId = id;
                cm.Name = rdr["Name"].ToString();
                cm.URLHandle = rdr["URLHandle"].ToString();
            }
            return cm;
        }
        //public void UpdateCategories(CategoryModel cm,int id)
        //{
        //    SqlConnection con = new SqlConnection(connectionString);
        //    con.Open();
        //    SqlCommand sqlCommand = new SqlCommand("USP_TS_T_UpdateCategories", con);
        //    sqlCommand.CommandType = CommandType.StoredProcedure;
        //    sqlCommand.Parameters.AddWithValue("@Id", id);
        //    sqlCommand.Parameters.AddWithValue("@Name", cm.Name);
        //    sqlCommand.Parameters.AddWithValue("@URLHandle", cm.URLHandle);
        //    sqlCommand.ExecuteNonQuery();
        //}

        public bool UpdateCategories(int id,CategoryModel model)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            try
            {
                SqlCommand cmd  =  new SqlCommand("USP_TS_T_UpdateCategories", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@URLHandle", model.URLHandle);
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        public bool DeleteCategory(int id)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("USP_TS_T_DeleteCategories", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID",id);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
