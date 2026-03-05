using System.ComponentModel.DataAnnotations;

namespace NextOps.Api.Dtos;

public record class CreateProductDto
{
   [Required] string Name;
   string Brand;
   string Model;
   string Description;

}
