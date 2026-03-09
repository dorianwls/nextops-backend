using System.ComponentModel.DataAnnotations;

namespace NextOps.Api.Dtos;

public record class UpdateProductDto
{
   [Required] int Id;
   string? Name;
   string? Model;
   string? Brand;
   int? CategotyId;
   int? ProviderId;
   string? Description;
}
