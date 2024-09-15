# FeatureTracker
This Repository contains product feature management system. 

**BackEnd** : ASP.Net Core WEB API (C#), 

**FrontEnd**: Angular, 

**DataBase**: SQL Server.  

**Features**: to add, edit and delete product features, and to track their status and complexity throughout their lifecycle.

**Database Setup**
The FeatureManagementDB database is created by default upon running the project. However, you need to manually add the Features table to the database.

**Features Table**

Connect to your FeatureManagementDB database.

Execute the following SQL query to create the **Features** table:

CREATE TABLE Features (
    Id INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(1000) NOT NULL,
    Description NVARCHAR(MAX),
    EstimatedComplexity NVARCHAR(2),
    Status NVARCHAR(20),
    TargetCompletionDate DATETIME,
    ActualCompletionDate DATETIME
);

**Seed Data:** After creating the table, the application will use the DbContext to seed initial data and function as expected.
