using System;

namespace NextOps.Api.Entities;

public class Product
{
      public int Id { get; set; }
      public required string Name { get; set; }
      public string? Model { get; set; }
      public string? Brand { get; set; }
      public int CategotyId { get; set; }
      public string? Description { get; set; }
      public required bool Status { get; set; }
}
