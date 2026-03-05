using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Tracing;
using Microsoft.AspNetCore.Mvc;

namespace NextOps.Api.Dtos;

public record class CreateProductDto
{
   [Required] string Name;
   [Required] string Brand;
   string Model;
   string Description;

}
