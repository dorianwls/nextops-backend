using System;
using Microsoft.AspNetCore.SignalR;

namespace NextOps.Api.Entities;

public class InventoryMovement
{
   public int Id { get; set; }
   public DateTime Date { get; set; }
   public MovementType Type { get; set; }
   public int? SupplierId { get; set; }
   public Supplier? Supplier { get; set; }
   public string? Description { get; set; }

   public required string UserId { get; set; }
   public required ApplicationUser User { get; set; }
   public ICollection<MovementDetails>? Details { get; set; }
}
