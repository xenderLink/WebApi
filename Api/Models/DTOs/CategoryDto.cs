namespace Store.Models.DTO;

public record CategoryDto : CrtCategoryDto
{
    public short Id { get; init; }    
}

public record CrtCategoryDto
{
    public string Name { get; init; }
}