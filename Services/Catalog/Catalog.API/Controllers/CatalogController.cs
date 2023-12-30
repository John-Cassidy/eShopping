using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers;

public class CatalogController : ApiController
{
    private readonly IMediator _mediator;

    // create constructor injecting IMediator
    public CatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // create GetProductById action
    [HttpGet]
    [Route("[action]/{id}", Name = "GetProductById")]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ProductResponse>> GetProductByIdAsync(string id)
    {
        var query = new GetProductByIdQuery(id);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    // create GetProductsByProductName action
    [HttpGet]
    [Route("[action]/{productName}", Name = "GetProductsByProductName")]
    [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<ProductResponse>>> GetProductsByProductNameAsync(string productName)
    {
        var query = new GetProductsByNameQuery(productName);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    // create GetAllProducts action
    [HttpGet]
    [Route("GetAllProducts")]
    [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<ProductResponse>>> GetAllProductsAsync([FromQuery] CatalogSpecParams catalogSpecParams)
    {
        var query = new GetAllProductsQuery(catalogSpecParams);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    // create GetAllBrands action
    [HttpGet]
    [Route("GetAllBrands")]
    [ProducesResponseType(typeof(IList<BrandResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<BrandResponse>>> GetAllBrandsAsync()
    {
        var query = new GetAllBrandsQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    // create GetAllTypes action
    [HttpGet]
    [Route("GetAllTypes")]
    [ProducesResponseType(typeof(IList<TypesResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<TypesResponse>>> GetAllTypesAsync()
    {
        var query = new GetAllTypesQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    // create GetProductsByBrandName action
    [HttpGet]
    [Route("[action]/{brand}", Name = "GetProductsByBrandName")]
    [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<ProductResponse>>> GetProductsByBrandNameAsync(string brand)
    {
        var query = new GetProductsByBrandQuery(brand);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    // create CreateProduct action
    [HttpPost]
    [Route("CreateProduct")]
    [Authorize(Policy = "CanWrite")]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductResponse>> CreateProductAsync([FromBody] CreateProductCommand productCommand)
    {
        var result = await _mediator.Send(productCommand);

        return Ok(result);
    }

    // create UpdateProduct action
    [HttpPut]
    [Route("UpdateProduct")]
    [Authorize(Policy = "CanWrite")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateProductAsync([FromBody] UpdateProductCommand productCommand)
    {
        var result = await _mediator.Send(productCommand);

        return Ok(result);
    }

    // create DeleteProduct action
    [HttpDelete]
    [Route("DeleteProduct/{id}", Name = "DeleteProduct")]
    [Authorize(Policy = "CanWrite")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteProductAsync(string id)
    {
        var command = new DeleteProductByIdCommand(id);
        var result = await _mediator.Send(command);

        return Ok(result);
    }
}
