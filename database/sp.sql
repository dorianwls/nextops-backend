CREATE OR ALTER PROCEDURE sp_CalcularPrecioRecomendado
    @product_id INT,
    @porcentaje_margen_base DECIMAL(5,2) = 30.00,     -- 30% por defecto
    @factor_ajuste_ultima_compra DECIMAL(5,2) = 1.10,  -- +10% si última compra > 15% del promedio
    @ventas_mensuales_estimadas INT = 500              -- Ajusta según tu negocio
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @average_cost DECIMAL(14,4),
        @ultimo_costo DECIMAL(14,4),
        @gastos_fijos_mes DECIMAL(14,2),
        @precio_base DECIMAL(14,2),
        @precio_recomendado DECIMAL(14,2);

    -- 1. Obtener costo promedio actual
    SELECT @average_cost = average_cost
    FROM Product
    WHERE product_id = @product_id;

    -- 2. Último costo de compra
    SELECT TOP 1 @ultimo_costo = unit_cost
    FROM InventoryMovementLine
    WHERE product_id = @product_id AND quantity > 0
    ORDER BY movement_id DESC;

    -- 3. Gastos fijos mensuales actuales
    SELECT @gastos_fijos_mes = ISNULL(SUM(amount), 0)
    FROM FixedCost
    WHERE monthly_recurring = 1
      AND effective_from <= CAST(GETDATE() AS DATE)
      AND (effective_until IS NULL OR effective_until >= CAST(GETDATE() AS DATE));

    -- 4. Precio base: costo + margen + prorrateo de gastos fijos
    SET @precio_base = 
        @average_cost * (1 + @porcentaje_margen_base / 100) +
        (@gastos_fijos_mes / @ventas_mensuales_estimadas);

    -- 5. Ajuste si la última compra fue significativamente más cara
    IF @ultimo_costo > @average_cost * 1.15
        SET @precio_recomendado = @precio_base * @factor_ajuste_ultima_compra;
    ELSE
        SET @precio_recomendado = @precio_base;

    -- 6. Redondeo común (ej: a 2 decimales o a múltiplos de 5/10 según país)
    SET @precio_recomendado = ROUND(@precio_recomendado, 2);

    -- 7. Actualizar en la tabla (opcional - puedes quitar esta línea si prefieres solo consultar)
    UPDATE Product
    SET suggested_selling_price = @precio_recomendado
    WHERE product_id = @product_id;

    -- Devolver resultado
    SELECT 
        @product_id AS product_id,
        @average_cost AS costo_promedio,
        @ultimo_costo AS ultimo_costo_compra,
        @gastos_fijos_mes AS gastos_fijos_mensuales,
        @precio_recomendado AS precio_venta_recomendado,
        'Se recomienda vender a aprox. C$' + FORMAT(@precio_recomendado, 'N2') AS mensaje;
END;
GO




CREATE PROCEDURE sp_RegisterEmployeeWithUser
    -- Parámetros de Usuario
    @Email NVARCHAR(150),
    @Username NVARCHAR(80),
    @PasswordHash NVARCHAR(255),
    @RoleId INT = NULL, -- Opcional: para asignarle un rol de una vez
    
    -- Parámetros de Empleado
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @EmployeeCode NVARCHAR(50) = NULL,
    @JobTitle NVARCHAR(100) = NULL,
    @Department NVARCHAR(100) = NULL,
    @PhoneNumber NVARCHAR(40) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1. Insertar el Usuario
        DECLARE @NewUserId INT;
        
        INSERT INTO [User] (email, username, password_hash, is_active)
        VALUES (@Email, @Username, @PasswordHash, 1);
        
        SET @NewUserId = SCOPE_IDENTITY(); -- Obtiene el ID generado

        -- 2. Asignar Rol (Si se proporcionó uno)
        IF @RoleId IS NOT NULL
        BEGIN
            INSERT INTO UserRole (user_id, role_id)
            VALUES (@NewUserId, @RoleId);
        END

        -- 3. Insertar el Empleado vinculándolo al Usuario
        INSERT INTO Employee (user_id, first_name, last_name, employee_code, job_title, department, phone_number)
        VALUES (@NewUserId, @FirstName, @LastName, @EmployeeCode, @JobTitle, @Department, @PhoneNumber);

        -- 4. Registrar en Auditoría
        INSERT INTO AuditLog (user_id, action, table_name, record_id, details)
        VALUES (@NewUserId, 'CREATE', 'User/Employee', CAST(@NewUserId AS NVARCHAR), 'Usuario y empleado registrado vía SP');

        -- Si todo sale bien, confirmamos los cambios
        COMMIT TRANSACTION;
        
        SELECT @NewUserId AS new_user_id, 'Usuario y Empleado creados exitosamente' AS message;

    END TRY
    BEGIN CATCH
        -- Si hay cualquier error, deshacemos todos los cambios
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Retornamos el error
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1);
    END CATCH
END;




CREATE PROCEDURE sp_RecordFailedLogin
    @Email NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId INT, @CurrentAttempts INT;
    DECLARE @MaxAttempts INT = 3;
    DECLARE @LockoutMinutes INT = 15;

    -- Buscar al usuario por email
    SELECT @UserId = user_id, @CurrentAttempts = failed_login_attempts 
    FROM [User] 
    WHERE email = @Email AND is_active = 1;

    IF @UserId IS NOT NULL
    BEGIN
        SET @CurrentAttempts = @CurrentAttempts + 1;

        IF @CurrentAttempts >= @MaxAttempts
        BEGIN
            -- Bloquear la cuenta por X minutos
            UPDATE [User]
            SET failed_login_attempts = @CurrentAttempts,
                last_failed_login = SYSDATETIME(),
                account_locked_until = DATEADD(MINUTE, @LockoutMinutes, SYSDATETIME())
            WHERE user_id = @UserId;
            
            -- Registrar auditoría
            INSERT INTO AuditLog (user_id, action, table_name, record_id, details)
            VALUES (@UserId, 'LOCKOUT', 'User', CAST(@UserId AS NVARCHAR), 'Cuenta bloqueada temporalmente por intentos fallidos');
        END
        ELSE
        BEGIN
            -- Solo incrementar contador
            UPDATE [User]
            SET failed_login_attempts = @CurrentAttempts,
                last_failed_login = SYSDATETIME()
            WHERE user_id = @UserId;
        END
    END
END;




CREATE PROCEDURE sp_UpdateProductCost
    @ProductId INT,
    @NewCost DECIMAL(14,4),
    @Reason NVARCHAR(100),
    @MovementId INT = NULL, -- Opcional, si el cambio viene por un movimiento
    @UserId INT -- Quién autorizó el cambio
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Obtener el costo anterior (Asumiendo que sacaríamos el último registrado)
        DECLARE @PreviousCost DECIMAL(14,4) = 0;
        
        SELECT TOP 1 @PreviousCost = new_average_cost 
        FROM ProductCostHistory 
        WHERE product_id = @ProductId 
        ORDER BY changed_at DESC;

        -- Solo registrar si el costo realmente cambió
        IF @PreviousCost <> @NewCost
        BEGIN
            INSERT INTO ProductCostHistory (product_id, previous_average_cost, new_average_cost, change_reason, movement_id, changed_at)
            VALUES (@ProductId, @PreviousCost, @NewCost, @Reason, @MovementId, SYSDATETIME());
            
            -- Auditoría
            INSERT INTO AuditLog (user_id, action, table_name, record_id, details)
            VALUES (@UserId, 'UPDATE COST', 'ProductCostHistory', CAST(SCOPE_IDENTITY() AS NVARCHAR), 'Cambio de costo de ' + CAST(@PreviousCost AS NVARCHAR) + ' a ' + CAST(@NewCost AS NVARCHAR));
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1);
    END CATCH
END;