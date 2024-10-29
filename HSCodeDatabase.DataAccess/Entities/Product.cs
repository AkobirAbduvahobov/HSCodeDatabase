namespace HSCodeDatabase.DataAccess.Entities;

public class Product
{
    public long ProductId { get; set; }

    public string HsCode { get; set; }

    public string Description { get; set; }

    public long SubCategoryId { get; set; }

    public SubCategory SubCategory { get; set; }
}
