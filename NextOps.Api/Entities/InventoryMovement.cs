using System;

namespace NextOps.Api.Entities;

public class InventoryMovement
{
    public int Id { get; set; }
    public DateTime Date { get; set; }

    public MovementType Type { get; set; }

    public int? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public string? Description { get; set; }

    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;

    public ICollection<MovementDetails> Details { get; set; } = new List<MovementDetails>();
}