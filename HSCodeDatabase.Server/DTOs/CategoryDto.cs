namespace HSCodeDatabase.Server.DTOs;

public class CategoryDto
{
    public long CategoryId { get; set; }
    public string? HsCode { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public long? ParentCategoryId { get; set; }  
    public List<CategoryDto> SubCategories { get; set; } = new List<CategoryDto>();
}
