using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Tracing;
using Microsoft.AspNetCore.Mvc;

namespace NextOps.Api.Dtos.Products;

public record class CreateProductDto
{
   public required string Name {get; set;}
   public string? Description1 { get; set; }

   string? Model;
   string? Brand;
   int CategotyId;
   int ProviderId;
}
