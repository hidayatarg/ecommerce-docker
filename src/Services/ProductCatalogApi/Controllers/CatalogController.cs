using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ProductCatalogApi.Data;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApi.Domain;

namespace ProductCatalogApi.Controllers
{
    [Route("api/[controller]")]
    public class CatalogController : Controller
    {
        private readonly CatalogContext _catalogContext;
        // over all setting of the Project
        private readonly IOptionsSnapshot<CatalogSettings> _settings;

        public CatalogController(CatalogContext catalogContext, IOptionsSnapshot<CatalogSettings> settings)
        {
            _catalogContext = catalogContext;
            _settings = settings;
            ((DbContext) catalogContext).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        
        // GET api/catalog/catalogtypes
        [HttpGet]
        [Route("[action]")]
        // action will be replaced by method name
        public async Task<IActionResult> CatalogTypes()
        {
            var items = await _catalogContext.CatalogTypes.ToListAsync();
            return Ok(items);
        }
        
        // GET api/catalog/catalogbrands
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogBrands()
        {
            var items = await _catalogContext.CatalogBrands.ToListAsync();
            return Ok(items);
        }

        [HttpGet]
        [Route("items/{id:int}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item = await _catalogContext.CatalogItems.FirstOrDefaultAsync(c => c.Id == id);
            if (item != null)
            {
                // replace the picture URL
                item.PictureUrl = item.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced",
                    _settings.Value.ExternalCatalogBaseUrl);
                return Ok(item);
            }

            return NotFound();
        }
        
        // Posting catalog item 
        // POST api/catalog/item
        [HttpPost]
        [Route(template: "items")]
        public async Task<IActionResult> CreateProduct([FromBody] CatalogItem product)
        {
            var item = new CatalogItem
            {
                    CatalogBrandId = product.CatalogBrandId,
                    CatalogTypeId = product.CatalogTypeId,
                    Description = product.Description,
                    Name = product.Name,
                    PictureFileName = product.PictureFileName,
                    Price = product.Price,
                    
            };
            _catalogContext.CatalogItems.Add(entity: item);
            await _catalogContext.SaveChangesAsync();
            // after it is added to database it should return the added record
            // call to the GetItemById and send the id of item.Id
            return CreatedAtAction(actionName: nameof(GetItemById), value: new { id=item.Id });
        }

        [HttpPut]
        [Route("items")]
        public async Task<IActionResult> UpdatedProduct([FromBody] CatalogItem productToUpdate)
        {
            // search in the list of catalog items
            var catalogItem =
                await _catalogContext.CatalogItems.FirstOrDefaultAsync(i => i.Id == productToUpdate.Id);
            if (catalogItem == null)
            {
                return NotFound(new {Message = $"Item with Id={productToUpdate.Id} not found!!"});
            }
            // if item exists
            catalogItem = productToUpdate;
            _catalogContext.CatalogItems.Update(catalogItem);
            // save the changes to the database
            await _catalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItemById), new { id = productToUpdate.Id });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _catalogContext.CatalogItems.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _catalogContext.CatalogItems.Remove(product);
            await _catalogContext.SaveChangesAsync();
            // give a no response 
            return NoContent();
        }
    }
}