using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlTypes;
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
    }
}
