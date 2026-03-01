-- ======================================================
-- CONFIGURACIÓN Y CATÁLOGOS
-- ======================================================
CREATE TABLE Configuration (
config_id INT IDENTITY(1,1) PRIMARY KEY,
config_key VARCHAR(100) NOT NULL UNIQUE,
config_value NVARCHAR(MAX) NOT NULL,
description NVARCHAR(500) NULL
);

CREATE TABLE ProductCategory (
product_category_id INT IDENTITY(1,1) PRIMARY KEY,
category_name NVARCHAR(120) NOT NULL UNIQUE
);

CREATE TABLE UnitOfMeasure (
unit_id INT IDENTITY(1,1) PRIMARY KEY,
unit_name NVARCHAR(80) NOT NULL,
abbreviation NVARCHAR(20) NULL,
CONSTRAINT UQ_UnitOfMeasure_Name UNIQUE (unit_name)
);
-- ======================================================
-- SEGURIDAD
-- ======================================================
CREATE TABLE Permission (
permission_id INT IDENTITY(1,1) PRIMARY KEY,
permission_name NVARCHAR(120) NOT NULL UNIQUE,
description NVARCHAR(500) NULL
);

CREATE TABLE Role (
role_id INT IDENTITY(1,1) PRIMARY KEY,
role_name NVARCHAR(80) NOT NULL UNIQUE,
description NVARCHAR(500) NULL
);

CREATE TABLE RolePermission (
role_id INT NOT NULL,
permission_id INT NOT NULL,
CONSTRAINT PK_RolePermission PRIMARY KEY (role_id, permission_id),
CONSTRAINT FK_RolePermission_Role FOREIGN KEY (role_id) REFERENCES Role(role_id) ON DELETE CASCADE,
CONSTRAINT FK_RolePermission_Permission FOREIGN KEY (permission_id) REFERENCES Permission(permission_id) ON DELETE CASCADE
);

CREATE TABLE [User] (
user_id INT IDENTITY(1,1) PRIMARY KEY,
email NVARCHAR(150) NOT NULL UNIQUE,
username NVARCHAR(80) NULL,
password_hash NVARCHAR(255) NOT NULL,
is_active BIT NOT NULL DEFAULT 1,
failed_login_attempts INT NOT NULL DEFAULT 0,
last_failed_login DATETIME2 NULL,
account_locked_until DATETIME2 NULL,
last_login DATETIME2 NULL,
created_at DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);

CREATE TABLE UserRole (
user_id INT NOT NULL,
role_id INT NOT NULL,
CONSTRAINT PK_UserRole PRIMARY KEY (user_id, role_id),
CONSTRAINT FK_UserRole_User FOREIGN KEY (user_id) REFERENCES [User](user_id) ON DELETE CASCADE,
CONSTRAINT FK_UserRole_Role FOREIGN KEY (role_id) REFERENCES Role(role_id) ON DELETE CASCADE
);

-- ======================================================
-- RECURSOS HUMANOS / PERSONAL
-- ======================================================
-- Tabla de Empleado: Datos personales y corporativos
CREATE TABLE Employee (
    employee_id     INT IDENTITY(1,1) PRIMARY KEY,
    user_id         INT           NULL UNIQUE, -- Relación 1 a 1 opcional con User
    first_name      NVARCHAR(100) NOT NULL,
    last_name       NVARCHAR(100) NOT NULL,
    employee_code   NVARCHAR(50)  NULL UNIQUE, 
    job_title       NVARCHAR(100) NULL,
    department      NVARCHAR(100) NULL,        -- Área a la que pertenece
    phone_number    NVARCHAR(40)  NULL,
    
    CONSTRAINT FK_Employee_User FOREIGN KEY (user_id) REFERENCES [User](user_id) ON DELETE SET NULL
);
-- ======================================================
-- PRODUCTOS, PROVEEDORES, LOTES
-- ======================================================
-- ======================================================
-- PRODUCTO → versión más limpia 3NF (campos derivados removidos)
-- ======================================================

CREATE TABLE Product (
    product_id              INT IDENTITY(1,1) PRIMARY KEY,
    sku                     NVARCHAR(60)   NULL UNIQUE,
    barcode                 NVARCHAR(50)   NULL UNIQUE,
    product_name            NVARCHAR(200)  NOT NULL,
    description             NVARCHAR(MAX)  NULL,
    image_url               NVARCHAR(500)  NULL,
    product_category_id     INT            NOT NULL,
    unit_id                 INT            NOT NULL,
    minimum_stock_level     INT            NOT NULL DEFAULT 0,
    is_active               BIT            NOT NULL DEFAULT 1,

    CONSTRAINT FK_Product_Category FOREIGN KEY (product_category_id) REFERENCES ProductCategory(product_category_id),
    CONSTRAINT FK_Product_Unit     FOREIGN KEY (unit_id)             REFERENCES UnitOfMeasure(unit_id)
);

CREATE TABLE Supplier (
supplier_id INT IDENTITY(1,1) PRIMARY KEY,
supplier_name NVARCHAR(200) NOT NULL,
ruc NVARCHAR(20) NULL UNIQUE,
contact_name NVARCHAR(120) NULL,
phone NVARCHAR(40) NULL,
email NVARCHAR(150) NULL,
CONSTRAINT UQ_Supplier_Name UNIQUE (supplier_name)
);

CREATE TABLE Batch (
batch_id INT IDENTITY(1,1) PRIMARY KEY,
product_id INT NOT NULL,
batch_code NVARCHAR(80) NULL,
expiration_date DATE NULL,
received_date DATE NOT NULL DEFAULT CAST(SYSDATETIME() AS DATE),
initial_quantity INT NOT NULL CHECK (initial_quantity >= 0),
current_quantity INT NOT NULL CHECK (current_quantity >= 0),
CONSTRAINT FK_Batch_Product FOREIGN KEY (product_id) REFERENCES Product(product_id),
CONSTRAINT UQ_Batch_Product_BatchCode UNIQUE (product_id, batch_code)
);
-- ======================================================
-- MOVIMIENTOS
-- ======================================================
CREATE TABLE MovementType (
movement_type_id INT IDENTITY(1,1) PRIMARY KEY,
type_code NVARCHAR(20) NOT NULL UNIQUE,
description NVARCHAR(120) NOT NULL
);

CREATE TABLE InventoryMovement (
movement_id INT IDENTITY(1,1) PRIMARY KEY,
movement_date DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
movement_type_id INT NOT NULL,
supplier_id INT NULL,
user_id INT NOT NULL,
reference_no NVARCHAR(60) NULL,
movement_reason NVARCHAR(100) NULL,
notes NVARCHAR(1000) NULL,
CONSTRAINT FK_IM_Type FOREIGN KEY (movement_type_id) REFERENCES MovementType(movement_type_id),
CONSTRAINT FK_IM_Supplier FOREIGN KEY (supplier_id) REFERENCES Supplier(supplier_id),
CONSTRAINT FK_IM_User FOREIGN KEY (user_id) REFERENCES [User](user_id)
);

CREATE TABLE InventoryMovementLine (
movement_id INT NOT NULL,
line_number INT NOT NULL,
product_id INT NOT NULL,
batch_id INT NOT NULL,
quantity INT NOT NULL CHECK (quantity <> 0),
unit_cost DECIMAL(14,4) NOT NULL,
CONSTRAINT PK_InventoryMovementLine PRIMARY KEY (movement_id, line_number),
CONSTRAINT FK_IML_Movement FOREIGN KEY (movement_id) REFERENCES InventoryMovement(movement_id) ON DELETE CASCADE,
CONSTRAINT FK_IML_Product FOREIGN KEY (product_id) REFERENCES Product(product_id),
CONSTRAINT FK_IML_Batch FOREIGN KEY (batch_id) REFERENCES Batch(batch_id)
);

-- ======================================================
-- HISTORIAL DE COSTOS + GASTOS FIJOS
-- ======================================================
CREATE TABLE ProductCostHistory (
cost_history_id INT IDENTITY(1,1) PRIMARY KEY,
product_id INT NOT NULL,
previous_average_cost DECIMAL(14,4) NOT NULL,
new_average_cost DECIMAL(14,4) NOT NULL,
change_reason NVARCHAR(100) NOT NULL,
movement_id INT NULL,
changed_at DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
CONSTRAINT FK_PCH_Product FOREIGN KEY (product_id) REFERENCES Product(product_id),
CONSTRAINT FK_PCH_Movement FOREIGN KEY (movement_id) REFERENCES InventoryMovement(movement_id)
);

CREATE TABLE FixedCostCategory (
fixed_cost_category_id INT IDENTITY(1,1) PRIMARY KEY,
category_name NVARCHAR(120) NOT NULL UNIQUE
);

CREATE TABLE FixedCost (
fixed_cost_id INT IDENTITY(1,1) PRIMARY KEY,
fixed_cost_category_id INT NOT NULL,
amount DECIMAL(14,2) NOT NULL CHECK (amount >= 0),
effective_from DATE NOT NULL,
effective_until DATE NULL,
description NVARCHAR(500) NULL,
monthly_recurring BIT NOT NULL DEFAULT 1,
CONSTRAINT FK_FixedCost_Category FOREIGN KEY (fixed_cost_category_id)
REFERENCES FixedCostCategory(fixed_cost_category_id)
);
-- ======================================================
-- AUDITORÍA + ÍNDICES
-- ======================================================
CREATE TABLE AuditLog (
audit_id INT IDENTITY(1,1) PRIMARY KEY,
user_id INT NOT NULL,
action NVARCHAR(80) NOT NULL,
table_name NVARCHAR(128) NOT NULL,
record_id NVARCHAR(100) NULL,
details NVARCHAR(MAX) NULL,
event_datetime DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
CONSTRAINT FK_AuditLog_User FOREIGN KEY (user_id) REFERENCES [User](user_id)
);

CREATE INDEX IX_AuditLog_User_Date ON AuditLog (user_id, event_datetime);
CREATE INDEX IX_Product_Active_Category ON Product (is_active, product_category_id);
CREATE INDEX IX_IM_Date ON InventoryMovement (movement_date);
CREATE INDEX IX_IML_Product ON InventoryMovementLine (product_id);