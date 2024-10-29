namespace HSCodeDatabase.DataAccess.Entities;

public class Category
{
    public long CategoryId { get; set; }

    public string? HsCode { get; set; }  

    public string CategoryName { get; set; } = string.Empty;

    public int Level { get; set; }

    public long? ParentCategoryId { get; set; } 

    public Category? ParentCategory { get; set; }  

    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
}

