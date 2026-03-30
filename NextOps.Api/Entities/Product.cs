using System;

namespace NextOps.Api.Entities;

public class Product
{
      public int Id { get; set; }
      public required string Name { get; set; } 
      public string? Model { get; set; }
      public string? Brand { get; set; }
      public string? Description { get; set; }
      public decimal? AverageCost { get; set; }
      public int? Stock { get; set; }
      public required int CategoryId { get; set; }
      public required Category Category { get; set; }
      public bool Status { get; set; }
}
