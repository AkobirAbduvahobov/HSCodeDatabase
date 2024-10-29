namespace HSCodeDatabase.DataAccess.Entities;

public class SubCategory
{
    public long SubCategoryId { get; set; }
    public string? HsCode { get; set; }
    public string Description { get; set; } = default!;
    public bool IsDefaultCategory { get; set; }
    public long RootCategoryId { get; set; }
    public Root RootCategory { get; set; } = default!;
    public ICollection<SubCategory> SubCategories { get; set; } = default!;
}
