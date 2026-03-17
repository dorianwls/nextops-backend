using System;
using Microsoft.AspNetCore.Identity;

namespace NextOps.Api.Entities;

public class ApplicationUser : IdentityUser
{
   public required string FirstName {get; set;}
   public string? MiddleName {get; set;}
   public required string LastName {get; set;}
   public string? SecondLastname {get; set;}
   public string? Title {get; set;}
   public required bool Status {get; set;}

}
