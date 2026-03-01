-- Trigger para actualizar stock y costo promedio al registrar movimiento
CREATE OR ALTER TRIGGER trg_AfterInventoryMovementLine_Insert
ON InventoryMovementLine
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Actualizar stock del producto
    UPDATE p
    SET p.current_stock = p.current_stock + i.quantity
    FROM Product p
    INNER JOIN inserted i ON p.product_id = i.product_id;

    -- Actualizar stock del lote
    UPDATE b
    SET b.current_quantity = b.current_quantity + i.quantity
    FROM Batch b
    INNER JOIN inserted i ON b.batch_id = i.batch_id;

    -- Recalcular costo promedio solo para entradas (quantity > 0)
    DECLARE @product_id INT, @total_cost DECIMAL(18,4), @total_qty INT;

    DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
        SELECT product_id
        FROM inserted
        WHERE quantity > 0;

    OPEN cur;
    FETCH NEXT FROM cur INTO @product_id;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Costo total actual + nueva entrada
        SELECT 
            @total_cost = SUM(b.current_quantity * p.average_cost) + SUM(i.quantity * i.unit_cost),
            @total_qty  = SUM(b.current_quantity) + SUM(i.quantity)
        FROM Product p
        INNER JOIN Batch b ON p.product_id = b.product_id
        INNER JOIN inserted i ON i.product_id = p.product_id AND i.quantity > 0
        WHERE p.product_id = @product_id;

        DECLARE @new_avg DECIMAL(14,4) = @total_cost / @total_qty;

        -- Guardar historial
        INSERT INTO ProductCostHistory (product_id, previous_average_cost, new_average_cost, change_reason, movement_id, changed_at)
        SELECT @product_id, p.average_cost, @new_avg, 'Entrada de inventario', MAX(i.movement_id), SYSDATETIME()
        FROM Product p
        CROSS JOIN inserted i
        WHERE p.product_id = @product_id;

        -- Actualizar producto
        UPDATE Product
        SET average_cost = @new_avg,
            suggested_selling_price = NULL  -- Se recalculará después con sp
        WHERE product_id = @product_id;

        FETCH NEXT FROM cur INTO @product_id;
    END;

    CLOSE cur;
    DEALLOCATE cur;
END;
GO


CREATE TRIGGER trg_UpdateBatchQuantity
ON InventoryMovementLine
AFTER INSERT
AS
BEGIN
    -- SET NOCOUNT ON evita que se devuelvan mensajes de "X filas afectadas", 
    -- lo cual puede confundir a algunos ORMs (como Entity Framework o Prisma)
    SET NOCOUNT ON;

    BEGIN TRY
        -- 1. Actualizar la cantidad del lote
        -- Si i.quantity es positivo (entrada), suma. Si es negativo (salida), resta automáticamente.
        UPDATE b
        SET b.current_quantity = b.current_quantity + i.quantity
        FROM Batch b
        INNER JOIN inserted i ON b.batch_id = i.batch_id;

        -- 2. Validación de Stock Negativo (Doble seguridad)
        -- Revisamos específicamente los lotes que acabamos de afectar
        IF EXISTS (
            SELECT 1 
            FROM Batch b
            INNER JOIN inserted i ON b.batch_id = i.batch_id
            WHERE b.current_quantity < 0
        )
        BEGIN
            -- Si cae por debajo de cero, lanzamos un error claro y deshacemos todo
            RAISERROR ('Error: El movimiento excede el stock disponible en el lote. El inventario no puede ser negativo.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

    END TRY
    BEGIN CATCH
        -- Captura cualquier otro error inesperado y deshace la transacción
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1);
    END CATCH
END;

CREATE TRIGGER trg_PreventMovementDeletion
ON InventoryMovement
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Si alguien intenta hacer un DELETE, disparamos este error y bloqueamos la acción
    RAISERROR ('Violación de Auditoría: No se permite eliminar movimientos de inventario históricas. Si hay un error, debe registrar un movimiento de tipo "Ajuste" para compensarlo.', 16, 1);
    
    -- Nos aseguramos de deshacer cualquier intento
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
END;