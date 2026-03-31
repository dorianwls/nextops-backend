using System.ComponentModel.DataAnnotations;

namespace NextOps.Api.Dtos.Categories;

public record class CreateCategoryDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }
}