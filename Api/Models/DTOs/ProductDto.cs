using System.ComponentModel.DataAnnotations;

namespace Store.Models.DTO;

public record ProductDto
{
    [Required] public long Id { get; init; }

    [Required] public string Name { get; init; }
    public decimal? Price { get; init; }
    public string? Description { get; init; }
    public string? Sku { get; set; }
}

public record CrtProductDto
{
    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "Неправильное количество символов")]
    public string Name { get; init; }

    public string? CategoryName { get; set; }

    public decimal? Price { get; init; }

    public string? Sku { get; set; }

    public string? Description { get; init; }
}