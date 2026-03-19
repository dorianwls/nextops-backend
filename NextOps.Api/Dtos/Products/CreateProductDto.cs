using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Tracing;
using Microsoft.AspNetCore.Mvc;

namespace NextOps.Api.Dtos.Products;

public record class CreateProductDto
{
   [Required] string Name;
   string Model;
   [Required] string Brand;
   int CategotyId;
   int ProviderId;
   string Description;

}
