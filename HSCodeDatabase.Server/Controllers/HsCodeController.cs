using HSCodeDatabase.DataAccess.Entities;
using HSCodeDatabase.Server.DTOs;
using HSCodeDatabase.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace HSCodeDatabase.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HsCodeController : ControllerBase
{
    private readonly IBaseService _baseService;
    private readonly IHsService _hsService;

    public HsCodeController(IBaseService baseService, IHsService hsService)
    {
        _baseService = baseService;
        _hsService = hsService;
    }



    [HttpGet("GetCategories")]
    public async Task<List<string>> GetCategories(string hsCode)
    {
        var res = await _hsService.GetParentCategoriesAsync(hsCode);
        return res;
    }













    //[HttpPost("AddRoots")]
    //public bool AddRoots(ICollection<RootDto> roots)
    //{
    //    foreach (var root in roots)
    //    {
    //        _baseService.InsertRoot(root);
    //    }

    //    return true;
    //}

    //[HttpPost("AddRoot")]
    //public bool AddRoot(RootDto root)
    //{
    //    _baseService.InsertRoot(root);
    //    return true;
    //}

    //[HttpPost("AddCategories")]
    //public bool AddCategories(ICollection<CategoryDto> categories)
    //{
    //    foreach(var category in categories)
    //    {
    //        _baseService.InsertCategory(category);
    //    }

    //    return true;
    //}

    //[HttpPost("AddCategory")]
    //public bool AddCategory(CategoryDto category)
    //{
    //    _baseService.InsertCategory(category);
    //    return true;
    //}
    //[HttpPost("Start")]
    //public bool Start()
    //{
    //    _baseService.FillCategoryTable();
    //    return true;
    //}

    //[HttpGet("GetParents")]
    //public IActionResult GetParents(string hsCode)
    //{
    //    var res = _baseService.GetAllParentCategories(hsCode);
    //    return Ok(res);
    //}

    //[HttpGet("GetParentsDescs")]
    //public IActionResult GetParentsDescs(string hsCode)
    //{
    //    var res = _baseService.GetAllParentCategoriesDescriptions(hsCode);
    //    return Ok(res);
    //}



    //[HttpPost("AddSubCategories")]
    //public bool AddSubCategories(ICollection<SubCategoryDto> subCategories)
    //{
    //    foreach(var subCategory in subCategories)
    //    {
    //        _baseService.InsertSubCategory(subCategory);
    //    }

    //    return true;
    //}

    //[HttpPost("AddSubCategory")]
    //public bool AddSubCategory(SubCategoryDto subCategory)
    //{
    //    _baseService.InsertSubCategory(subCategory);
    //    return true;
    //}

    //[HttpPost("AddProducts")]
    //public bool AddProducts(ICollection<ProductDto> products)
    //{
    //    foreach(var product in products)
    //    {
    //        _baseService.InsertProduct(product);
    //    }

    //    return true;
    //}

    //[HttpPost("AddProduct")]
    //public bool AddProduct(ProductDto product)
    //{
    //    _baseService.InsertProduct(product);
    //    return true;
    //}
}
