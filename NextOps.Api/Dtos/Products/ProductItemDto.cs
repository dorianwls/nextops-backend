using System.ComponentModel.DataAnnotations;

namespace NextOps.Api.Dtos.Products;

public record class ProductItemDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Model { get; init; }
    public string? Brand { get; init; }
    public string? Description { get; init; }
    public decimal? AverageCost { get; init; }
    public int? Stock { get; init; }
    public required int CategoryId { get; set; }
    public string? CategoryName { get; init; }
    public bool Status { get; init; }  
};