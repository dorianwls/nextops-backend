namespace NextOps.Api.Dtos.Categories;

public record class CategoryItemDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool Status { get; init; }
}