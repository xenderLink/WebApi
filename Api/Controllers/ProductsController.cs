using System.Text.Json;
using AutoMapper;
using Store.Extensions;
using Store.Repositories;
using Store.Models.DTO;
using Store.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Store.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductRepository repository;
    private readonly IMapper _mapper;

    public ProductsController(IProductRepository rep, IMapper mapper)
    {
        repository = rep;
        _mapper = mapper;
    }
    
    [HttpGet("Get/{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        try
        {
            var product = await repository.GetByIdAsync(id, cancellationToken);       

            return product is null ? NotFound("No such product") : Ok(_mapper.Map<Product, ProductDto>(product));               
        }
        catch (OperationCanceledException)
        {
            return BadRequest();
        }
        catch (Exception)
        {
            return new JsonResult("Internal server error") { StatusCode = 500 };
        }        
    }

    [HttpGet("Get/{category}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(IEnumerable<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByCategory(string category, CancellationToken cancellationToken)
    {
        try
        {
            var categoryId = await repository.GetCategoryId(category, cancellationToken);
            
            if (categoryId is 0)
                return NotFound("Category doesn't exist");
            
            var products = await repository.GetByCategoryAsync(categoryId, cancellationToken);

            return products.Count() is 0 
                ? NotFound("No products with this category yet") 
                : Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products));
        }
        catch (OperationCanceledException)
        {
            return BadRequest();
        }
        catch (Exception)
        {
            return new JsonResult("Internal server error") { StatusCode = 500 };
        }
    }

    [HttpGet("Get/All")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(IEnumerable<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var products = await repository.GetAllAsync(cancellationToken);

            return products.Count() is 0 
                ? NotFound("The database has no products yet") 
                : Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products));
        }
        catch (OperationCanceledException)
        {
            return BadRequest();
        }
        catch (Exception)
        {
            return new JsonResult("Internal server error") { StatusCode = 500 };
        }        
    }

    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CrtProductDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]    
    public async Task<IActionResult> Create(CrtProductDto productDto, CancellationToken cancellationToken)
    {
        try
        {
            if (!string.IsNullOrEmpty(productDto.Description))
            {
                if (!productDto.Description.IsJson())
                    return BadRequest("Wrong format for JSON in product description");
            }
            
            var product = _mapper.Map<CrtProductDto, Product>(productDto);
            
            if (!string.IsNullOrEmpty(productDto.CategoryName))
            {
                var categoryId = await repository.GetCategoryId(productDto.CategoryName, cancellationToken);
            
                if (categoryId is 0)
                    return NotFound("Category doesn't exist");

                product.catId = categoryId;
            }
            
            await repository.CreateAsync(product, cancellationToken);

            return new JsonResult("Product's been created") { StatusCode = 201 };
        }
        catch (OperationCanceledException)
        {
            return BadRequest();
        }
        catch (Exception)
        {
            return new JsonResult("Internal server error") { StatusCode = 500 };
        }      
    }

    [HttpPut("Update/{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CrtProductDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] 
    public async Task<IActionResult> Update(long id, CrtProductDto productDto, CancellationToken cancellationToken)
    {
        try
        {
            var product = await repository.GetByIdAsync(id, cancellationToken);

            if (product is null)
                return NotFound("No such product");
            
            if (!string.IsNullOrEmpty(productDto.Description))
            {
                if (!productDto.Description.IsJson())
                    return BadRequest("Wrong format for JSON in product description");
            }

            _mapper.Map(productDto, product);

            if (!string.IsNullOrEmpty(productDto.CategoryName))
            {
                var categoryId = await repository.GetCategoryId(productDto.CategoryName, cancellationToken);
            
                if (categoryId is 0)
                    return NotFound("Category doesn't exist");

                product.catId = categoryId;
            }

            await repository.UpdateAsync(product, cancellationToken);

            return Ok("Product's been updated");         
        }
        catch (OperationCanceledException)
        {
            return BadRequest();
        }
        catch (Exception)
        {
            return new JsonResult("Internal server error") { StatusCode = 500 };
        }        
    }

    [HttpDelete("Delete/{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] 
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        try
        {
            var product = await repository.GetByIdAsync(id, cancellationToken);

            if (product is null)
                return NotFound("No such product");

            await repository.DeleteAsync(id, cancellationToken);

            return Ok("Product's been deleted");
        }
        catch (OperationCanceledException)
        {
            return BadRequest();
        }
        catch (Exception)
        {
            return new JsonResult("Internal server error") { StatusCode = 500 };
        } 
    }
}