-- ========================================
-- Logging System Database Verification
-- ========================================

USE [Shipping]
GO

-- 1. Check if ApplicationLogs table exists
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ApplicationLogs')
BEGIN
    PRINT '✅ ApplicationLogs table exists'
    
    -- Show table structure
    SELECT 
        COLUMN_NAME,
        DATA_TYPE,
        CHARACTER_MAXIMUM_LENGTH,
        IS_NULLABLE
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'ApplicationLogs'
    ORDER BY ORDINAL_POSITION
END
ELSE
BEGIN
    PRINT '❌ ApplicationLogs table does NOT exist'
    PRINT 'ℹ️  Start the Ui or WebApi application to auto-create the table'
END
GO

-- 2. Check record count (if table exists)
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ApplicationLogs')
BEGIN
    DECLARE @RecordCount INT
    SELECT @RecordCount = COUNT(*) FROM ApplicationLogs
    
    PRINT ''
    PRINT 'Total Logs: ' + CAST(@RecordCount AS VARCHAR)
    
    IF @RecordCount > 0
    BEGIN
        -- Show log level distribution
        PRINT ''
        PRINT '📊 Log Level Distribution:'
        SELECT 
            Level,
            COUNT(*) as Count,
            CAST(COUNT(*) * 100.0 / @RecordCount AS DECIMAL(5,2)) as Percentage
        FROM ApplicationLogs
        GROUP BY Level
        ORDER BY Count DESC
        
        -- Show recent logs
        PRINT ''
        PRINT '📝 Last 10 Logs:'
        SELECT TOP 10
            TimeStamp,
            Level,
            LEFT(Message, 100) as MessagePreview,
            UserName,
            RequestPath,
            CorrelationId
        FROM ApplicationLogs
        ORDER BY TimeStamp DESC
        
        -- Show user activity
        PRINT ''
        PRINT '👥 User Activity:'
        SELECT 
            ISNULL(UserName, 'Anonymous') as UserName,
            COUNT(*) as LogCount,
            MIN(TimeStamp) as FirstActivity,
            MAX(TimeStamp) as LastActivity
        FROM ApplicationLogs
        GROUP BY UserName
        ORDER BY LogCount DESC
        
        -- Show error logs
        PRINT ''
        PRINT '❌ Error/Warning Logs:'
        SELECT 
            TimeStamp,
            Level,
            LEFT(Message, 100) as MessagePreview,
            UserName,
            RequestPath
        FROM ApplicationLogs
        WHERE Level IN ('Error', 'Warning')
        ORDER BY TimeStamp DESC
    END
    ELSE
    BEGIN
        PRINT 'ℹ️  No logs recorded yet. Use the test page: https://localhost:7279/LogTest'
    END
END
GO

-- 3. Create recommended indexes for performance
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ApplicationLogs')
BEGIN
    -- Check if indexes exist
    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ApplicationLogs_TimeStamp' AND object_id = OBJECT_ID('ApplicationLogs'))
    BEGIN
        PRINT ''
        PRINT '📈 Creating performance indexes...'
        
        CREATE INDEX IX_ApplicationLogs_TimeStamp 
        ON ApplicationLogs(TimeStamp DESC)
        PRINT '✅ Created: IX_ApplicationLogs_TimeStamp'
        
        CREATE INDEX IX_ApplicationLogs_Level 
        ON ApplicationLogs(Level) INCLUDE (TimeStamp)
        PRINT '✅ Created: IX_ApplicationLogs_Level'
        
        CREATE INDEX IX_ApplicationLogs_UserName 
        ON ApplicationLogs(UserName) INCLUDE (TimeStamp)
        PRINT '✅ Created: IX_ApplicationLogs_UserName'
        
        CREATE INDEX IX_ApplicationLogs_CorrelationId 
        ON ApplicationLogs(CorrelationId) INCLUDE (TimeStamp)
        PRINT '✅ Created: IX_ApplicationLogs_CorrelationId'
    END
    ELSE
    BEGIN
        PRINT ''
        PRINT '✅ Performance indexes already exist'
    END
END
GO

-- 4. Create cleanup stored procedure
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_CleanupOldLogs')
BEGIN
    PRINT ''
    PRINT '🧹 Creating cleanup stored procedure...'
    
    EXEC('
    CREATE PROCEDURE sp_CleanupOldLogs
        @DaysToKeep INT = 30
    AS
    BEGIN
        SET NOCOUNT ON;
        
        DECLARE @DeleteCount INT
        DECLARE @CutoffDate DATETIME2 = DATEADD(DAY, -@DaysToKeep, GETDATE())
        
        DELETE FROM ApplicationLogs 
        WHERE TimeStamp < @CutoffDate
        
        SET @DeleteCount = @@ROWCOUNT
        
        SELECT 
            @DeleteCount as RecordsDeleted,
            @CutoffDate as CutoffDate,
            @DaysToKeep as DaysKept
    END
    ')
    
    PRINT '✅ Created: sp_CleanupOldLogs'
    PRINT 'ℹ️  Usage: EXEC sp_CleanupOldLogs @DaysToKeep = 30'
END
ELSE
BEGIN
    PRINT ''
    PRINT '✅ Cleanup stored procedure already exists'
END
GO

-- 5. Show database size impact
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ApplicationLogs')
BEGIN
    PRINT ''
    PRINT '💾 Storage Information:'
    
    SELECT 
        t.NAME AS TableName,
        p.rows AS RowCounts,
        SUM(a.total_pages) * 8 AS TotalSpaceKB, 
        SUM(a.used_pages) * 8 AS UsedSpaceKB, 
        (SUM(a.total_pages) - SUM(a.used_pages)) * 8 AS UnusedSpaceKB
    FROM 
        sys.tables t
    INNER JOIN      
        sys.indexes i ON t.OBJECT_ID = i.object_id
    INNER JOIN 
        sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
    INNER JOIN 
        sys.allocation_units a ON p.partition_id = a.container_id
    WHERE 
        t.NAME = 'ApplicationLogs'
    GROUP BY 
        t.Name, p.Rows
    ORDER BY 
        SUM(a.total_pages) DESC
END
GO

PRINT ''
PRINT '========================================='
PRINT '✅ Verification Complete!'
PRINT '========================================='
PRINT ''
PRINT 'Next Steps:'
PRINT '1. Start Ui project: cd Ui && dotnet run'
PRINT '2. Navigate to: https://localhost:7279/LogTest'
PRINT '3. Click test buttons to generate logs'
PRINT '4. Re-run this script to see results'
PRINT ''
