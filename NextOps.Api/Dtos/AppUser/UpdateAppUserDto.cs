namespace NextOps.Api.Dtos.AppUser;

public record class UpdateAppUserDto
{
   public string? FirstName { get; set; }
   public string? MiddleName { get; set; }
   public string? LastName { get; set; }
   public string? SecondLastname { get; set; }
   public string? Title { get; set; }
   public string? PhoneNumber {get; set;}

}
