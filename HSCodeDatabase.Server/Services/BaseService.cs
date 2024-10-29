using HSCodeDatabase.DataAccess;
using HSCodeDatabase.DataAccess.Entities;
using HSCodeDatabase.Server.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HSCodeDatabase.Server.Services;

public class BaseService : IBaseService
{
    private readonly MainContext _context;

    public BaseService(MainContext context)
    {
        _context = context;
    }



    public async Task<List<Category>> GetAllParentCategories(string hsCode)
    {
        var ancestors = new List<Category>();

        var currentCategory = _context.Categories
            .Include(c => c.ParentCategory)
            .FirstOrDefault(c => c.HsCode == hsCode);

        if (currentCategory == null) return ancestors;

        await LoadParentsRecursive(currentCategory, ancestors);

        return ancestors;
    }


    private async Task LoadParentsRecursive(Category? category, List<Category> parents)
    {
        if (category == null) return;

        parents.Add(category);

        var nextParent = await _context.Categories
            .Include(c => c.ParentCategory)
            .FirstOrDefaultAsync(c => c.CategoryId == category.ParentCategoryId);

        await LoadParentsRecursive(nextParent, parents);
    }

    private async Task<List<string>> LoadParentsRecursive(Category? category, List<string> descs)
    {
        if (category == null) return descs;

        descs.Add(category.CategoryName);

        var nextParent = await _context.Categories
            .Include(c => c.ParentCategory)
            .FirstOrDefaultAsync(c => c.CategoryId == category.ParentCategoryId);

        var res = await LoadParentsRecursive(nextParent, descs);

        return res;
    }


    public async Task<List<string>> GetAllParentCategoriesDescriptions(string hsCode)
    {
        var descriptions = new List<string>();

        var currentCategory = _context.Categories
            .Include(c => c.ParentCategory)
            .FirstOrDefault(c => c.HsCode == hsCode);

        await LoadParentsRecursive(currentCategory, descriptions);

        return descriptions;
    }


    private static CategoryDto MapToCategoryDto(Category category)
    {
        return new CategoryDto
        {
            CategoryId = category.CategoryId,
            HsCode = category.HsCode,
            CategoryName = category.CategoryName,
            ParentCategoryId = category.ParentCategoryId,
            SubCategories = category.SubCategories
                .Select(MapToCategoryDto)
                .ToList()
        };
    }
    private static Category MapToCategoryEntity(CategoryDto dto)
    {
        return new Category
        {
            HsCode = dto.HsCode,
            CategoryName = dto.CategoryName,
            ParentCategoryId = dto.ParentCategoryId
        };
    }






    public void FillCategoryTable()
    {
        string inputFilePath = @"C:\Users\user\source\repos\ConsoleApp1\ConsoleApp1\GoldData.txt";
        string outputFilePath = @"C:\Users\user\source\repos\ConsoleApp1\ConsoleApp1\output6.txt";


        using (StreamReader reader = new StreamReader(inputFilePath, Encoding.UTF8))
        using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            string line;
            int sanoq = 20973; // 17216 // 18724
            while ((line = reader.ReadLine()) != null)
            {

                var isRoot11 = IsRoot11(line);
                var isRoot10 = IsRoot10(line);

                if (isRoot10 is true)
                {
                    ++sanoq;
                }
                else if (isRoot11 is true)
                {
                    var category = Root11(line);
                    category.ParentCategoryId = sanoq;

                    _context.Categories.Add(category);
                    _context.SaveChanges();
                }



            }
        }

        //using (StreamReader reader = new StreamReader(inputFilePath, Encoding.UTF8))
        //using (StreamWriter writer = new StreamWriter(outputFilePath))
        //{
        //    string line;
        //    int sanoq = 12921;
        //    while ((line = reader.ReadLine()) != null)
        //    {

        //        var isRoot5 = IsRoot1(line);
        //        var isRoot6 = IsRoot2(line);    

        //        if (isRoot5 is true)
        //        {
        //            ++sanoq;
        //        }
        //        else if(isRoot6 is true)
        //        {
        //            var category = Root6(line);
        //            category.ParentCategoryId = sanoq;

        //            _context.Categories.Add(category);
        //            _context.SaveChanges();
        //        }



        //    }
        //}
    }




    public bool IsRoot11(string line)
    {
        int sanoq = 0;
        for (int i = 0; i < line.Length; i++)
        {
            if (Char.IsDigit(line[i]) || line[i] == ' ')
            {
                continue;
            }
            else if (line[i] == '–')
            {
                ++sanoq;
            }
            else
            {
                if (sanoq == 10)
                {
                    return true;
                }
                else break;
            }
        }

        return false;
    }


    public Category Root11(string line)
    {
        string s1 = "";
        string s2 = "";

        for (int i = 0; i < line.Length; i++)
        {
            if ((Char.IsDigit(line[i]) || line[i] == ' ') && 14 >= s1.Length)
            {
                s1 += line[i];
            }
            else if (line[i] == '–' || line[i] == ' ')
            {
                continue;
            }
            else
            {
                s2 = line.Substring(i);
                break;
            }
        }



        var category = new Category()
        {
            CategoryName = s2,
            Level = 11,
        };

        if (s1.IsNullOrEmpty())
        {
            category.HsCode = null;
            return category;
        }


        while (s1[s1.Length - 1] == ' ')
        {
            s1 = s1.Remove(s1.Length - 1, 1);
            if (s1.IsNullOrEmpty())
            {
                category.HsCode = null;
                return category;
            }
        }

        if (s1.IsNullOrEmpty() || string.IsNullOrWhiteSpace(s1))
        {
            category.HsCode = null;
        }
        else
        {
            category.HsCode = s1;
        }

        return category;
    }



    public Category Root10(string line)
    {
        string s1 = "";
        string s2 = "";

        for (int i = 0; i < line.Length; i++)
        {
            if ((Char.IsDigit(line[i]) || line[i] == ' ') && 14 >= s1.Length)
            {
                s1 += line[i];
            }
            else if (line[i] == '–' || line[i] == ' ')
            {
                continue;
            }
            else
            {
                s2 = line.Substring(i);
                break;
            }
        }



        var category = new Category()
        {
            CategoryName = s2,
            Level = 10,
        };

        if (s1.IsNullOrEmpty())
        {
            category.HsCode = null;
            return category;
        }


        while (s1[s1.Length - 1] == ' ')
        {
            s1 = s1.Remove(s1.Length - 1, 1);
            if (s1.IsNullOrEmpty())
            {
                category.HsCode = null;
                return category;
            }
        }

        if (s1.IsNullOrEmpty() || string.IsNullOrWhiteSpace(s1))
        {
            category.HsCode = null;
        }
        else
        {
            category.HsCode = s1;
        }

        return category;
    }

    public bool IsRoot10(string line)
    {
        int sanoq = 0;
        for (int i = 0; i < line.Length; i++)
        {
            if (Char.IsDigit(line[i]) || line[i] == ' ')
            {
                continue;
            }
            else if (line[i] == '–')
            {
                ++sanoq;
            }
            else
            {
                if (sanoq == 9)
                {
                    return true;
                }
                else break;
            }
        }

        return false;
    }









    public Category Root9(string line)
    {
        string s1 = "";
        string s2 = "";

        for (int i = 0; i < line.Length; i++)
        {
            if ((Char.IsDigit(line[i]) || line[i] == ' ') && 14 >= s1.Length)
            {
                s1 += line[i];
            }
            else if (line[i] == '–' || line[i] == ' ')
            {
                continue;
            }
            else
            {
                s2 = line.Substring(i);
                break;
            }
        }



        var category = new Category()
        {
            CategoryName = s2,
            Level = 9,
        };

        if (s1.IsNullOrEmpty())
        {
            category.HsCode = null;
            return category;
        }


        while (s1[s1.Length - 1] == ' ')
        {
            s1 = s1.Remove(s1.Length - 1, 1);
            if (s1.IsNullOrEmpty())
            {
                category.HsCode = null;
                return category;
            }
        }

        if (s1.IsNullOrEmpty() || string.IsNullOrWhiteSpace(s1))
        {
            category.HsCode = null;
        }
        else
        {
            category.HsCode = s1;
        }

        return category;
    }

    public bool IsRoot9(string line)
    {
        int sanoq = 0;
        for (int i = 0; i < line.Length; i++)
        {
            if (Char.IsDigit(line[i]) || line[i] == ' ')
            {
                continue;
            }
            else if (line[i] == '–')
            {
                ++sanoq;
            }
            else
            {
                if (sanoq == 8)
                {
                    return true;
                }
                else break;
            }
        }

        return false;
    }






    public Category Root8(string line)
    {
        string s1 = "";
        string s2 = "";

        for (int i = 0; i < line.Length; i++)
        {
            if ((Char.IsDigit(line[i]) || line[i] == ' ') && 14 >= s1.Length)
            {
                s1 += line[i];
            }
            else if (line[i] == '–' || line[i] == ' ')
            {
                continue;
            }
            else
            {
                s2 = line.Substring(i);
                break;
            }
        }



        var category = new Category()
        {
            CategoryName = s2,
            Level = 8,
        };

        if (s1.IsNullOrEmpty())
        {
            category.HsCode = null;
            return category;
        }


        while (s1[s1.Length - 1] == ' ')
        {
            s1 = s1.Remove(s1.Length - 1, 1);
            if (s1.IsNullOrEmpty())
            {
                category.HsCode = null;
                return category;
            }
        }

        if (s1.IsNullOrEmpty() || string.IsNullOrWhiteSpace(s1))
        {
            category.HsCode = null;
        }
        else
        {
            category.HsCode = s1;
        }

        return category;
    }

    public bool IsRoot8(string line)
    {
        int sanoq = 0;
        for (int i = 0; i < line.Length; i++)
        {
            if (Char.IsDigit(line[i]) || line[i] == ' ')
            {
                continue;
            }
            else if (line[i] == '–')
            {
                ++sanoq;
            }
            else
            {
                if (sanoq == 7)
                {
                    return true;
                }
                else break;
            }
        }

        return false;
    }

    public Category Root7(string line)
    {
        string s1 = "";
        string s2 = "";

        for (int i = 0; i < line.Length; i++)
        {
            if ((Char.IsDigit(line[i]) || line[i] == ' ') && 14 >= s1.Length)
            {
                s1 += line[i];
            }
            else if (line[i] == '–' || line[i] == ' ')
            {
                continue;
            }
            else
            {
                s2 = line.Substring(i);
                break;
            }
        }



        var category = new Category()
        {
            CategoryName = s2,
            Level = 7,
        };

        if (s1.IsNullOrEmpty())
        {
            category.HsCode = null;
            return category;
        }


        while (s1[s1.Length - 1] == ' ')
        {
            s1 = s1.Remove(s1.Length - 1, 1);
            if (s1.IsNullOrEmpty())
            {
                category.HsCode = null;
                return category;
            }
        }

        if (s1.IsNullOrEmpty() || string.IsNullOrWhiteSpace(s1))
        {
            category.HsCode = null;
        }
        else
        {
            category.HsCode = s1;
        }

        return category;
    }

    public bool IsRoot7(string line)
    {
        int sanoq = 0;
        for (int i = 0; i < line.Length; i++)
        {
            if (Char.IsDigit(line[i]) || line[i] == ' ')
            {
                continue;
            }
            else if (line[i] == '–')
            {
                ++sanoq;
            }
            else
            {
                if (sanoq == 6)
                {
                    return true;
                }
                else break;
            }
        }

        return false;
    }

    public Category Root6(string line)
    {
        string s1 = "";
        string s2 = "";

        for (int i = 0; i < line.Length; i++)
        {
            if ((Char.IsDigit(line[i]) || line[i] == ' ') && 14 >= s1.Length)
            {
                s1 += line[i];
            }
            else if (line[i] == '–' || line[i] == ' ')
            {
                continue;
            }
            else
            {
                s2 = line.Substring(i);
                break;
            }
        }



        var category = new Category()
        {
            CategoryName = s2,
            Level = 6,
        };

        if (s1.IsNullOrEmpty())
        {
            category.HsCode = null;
            return category;
        }


        while (s1[s1.Length - 1] == ' ')
        {
            s1 = s1.Remove(s1.Length - 1, 1);
            if (s1.IsNullOrEmpty())
            {
                category.HsCode = null;
                return category;
            }
        }

        if (s1.IsNullOrEmpty() || string.IsNullOrWhiteSpace(s1))
        {
            category.HsCode = null;
        }
        else
        {
            category.HsCode = s1;
        }

        return category;
    }

    public bool IsRoot6(string line)
    {
        int sanoq = 0;
        for (int i = 0; i < line.Length; i++)
        {
            if (Char.IsDigit(line[i]) || line[i] == ' ')
            {
                continue;
            }
            else if (line[i] == '–')
            {
                ++sanoq;
            }
            else
            {
                if (sanoq == 5)
                {
                    return true;
                }
                else break;
            }
        }

        return false;
    }

    public bool IsRoot4(string line)
    {
        int sanoq = 0;
        bool check = true;
        for (int i = 0; i < line.Length; i++)
        {
            if ((Char.IsDigit(line[i]) || line[i] == ' ') && check)
            {
                continue;
            }
            else if (line[i] == ' ')
            {
                continue;
            }
            else if (line[i] == '–')
            {
                ++sanoq;
            }
            else
            {
                if (sanoq == 3)
                {
                    return true;
                }
                else break;
            }
        }

        return false;
    }

    public Category Root5(string line)
    {
        string s1 = "";
        string s2 = "";

        for (int i = 0; i < line.Length; i++)
        {
            if ((Char.IsDigit(line[i]) || line[i] == ' ') && 14 >= s1.Length)
            {
                s1 += line[i];
            }
            else if (line[i] == '–' || line[i] == ' ')
            {
                continue;
            }
            else
            {
                s2 = line.Substring(i);
                break;
            }
        }



        var category = new Category()
        {
            CategoryName = s2,
            Level = 5,
        };

        if (s1.IsNullOrEmpty())
        {
            category.HsCode = null;
            return category;
        }


        while (s1[s1.Length - 1] == ' ')
        {
            s1 = s1.Remove(s1.Length - 1, 1);
            if (s1.IsNullOrEmpty())
            {
                category.HsCode = null;
                return category;
            }
        }

        if (s1.IsNullOrEmpty() || string.IsNullOrWhiteSpace(s1))
        {
            category.HsCode = null;
        }
        else
        {
            category.HsCode = s1;
        }

        return category;
    }

    public bool IsRoot5(string line)
    {
        int sanoq = 0;
        for (int i = 0; i < line.Length; i++)
        {
            if (Char.IsDigit(line[i]) || line[i] == ' ')
            {
                continue;
            }
            else if (line[i] == '–')
            {
                ++sanoq;
            }
            else
            {
                if (sanoq == 4)
                {
                    return true;
                }
                else break;
            }
        }

        return false;
    }

    public Category Root4(string line)
    {
        string s1 = "";
        string s2 = "";

        for (int i = 0; i < line.Length; i++)
        {
            if ((Char.IsDigit(line[i]) || line[i] == ' ') && 14 >= s1.Length)
            {
                s1 += line[i];
            }
            else if (line[i] == '–' || line[i] == ' ')
            {
                continue;
            }
            else
            {
                s2 = line.Substring(i);
                break;
            }
        }



        var category = new Category()
        {
            CategoryName = s2,
            Level = 4,
        };

        if (s1.IsNullOrEmpty())
        {
            category.HsCode = null;
            return category;
        }


        while (s1[s1.Length - 1] == ' ')
        {
            s1 = s1.Remove(s1.Length - 1, 1);
            if (s1.IsNullOrEmpty())
            {
                category.HsCode = null;
                return category;
            }
        }

        if (s1.IsNullOrEmpty() || string.IsNullOrWhiteSpace(s1))
        {
            category.HsCode = null;
        }
        else
        {
            category.HsCode = s1;
        }

        return category;
    }

    public Category Root1(string line)
    {
        string s1 = "";
        string s2 = "";

        for (int i = 0; i < line.Length; i++)
        {
            if (Char.IsDigit(line[i]) || line[i] == ' ')
            {
                s1 += line[i];
            }
            else
            {
                s2 = line.Substring(i);
                break;
            }
        }

        while (s1[s1.Length - 1] == ' ')
        {
            s1 = s1.Remove(s1.Length - 1, 1);
        }

        var category = new Category()
        {
            HsCode = s1,
            CategoryName = s2,
            Level = 1,
        };

        return category;
    }

    public bool? IsRoot1(string line)
    {
        if (!Char.IsDigit(line[0])) return false;

        for (int i = 0; i < line.Length; i++)
        {
            if (Char.IsDigit(line[i]) || line[i] == ' ')
            {
                continue;
            }
            else
            {
                if (Char.IsLetter(line[i])) return true;
                else return false;
            }
        }

        return null;
    }


    public Category Root2(string line)
    {
        string s1 = "";
        string s2 = "";

        for (int i = 0; i < line.Length; i++)
        {
            if (Char.IsDigit(line[i]) || line[i] == ' ')
            {
                s1 += line[i];
            }
            else if (line[i] == '–')
            {
                continue;
            }
            else
            {
                s2 = line.Substring(i);
                break;
            }
        }



        var category = new Category()
        {
            CategoryName = s2,
            Level = 2,
        };

        if (s1.IsNullOrEmpty())
        {
            category.HsCode = null;
            return category;
        }


        while (s1[s1.Length - 1] == ' ')
        {
            s1 = s1.Remove(s1.Length - 1, 1);
            if (s1.IsNullOrEmpty())
            {
                category.HsCode = null;
                return category;
            }
        }

        if (s1.IsNullOrEmpty() || string.IsNullOrWhiteSpace(s1))
        {
            category.HsCode = null;
        }
        else
        {
            category.HsCode = s1;
        }

        return category;
    }


    public Category Root3(string line)
    {
        string s1 = "";
        string s2 = "";

        for (int i = 0; i < line.Length; i++)
        {
            if (Char.IsDigit(line[i]) || line[i] == ' ')
            {
                s1 += line[i];
            }
            else if (line[i] == '–')
            {
                continue;
            }
            else
            {
                s2 = line.Substring(i);
                break;
            }
        }



        var category = new Category()
        {
            CategoryName = s2,
            Level = 3,
        };

        if (s1.IsNullOrEmpty())
        {
            category.HsCode = null;
            return category;
        }


        while (s1[s1.Length - 1] == ' ')
        {
            s1 = s1.Remove(s1.Length - 1, 1);
            if (s1.IsNullOrEmpty())
            {
                category.HsCode = null;
                return category;
            }
        }

        if (s1.IsNullOrEmpty() || string.IsNullOrWhiteSpace(s1))
        {
            category.HsCode = null;
        }
        else
        {
            category.HsCode = s1;
        }

        return category;
    }

    public bool IsRoot2(string line)
    {
        int sanoq = 0;
        for (int i = 0; i < line.Length; i++)
        {
            if (Char.IsDigit(line[i]) || line[i] == ' ')
            {
                continue;
            }
            else if (line[i] == '–')
            {
                ++sanoq;
            }
            else
            {
                if (sanoq == 1)
                {
                    return true;
                }
                else break;
            }
        }

        return false;
    }

    public bool IsRoot3(string line)
    {
        int sanoq = 0;
        for (int i = 0; i < line.Length; i++)
        {
            if (Char.IsDigit(line[i]) || line[i] == ' ')
            {
                continue;
            }
            else if (line[i] == '–')
            {
                ++sanoq;
            }
            else
            {
                if (sanoq == 2)
                {
                    return true;
                }
                else break;
            }
        }

        return false;
    }






    public long InsertCategory(CategoryDto categoryDto)
    {
        //FillCategoryTable();
        //return 1u;
        var category = MapToCategoryEntity(categoryDto);
        _context.Categories.Add(category);
        _context.SaveChanges();
        return category.CategoryId;
    }



    //public long InsertProduct(ProductDto productDto)
    //{
    //    var product = new Product()
    //    {
    //        Description = productDto.Description,
    //        HsCode = productDto.HsCode,
    //        SubCategoryId = productDto.SubCategoryId,
    //    };

    //    //_context.Products.Add(product);
    //    _context.SaveChanges();
    //    return product.ProductId;
    //}

    //public long InsertRoot(RootDto rootDto)
    //{
    //    var root = new Root()
    //    {
    //        Description = rootDto.Description,
    //        HsCode = rootDto.HsCode,
    //    };

    //    //_context.Roots.Add(root);
    //    _context.SaveChanges();
    //    return root.RootId;
    //}

    //public long InsertSubCategory(SubCategoryDto subCategoryDto)
    //{
    //    var subCategory = new SubCategory()
    //    {
    //        Description = subCategoryDto.Description,
    //        HsCode = subCategoryDto.HsCode,
    //        SubCategoryId = subCategoryDto.CategoryId,
    //    };

    //    //_context.SubCategories.Add(subCategory);
    //    _context.SaveChanges();
    //    return subCategory.SubCategoryId;
    //}




}
