using System;
using Microsoft.AspNetCore.Identity;

namespace NextOps.Api.Entities;

public class ApplicationUser : IdentityUser
{
   public string? FirstName {get; set;}
   public string? MiddleName {get; set;}
   public string? LastName {get; set;}
   public string? SecondLastname {get; set;}
   public string? Title {get; set;}

}
