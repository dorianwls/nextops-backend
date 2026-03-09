using System;

namespace NextOps.Api.Entities;

public class Supplier
{
   public required int Id {get; set;}
   public required string BussinesName {get; set;}
   public string? ContactName {get; set;}
   public required string PhoneNumber {get; set;}
   public string? Address {get; set;}
   public required bool Status {get; set;}
   
}
