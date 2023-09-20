using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Models.Entities;

public sealed class Category
{
    [Column("id")]
    public short Id { get; set; }

    [Column("name", TypeName = "varchar(50)")]
    public string Name { get; set; }

    public ICollection<SubCategory>? Subcats { get; set; }
}