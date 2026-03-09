using System;

namespace NextOps.Api.Entities;

public class User
{
   public int Id { get; set; }
   public required string Username {get; set;}
   public required string PasswordHash {get; set;}
   public required int EmployeeId {get; set;}
   public required Employee Employee {get; set;}
   public required int RolId {get; set;}
   public required Rol Rol {get; set;}
   public required bool Status {get; set;}
}
