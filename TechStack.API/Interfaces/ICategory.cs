using TechStack.API.Models;

namespace TechStack.API.Interfaces
{
    public interface ICategory
    {
        bool AddCategories(CategoryModel cm);
        List<CategoryModel> GetAllCategories();
        CategoryModel GetCategoryById(int id);
        //void UpdateCategories(CategoryModel cm,int id);
        bool UpdateCategories(int id,CategoryModel cm);
        bool DeleteCategory(int id);
    }
}
