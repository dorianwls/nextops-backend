using System;

namespace NextOps.Api.Entities;

public class MovementDetails
{
    public int Id { get; set; }
    public required int InventoryMovementId { get; set; }
    public InventoryMovement? Movement { get; set; }

    public required int ProductId { get; set; }
    public Product? Product { get; set; }
    public int? Quantity { get; set; }

    public required decimal UnitCost { get; set; }   

    public decimal? UnitSalePrice { get; set; }
   
}
