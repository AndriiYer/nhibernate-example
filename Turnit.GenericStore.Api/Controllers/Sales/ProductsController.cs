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
    public async Task AddProductToCategoryAsync(Guid categoryId, Guid productId, CancellationToken token) =>
        await _productsService.AddProductToCategoryAsync(categoryId, productId, token);

    [HttpDelete("{productId}/category/{categoryId}")]
    public async Task RemoveProductFromCategoryAsync(Guid categoryId, Guid productId, CancellationToken token) =>
        await _productsService.RemoveProductFromCategoryAsync(categoryId, productId, token);

    [HttpPost("{productId}/book")]
    public async Task BookProductAsync([FromQuery] Guid productId, [FromBody] ProductBookingModel productBookingModel, CancellationToken token) =>
        await _productsService.BookProductAsync(productId, productBookingModel, token);
}