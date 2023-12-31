using AutoMapper;
using Store.Extensions;
using Store.Services;
using Store.Services.Repositories;
using Store.Models.DTO;
using Store.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Store.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductRepository repository;
    private readonly IMessageProducer _producer;
    private readonly IMapper _mapper;

    public ProductsController(IProductRepository rep, IMessageProducer producer, IMapper mapper)
    {
        repository = rep;
        _producer = producer;
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

            

            if (product is null)
                return NotFound("No such product"); 
            
            var productDto = _mapper.Map<Product, ProductDto>(product);

             _producer.SendingMessage<ProductDto>(productDto);

             return Ok(productDto);

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

            if (!products.Any())
                return  NotFound("No products with this category yet");

            var productDtos = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);

            _producer.SendingMessage<IEnumerable<ProductDto>>(productDtos); 
            
            return Ok(productDtos);
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

            if (!products.Any())
                return NotFound("The database has no products yet");

            var productDtos = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);

            _producer.SendingMessage<IEnumerable<ProductDto>>(productDtos); 

            return Ok(productDtos);
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

            _producer.SendingMessage<Product>(product);

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

            _producer.SendingMessage<Product>(product);

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

            _producer.SendingMessage<Product>(product);

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