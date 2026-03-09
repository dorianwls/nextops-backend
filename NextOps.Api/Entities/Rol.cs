using System;

namespace NextOps.Api.Entities;

public class Rol
{
   public required int Id {get; set;}
   public required string Name {get; set;}
   public string? Description {get; set;}
   public List<User> Users {get; set;}
   public required bool Status {get; set;}
}
