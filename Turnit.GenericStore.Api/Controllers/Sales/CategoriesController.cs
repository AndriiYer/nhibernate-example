using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Turnit.GenericStore.Data.Models;
using Turnit.GenericStore.Services.Contacts;

namespace Turnit.GenericStore.Api.Controllers.Sales;

public class CategoriesController : ApiControllerBase
{
    private readonly ICategoriesService _categoriesService;

    public CategoriesController(ICategoriesService categoriesService) 
    {
        _categoriesService = categoriesService;
    }

    [HttpGet]
    public async Task<IEnumerable<CategoryModel>> AllCategories(CancellationToken token = default) => 
        await _categoriesService.GetAllCategoriesAsync(token);
}