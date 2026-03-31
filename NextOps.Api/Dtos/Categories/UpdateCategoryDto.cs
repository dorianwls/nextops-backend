using System.ComponentModel.DataAnnotations;

namespace NextOps.Api.Dtos.Categories;

public record class UpdateCategoryDto
{
    [Required]
    public int Id { get; init; }

    public string? Name { get; init; }
    public string? Description { get; init; }
    public bool? Status { get; init; }
}