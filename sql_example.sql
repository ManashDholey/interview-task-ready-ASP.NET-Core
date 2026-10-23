SELECT
    CustomerId,
    SUM(Amount) AS TotalAmount
FROM Transactions
WHERE TransactionDate >= DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1)
  AND TransactionDate < DATEADD(MONTH, 1, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1))
GROUP BY CustomerId;

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

SELECT Top 1  Salary  FROM ( SELECT DISTINCT TOP 2 Salary FROM Employees ORDER BY Salary DESC) AS Result ORDER BY Salary  

;WITH Result AS (
SELECT Salary , DENSE_RANK() over (order by Salary) as RankSalary FROM Employees
)
SELECT * FROM Result WHERE RankSalary =3

;WITH Result1 AS (
SELECT Salary , ROW_NUMBER() over (order by Salary) as RankSalary FROM Employees
)
SELECT * FROM Result1 WHERE RankSalary =3

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
    



