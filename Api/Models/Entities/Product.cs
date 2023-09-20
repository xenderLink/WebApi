using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Models.Entities;

public sealed class Product
{
    [Column("id")]
    public long Id { get; set; }

    [Column("category_id")]
    public int? catId { get; set; }

    [Column("name", TypeName = "varchar(50)")]
    public string Name { get; set; }

    [Column("price", TypeName = "money")]
    public decimal? Price { get; set; }

    [Column("sku", TypeName = "varchar(100)")]
    public string? Sku { get; set; } = string.Empty;

    [Column("description", TypeName = "jsonb")]
    public string? Description { get; set; } 

    public SubCategory? Category { get; set; }
}