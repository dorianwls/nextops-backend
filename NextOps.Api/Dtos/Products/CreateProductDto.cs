using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Tracing;
using Microsoft.AspNetCore.Mvc;

namespace NextOps.Api.Dtos.Products;

public record class CreateProductDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Model { get; init; }
    public string? Brand { get; init; }
    public int CategoryId { get; init; }
}
