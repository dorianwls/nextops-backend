using System.ComponentModel.DataAnnotations;

namespace NextOps.Api.Dtos.Products;

public record class UpdateProductDto
{
    [Required]
    public int Id { get; init; }

    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Model { get; init; }
    public string? Brand { get; init; }
    public int? CategoryId { get; init; }
}
