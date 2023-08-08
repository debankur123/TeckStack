using TechStack.API.Models;

namespace TechStack.API.Interfaces
{
    public interface ICategory
    {
        bool AddCategories(CategoryModel cm);
        List<CategoryModel> GetAllCategories();
        List<CategoryModel>GetCategoryById(int id);
    }
}
