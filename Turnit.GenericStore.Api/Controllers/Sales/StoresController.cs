using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Turnit.GenericStore.Data.Models;
using Turnit.GenericStore.Services.Contacts;

namespace Turnit.GenericStore.Api.Controllers.Sales
{
    public class StoresController : ApiControllerBase
    {
        private readonly IStoresService _storesService;

        public StoresController(IStoresService storesService)
        {
            _storesService = storesService;
        }

        [HttpPost("{storeId}/restock")]
        public async Task RestockProductAsync([FromQuery] Guid storeId, [FromBody] ProductRestockModel productRestockModel, CancellationToken token) => 
            await _storesService.RestockProductAsync(storeId, productRestockModel, token);
    }
}
