using System;

namespace NextOps.Api.Entities;

public class Employee
{
   public required int Id {get; set;}
   public required string FirstName {get; set;}
   public string? MiddleName {get; set;}
   public required string LastName {get; set;}
   public string? SecondLastname {get; set;}
   public string? Title {get; set;}
   public string? PhoneNumber {get; set;}
   public required User User {get; set;}
   public required bool Status {get; set;}

}
