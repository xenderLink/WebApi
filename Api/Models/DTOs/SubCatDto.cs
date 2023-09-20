using System.ComponentModel.DataAnnotations;

namespace Store.Models.DTO;

public record SubCatDto
{
    public int Id { get; init; }

    public string Name { get; init; }
}

public record CrtSubCatDto
{
    [Required]
    public string Name { get; init; }

    public string? ParentCategory { get; init; }
}