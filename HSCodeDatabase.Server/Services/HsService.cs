using HSCodeDatabase.DataAccess;
using HSCodeDatabase.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text.Json;

namespace HSCodeDatabase.Server.Services;

public class HsService : IHsService
{
    private readonly MainContext _context;

    public HsService(MainContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetParentCategoriesAsync(long categoryId)
    {
        var parents = new List<Category>();
        await LoadParentCategoriesAsync(categoryId, parents);
        return parents;
    }

    public async Task<List<string>> GetParentCategoriesAsync(string hsCode)
    {
        var parents = new List<Category>();

        var category = _context.Categories
            .Include(c => c.ParentCategory)
            .FirstOrDefault(c => c.HsCode == hsCode);

        parents.Add(category);

        await LoadParentCategoriesAsync(category.CategoryId, parents);

        List<string> names = new List<string>();
        foreach(var parent in parents)
        {
            names.Add(parent.CategoryName);
        }


        return names;
    }

    private async Task LoadParentCategoriesAsync(long categoryId, List<Category> parents)
    {
        var category = await _context.Categories
            .Include(c => c.ParentCategory)
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

        if (category?.ParentCategory != null)
        {
            parents.Add(category.ParentCategory);
            await LoadParentCategoriesAsync(category.ParentCategory.CategoryId, parents);
        }
    }

}
