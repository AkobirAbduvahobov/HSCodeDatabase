using HSCodeDatabase.DataAccess.Entities;
using HSCodeDatabase.Server.DTOs;


namespace HSCodeDatabase.Server.Services;

public interface IBaseService
{
    //long InsertRoot(RootDto root);
    long InsertCategory(CategoryDto category);
    void FillCategoryTable();

    Task<List<Category>> GetAllParentCategories(string hsCode);
    Task<List<string>> GetAllParentCategoriesDescriptions(string hsCode);
    //long InsertSubCategory(SubCategoryDto subCategory);
    //long InsertProduct(ProductDto product);
}
