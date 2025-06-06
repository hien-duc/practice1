-- SQL Script to delete existing test users and insert one admin user with BCrypt hashed password

-- Delete existing test users
DELETE FROM UserRoles WHERE UserId IN (
    SELECT UserId FROM Users 
    WHERE Email IN ('admin@example.com', 'user1@example.com', 'user2@example.com')
);

DELETE FROM Users 
WHERE Email IN ('admin@example.com', 'user1@example.com', 'user2@example.com');

-- Check if Admin role exists, create if not
IF NOT EXISTS (SELECT * FROM Roles WHERE Name = 'Admin')
BEGIN
    INSERT INTO Roles (Name)
    VALUES ('Admin')
END

-- Get Admin role ID
DECLARE @AdminRoleId INT
SELECT @AdminRoleId = Id FROM Roles WHERE Name = 'Admin'

-- Insert Admin user with BCrypt hashed password
IF NOT EXISTS (SELECT * FROM Users WHERE Email = 'admin@example.com')
BEGIN
    -- Using BCrypt hashed password for 'Admin123!'
    -- This hash was generated with BCrypt.Net.BCrypt.HashPassword("Admin123!", 10)
    -- Lower work factor (10) for faster login processing
    INSERT INTO Users (Email, Fullname, Password, CreatedDate, Status, IsActive, IsLocked, IsDeleted)
    VALUES ('admin@example.com', 'Admin User', '$2a$11$IaD2Vcnv14fS8e3WGaydluyzxT0t4yHMHNpvRmHlFd6Dh99Iy7RBW', GETDATE(), 1, 1, 0, 0);
    
    DECLARE @AdminUserId INT
    SELECT @AdminUserId = UserId FROM Users WHERE Email = 'admin@example.com'
    
    INSERT INTO UserRoles (UserId, RoleId)
    VALUES (@AdminUserId, @AdminRoleId);
END

PRINT 'Admin user created successfully: admin@example.com';
PRINT 'Password: Admin123! (BCrypt hashed in database)';