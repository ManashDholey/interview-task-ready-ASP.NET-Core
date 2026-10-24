SELECT
    CustomerId,
    SUM(Amount) AS TotalAmount
FROM Transactions
WHERE TransactionDate >= DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1)
  AND TransactionDate < DATEADD(MONTH, 1, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1))
GROUP BY CustomerId;
GO
-- Replace 3 with your desired 'N' value
WITH RankedSalaries AS (
    SELECT 
        Salary,
        DENSE_RANK() OVER (ORDER BY Salary DESC) AS SalaryRank
    FROM Employee
)
SELECT DISTINCT Salary 
FROM RankedSalaries 
WHERE SalaryRank = 3; 
GO
Select *
FROM Employees3
Where DATEDIFF(MONTH, HireDate, GETDATE()) Between 1 and 6

CREATE NONCLUSTERED INDEX IX_Employees3_HireDate 
ON dbo.Employees3 (HireDate);
GO
SELECT * 
FROM Employees3 
WHERE HireDate >= DATEADD(MONTH, -6, CAST(GETDATE() AS DATE))
 AND HireDate <  DATEADD(MONTH, -1, CAST(GETDATE() AS DATE));
GO
--SELECT * FROM Employees2;
WITH EmployeeCTE AS (

SELECT *, ROW_NUMBER() OVER (PARTITION BY ID ORDER BY ID ) AS RowNumber FROM Employees2

)
-- SELECT * FROM EmployeeCTE;

DELETE FROM EmployeeCTE WHERE RowNumber > 1

GO

DECLARE @ID INT 
SET @ID = 10

;WITH EmployeeCTE AS (
SELECT EmployeeId , EmployeeName , ManagerID 
FROM Employees1 WHERE EmployeeId = @ID

UNION ALL 

SELECT e.EmployeeId , e.EmployeeName , e.ManagerID 
FROM Employees1 As e  
INNER JOIN EmployeeCTE AS ec ON e.EmployeeId = ec.ManagerID
)

SELECT E1.EmployeeName, E2.EmployeeName as ManagerName
FROM EmployeeCTE E1 INNER JOIN EmployeeCTE E2 ON E1.ManagerID = E2.EmployeeId
GO

SELECT Max(Salary) AS Salary FROM Employees 
WHERE Salary <(SELECT Max(Salary) FROM Employees)
SELECT Top 1  Salary  FROM ( SELECT DISTINCT TOP 2 Salary FROM Employees ORDER BY Salary DESC) AS Result ORDER BY Salary
WITH Result AS (
SELECT Salary , DENSE_RANK() over (order by Salary) as RankSalary FROM Employees
)
SELECT * FROM Result WHERE RankSalary =3

GO
SELECT Max(Salary) AS Salary FROM Employees 
WHERE Salary <(SELECT Max(Salary) FROM Employees)
GO
SELECT Top 1  Salary  FROM ( SELECT DISTINCT TOP 2 Salary FROM Employees ORDER BY Salary DESC) AS Result ORDER BY Salary  
GO
;WITH Result AS (
SELECT Salary , DENSE_RANK() over (order by Salary) as RankSalary FROM Employees
)
SELECT * FROM Result WHERE RankSalary =3
GO
;WITH Result1 AS (
SELECT Salary , ROW_NUMBER() over (order by Salary) as RankSalary FROM Employees
)
SELECT * FROM Result1 WHERE RankSalary =3
GO
CREATE OR ALTER PROCEDURE dbo.GetTop10CustomersByPurchase
(
    @StartDate DATE,
    @EndDate   DATE
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        -- Validate input parameters
        IF @StartDate IS NULL
           OR @EndDate IS NULL
           OR @StartDate > @EndDate
        BEGIN
            THROW 50001, 'Invalid date range.', 1;
        END;

        SELECT TOP (10)
            c.CustomerId,
            c.CustomerName,
            SUM(o.TotalAmount) AS TotalPurchaseValue
        FROM dbo.Customers AS c
        INNER JOIN dbo.Orders AS o
            ON o.CustomerId = c.CustomerId
        WHERE o.OrderDate >= @StartDate
          AND o.OrderDate < DATEADD(DAY, 1, @EndDate)
        GROUP BY
            c.CustomerId,
            c.CustomerName
        ORDER BY
            TotalPurchaseValue DESC,
            c.CustomerId ASC
        OPTION (RECOMPILE);

    END TRY
    BEGIN CATCH

        -- Capture and return the error details
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorNumber INT = ERROR_NUMBER();
        DECLARE @ErrorLine INT = ERROR_LINE();
        DECLARE @ErrorProcedure NVARCHAR(128) = ERROR_PROCEDURE();

        -- Optional: log error details to an error-log table here

        -- Re-throw the original error
        THROW;

    END CATCH;
END;
GO
CREATE OR ALTER PROCEDURE dbo.GetProductDetails
(
    @ProductName NVARCHAR(150) = NULL,
    @Category    NVARCHAR(100) = NULL,
    @MinPrice    DECIMAL(18,2) = NULL,
    @MaxPrice    DECIMAL(18,2) = NULL,
    @IsActive    BIT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        -- Validate input
        IF @MinPrice IS NOT NULL
           AND @MaxPrice IS NOT NULL
           AND @MinPrice > @MaxPrice
        BEGIN
            THROW 50001, 'Minimum price cannot exceed maximum price.', 1;
        END;

        SELECT
            p.ProductId,
            p.ProductName,
            p.Category,
            p.Price,
            p.Stock,
            p.IsActive,
            p.CreatedDate
        FROM dbo.Products AS p
        WHERE
            (@ProductName IS NULL
                OR p.ProductName LIKE '%' + @ProductName + '%')
            AND (@Category IS NULL
                OR p.Category = @Category)
            AND (@MinPrice IS NULL
                OR p.Price >= @MinPrice)
            AND (@MaxPrice IS NULL
                OR p.Price <= @MaxPrice)
            AND (@IsActive IS NULL
                OR p.IsActive = @IsActive)
        ORDER BY p.ProductName
        OPTION (RECOMPILE);

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;

-- COUNT(*) targets the entire row structurally, while COUNT(1) evaluates the constant 
-- integer 1 for every row. Because 1 is never NULL, it counts every single row.    
-- Counts only the rows where EmployeeId is not NULL.
  SELECT Count(*) FROM [DB_Interview].[dbo].[EmployeesTest];
  
  SELECT Count(1) FROM [DB_Interview].[dbo].[EmployeesTest];
  SELECT Count(EmployeeId) FROM [DB_Interview].[dbo].[EmployeesTest];

SET STATISTICS TIME ON;
SET STATISTICS IO ON;
-- Your Query Here
SELECT * FROM [DB_Interview].[dbo].[EmployeesTest];
SET STATISTICS IO OFF;
SET STATISTICS TIME OFF;


-- Analyze STATISTICS TIME

-- • Check the Messages tab for parse, compile, and CPU times.
-- • Compare CPU time to Elapsed (total) time.
-- • Spot a parallelism bottleneck if CPU time is significantly higher than elapsed time, meaning multiple threads shared the processing load.
-- • Spot a wait/blocking bottleneck if elapsed time is much higher than CPU time, meaning the query spent time waiting for resources, locks, or disk I/O.

-- Analyze STATISTICS IO

-- • Look at the logical reads (8KB pages read from memory) for each table referenced.
-- • Identify heavy table access by finding which specific object name shows a high scan count and massive logical read numbers.
-- • Check for Worktables/Workfiles usage in tempdb, which signals expensive hashes, spools, or sorts

-- An Index Seek and an Index Scan are two different ways a database query optimizer (like in SQL Server)
-- retrieves data using a B-tree index structure.

-- Index Seek

-- • How it works: The database uses the index B-tree to navigate directly to the specific starting point
-- of the matching data and reads only the qualifying rows and pages.
-- • Cost: Proportional to the number of matching rows returned, not the total size of the table.
-- • Best used for: Selective queries that fetch a small percentage of rows 
--   (typically under 10% to 15% of the table).
-- • Example: WHERE CustomerID = 10248 (jumps straight to customer 10248)

-- Index Scan

-- • How it works: The database reads through all or most of the entries in an index sequentially 
--   from the first page to the last page, regardless of whether every entry matches the filter.
-- • Cost: Proportional to the total number of rows/entries in the index.
-- • Best used for: Non-selective queries, small tables, or situations where a large percentage (e.g., over 50% to 90%) of the table qualifies, or when data must be returned in a specific index order (such as an ORDER BY clause).
-- • Example: WHERE LastName LIKE '%son' (a leading wildcard stops the engine from using a seek, forcing a scan).
--   Example query that may result in an Index Scan
SELECT * 
FROM [DB_Interview].[dbo].[EmployeesTest];
-- Triggers an Index Scan
SELECT EmployeeID, FirstName 
FROM [DB_Interview].[dbo].[EmployeesTest] 
WHERE YEAR(HireDate) = 2026;
-- Triggers an Index Scan
SELECT EmployeeID 
FROM [DB_Interview].[dbo].[EmployeesTest] 
WHERE LastName LIKE '%Smith';
-- Examples of Queries Triggering an Index Seek
CREATE NONCLUSTERED INDEX IX_Employees_DepartmentID 
ON [DB_Interview].[dbo].[EmployeesTest] (DepartmentID);
-- Triggers an Index Seek
SELECT EmployeeID, LastName, FirstName 
FROM [DB_Interview].[dbo].[EmployeesTest] 
WHERE EmployeeID = 105;
-- Triggers an Index Seek
SELECT EmployeeID, DepartmentID 
FROM [DB_Interview].[dbo].[EmployeesTest] 
WHERE DepartmentID = 4;
-- Triggers an Index Seek
-- WHERE HireDate >= '2026-01-01'
-- WHERE LastName LIKE 'Smit%'
-- WHERE EmployeeID = 105
-- A stored procedure takes 20 seconds to execute. Explain your step-by-step approach to optimizing it.
-- 1. Analyze the execution plan to identify bottlenecks (e.g., table scans, index scans, expensive joins).
-- 2. Check for missing indexes and create appropriate indexes to improve query performance.
-- 3. Review the query logic for inefficiencies, such as unnecessary joins or subqueries.
-- 4. Optimize the use of temporary tables and table variables.
-- 5. Consider query hints or restructuring queries for better performance.
-- 6. Monitor wait statistics to identify resource contention (CPU, I/O, locks).
-- 7. Test the optimized procedure and compare execution times to ensure improvements.
-- A SARGable query (short for Search ARGument ABle) uses a condition in a WHERE or JOIN clause that allows the 
-- database engine to use an index efficiently via an Index Seek instead of scanning the entire table.
-- Examples of SARGable queries:
-- WHERE HireDate >= '2026-01-01'
-- WHERE LastName LIKE 'Smit%'
-- WHERE EmployeeID = 105