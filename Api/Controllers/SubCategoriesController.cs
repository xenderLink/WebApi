using AutoMapper;
using Store.Services.Repositories;
using Store.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Store.Models.Entities;

namespace Store.Controllers;

[ApiController]
[Route("api/[controller]")]

public sealed class SubCategoriesController : ControllerBase
{
    private readonly ISubCategoryRepository _repository;
    private readonly IMapper _mapper;

    public SubCategoriesController(ISubCategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("Get/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubCatDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(SubCatDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _repository.GetByIdAsync(id, cancellationToken);

            return category is null 
                ? NotFound("No such subcategory") 
                : Ok(_mapper.Map<SubCategory, SubCatDto>(category));
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SubCatDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(IEnumerable<SubCatDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByCategory(string category, CancellationToken cancellationToken)
    {
        try
        {
            var categoryId = await _repository.GetParentCategoryId(category, cancellationToken);
            
            if (categoryId is 0)
                return NotFound("Category doesn't exist");

            var categories = await _repository.GetByCategoryAsync(categoryId, cancellationToken);

            return categories.Count() is 0
                ? NotFound("No such subcategories yet")
                : Ok(_mapper.Map<IEnumerable<SubCategory>, IEnumerable<SubCatDto>>(categories));

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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SubCatDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(IEnumerable<SubCatDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var categories = await _repository.GetAllAsync(cancellationToken);

            return categories.Count() is 0 
                ? NotFound("The database has no subcategories yet") 
                : Ok(_mapper.Map<IEnumerable<SubCategory>, IEnumerable<SubCatDto>>(categories));
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
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CrtSubCatDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]  
    public async Task<IActionResult> Create(CrtSubCatDto subCatDto, CancellationToken cancellationToken)
    {
        try
        {
            var category = _mapper.Map<CrtSubCatDto, SubCategory>(subCatDto);
            
            if (!string.IsNullOrEmpty(subCatDto.ParentCategory))
            {
                var categoryId = await _repository.GetParentCategoryId(subCatDto.ParentCategory, cancellationToken);
            
                if (categoryId is 0)
                    return NotFound("Category doesn't exist");

                category.catId = categoryId;
            }
            
            await _repository.CreateAsync(category, cancellationToken);

            return new JsonResult("Subcategory's been created") { StatusCode = 201 };
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

    [HttpPut("Update/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CrtSubCatDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CrtSubCatDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, CrtSubCatDto subCatDto, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _repository.GetByIdAsync(id, cancellationToken);

            if (category is null)
                return NotFound("No such subcategory");

            _mapper.Map(subCatDto, category);

            if (!string.IsNullOrEmpty(subCatDto.ParentCategory))
            {
                var categoryId = await _repository.GetParentCategoryId(subCatDto.ParentCategory, cancellationToken);
            
                if (categoryId is 0)
                    return NotFound("Category doesn't exist");

                category.catId = categoryId;
            }

            await _repository.UpdateAsync(category, cancellationToken);

            return Ok("Subcategory's been updated");
        }
        catch (OperationCanceledException)
        {
            return BadRequest();
        }
        catch (Exception e)
        {
            return new JsonResult($"Internal server error {e}") { StatusCode = 500 };
        }
    }

    [HttpDelete("Delete/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(SubCatDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            if (await _repository.GetByIdAsync(id, cancellationToken) is null)
                return NotFound("No such subcategory");

            await _repository.DeleteAsync(id, cancellationToken);

            return Ok("Subcategory's been deleted");
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