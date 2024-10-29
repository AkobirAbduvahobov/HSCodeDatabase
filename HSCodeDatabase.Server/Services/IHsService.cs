using HSCodeDatabase.DataAccess.Entities;

namespace HSCodeDatabase.Server.Services;

public interface IHsService
{
    Task<List<string>> GetParentCategoriesAsync(string hsCode);
}
