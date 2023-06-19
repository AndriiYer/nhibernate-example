using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Turnit.GenericStore.Data.Models;
using Turnit.GenericStore.Services.Contacts;

namespace Turnit.GenericStore.Api.Controllers.Sales;

public class ProductsController : ApiControllerBase
{
    private readonly IProductsService _productsService;

    public ProductsController(IProductsService productsService) 
    {
        _productsService = productsService;
    }

    [HttpGet("by-category/{categoryId}")]
    public async Task<IEnumerable<ProductModel>> GetProductsByCategoryAsync(Guid categoryId, CancellationToken token = default) => 
        await _productsService.GetProductsByCategoryAsync(categoryId, token);

    [HttpGet]
    public async Task<IEnumerable<ProductCategoryModel>> GetAllProductsAsync(CancellationToken token) => 
        await _productsService.GetAllProductsAsync(token);

    [HttpPut("{productId}/category/{categoryId}")]
    public async Task<IActionResult> AddProductToCategoryAsync(Guid categoryId, Guid productId, CancellationToken token)
    {
        var created = await _productsService.AddProductToCategoryAsync(categoryId, productId, token);

        if(created)
        {
            return Ok($"The product {productId} added to the category {categoryId}");
        }

        return BadRequest($"Error when adding the product {productId} to the category {categoryId}. Presented data was wrong");
    }

    [HttpDelete("{productId}/category/{categoryId}")]
    public async Task<IActionResult> RemoveProductFromCategoryAsync(Guid categoryId, Guid productId, CancellationToken token)
    {
        var created = await _productsService.RemoveProductFromCategoryAsync(categoryId, productId, token);

        if (created)
        {
            return Ok($"The product {productId} removed from the category {categoryId}");
        }

        return BadRequest($"Error when removing the product {productId} to the category {categoryId}. Presented data was wrong");
    }
}