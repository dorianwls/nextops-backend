namespace NextOps.Api.Dtos.AppUser;

public record class AppUserItemDto
{
   public string Id { get; set; } = default!;
   public string? UserName {get; set;}
   public string? Email { get; set; }
   public string? PhoneNumber { get; set; }
   public string? FirstName { get; set; }
   public string? MiddleName { get; set; }
   public string? LastName { get; set; }
   public string? SecondLastname { get; set; }
   public string? Title { get; set; }
}
