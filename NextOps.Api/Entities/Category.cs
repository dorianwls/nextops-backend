using System;

namespace NextOps.Api.Entities;

public class Category
{
   public required int Id {get; set;}
   public required string Name {get; set;}
   public string? Description {get; set;}
   public required bool Status {get; set;}
}
