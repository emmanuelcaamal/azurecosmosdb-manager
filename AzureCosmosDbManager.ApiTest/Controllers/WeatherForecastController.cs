using AzureCosmosDbManager.ApiTest.Models;
using AzureCosmosDbManager.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureCosmosDbManager.ApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;

        public WeatherForecastController(IRepositoryManager repositoryManager)
        {
            this._repositoryManager = repositoryManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var products = await _repositoryManager.GetRepository<Product>().Find(x => x.Id == id);
            return Ok(products);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _repositoryManager.GetRepository<Product>().Delete(id);
            return Ok();
        }

        [HttpGet("GetPaging")]
        public async Task<IActionResult> GetPaging(string token)
        {
            var products = await _repositoryManager.GetRepository<Product>().Get<List<Product>>("select * from products", 1, token);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            var productCreated = await _repositoryManager.GetRepository<Product>().Save(product);
            return Ok(productCreated);
        }
    }
}
