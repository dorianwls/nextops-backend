CREATE OR ALTER VIEW vw_Product_Stock_And_Cost AS
SELECT 
    p.product_id,
    p.sku,
    p.barcode,
    p.product_name,
    p.description,
    p.image_url,
    p.product_category_id,
    p.unit_id,
    p.minimum_stock_level,
    p.is_active,

    -- Stock actual (calculado)
    ISNULL((
        SELECT SUM(quantity)
        FROM InventoryMovementLine iml
        INNER JOIN InventoryMovement im ON iml.movement_id = im.movement_id
        WHERE iml.product_id = p.product_id
    ), 0) AS current_stock,

    -- Costo promedio actual (último valor del historial o 0)
    ISNULL((
        SELECT TOP 1 new_average_cost
        FROM ProductCostHistory pch
        WHERE pch.product_id = p.product_id
        ORDER BY pch.changed_at DESC
    ), 0) AS average_cost,

    -- Precio sugerido (puedes mover la lógica del sp aquí o llamarlo)
    NULL AS suggested_selling_price   -- o calcularlo inline si es simple

FROM Product p;




CREATE VIEW vw_ProductStock AS
SELECT 
    p.product_id,
    p.sku,
    p.barcode,
    p.product_name,
    pc.category_name,
    u.abbreviation AS unit,
    p.minimum_stock_level,
    -- Calculamos el stock total sumando los lotes
    ISNULL(SUM(b.current_quantity), 0) AS total_stock,
    -- Alerta visual para reabastecimiento
    CASE 
        WHEN ISNULL(SUM(b.current_quantity), 0) <= p.minimum_stock_level THEN 'Bajo/Reabastecer'
        ELSE 'Óptimo'
    END AS stock_status
FROM Product p
LEFT JOIN ProductCategory pc ON p.product_category_id = pc.product_category_id
LEFT JOIN UnitOfMeasure u ON p.unit_id = u.unit_id
LEFT JOIN Batch b ON p.product_id = b.product_id
GROUP BY 
    p.product_id, p.sku, p.barcode, p.product_name, 
    pc.category_name, u.abbreviation, p.minimum_stock_level;




CREATE VIEW vw_UserProfile AS
SELECT 
    e.employee_id,
    e.first_name,
    e.last_name,
    (e.first_name + ' ' + e.last_name) AS full_name,
    e.department,
    e.job_title,
    u.user_id,
    u.email,
    u.username,
    u.is_active AS login_enabled
FROM Employee e
LEFT JOIN [User] u ON e.user_id = u.user_id;

CREATE VIEW vw_MovementHistory AS
SELECT 
    im.movement_id,
    im.movement_date,
    mt.type_code,
    mt.description AS movement_type,
    im.reference_no,
    (e.first_name + ' ' + e.last_name) AS recorded_by,
    s.supplier_name,
    iml.line_number,
    p.product_name,
    b.batch_code,
    iml.quantity,
    iml.unit_cost,
    -- Calculamos el total de esa línea
    (iml.quantity * iml.unit_cost) AS total_line_cost
FROM InventoryMovement im
INNER JOIN MovementType mt ON im.movement_type_id = mt.movement_type_id
INNER JOIN [User] u ON im.user_id = u.user_id
INNER JOIN Employee e ON u.user_id = e.user_id
LEFT JOIN Supplier s ON im.supplier_id = s.supplier_id
INNER JOIN InventoryMovementLine iml ON im.movement_id = iml.movement_id
INNER JOIN Product p ON iml.product_id = p.product_id
INNER JOIN Batch b ON iml.batch_id = b.batch_id;