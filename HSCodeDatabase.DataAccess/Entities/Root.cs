namespace HSCodeDatabase.DataAccess.Entities;

public class Root
{
    public long RootId { get; set; }
    public string HsCode { get; set; } = default!;
    public string Description { get; set; } = default!;
    public ICollection<SubCategory> SubCategories { get; set; } = default!;
}
