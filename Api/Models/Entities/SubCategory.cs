using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Models.Entities;

public sealed class SubCategory
{
    [Column("id")]
    public int Id { get; set; }

    [Column("category_id")]
    public short? catId { get; set; }

    [Column("name", TypeName = "varchar(50)")]
    public string Name { get; set; }

    public Category? Category { get; set; }
    public ICollection<Product>? Products { get; set; }
}