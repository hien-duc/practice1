-- SQL Script to insert carousel data
USE OnlineRetailStore;
GO

-- Clear existing data if needed
DELETE FROM Carousels;
GO

-- Reset identity column
DBCC CHECKIDENT ('Carousels', RESEED, 0);
GO

-- Insert carousel items using the existing images
INSERT INTO Carousels (ImageUrl, Title, Description, [Order], IsActive, CreatedDate)
VALUES
('/images/carousel1.png', 'Welcome to Our Store', 'Discover our amazing products and special offers.', 1, 1, GETDATE()),
('/images/carousel2.png', 'New Arrivals', 'Check out our latest products and collections.', 2, 1, GETDATE()),
('/images/carousel3.png', 'Special Offers', 'Limited time discounts on selected items.', 3, 1, GETDATE());
GO

-- Verify the data was inserted
SELECT * FROM Carousels;
GO