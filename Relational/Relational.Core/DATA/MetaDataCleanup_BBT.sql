--BBT Database New UI Metadata 

go
IF OBJECT_ID('dbo.LWStoredProcs2', 'U') IS NOT NULL 
DROP TABLE dbo.LWStoredProcs2;
SELECT * into [dbo].[LWStoredProcs2]
FROM [dbo].[LWStoredProcs]



IF OBJECT_ID('dbo.LW_CustomerSearch2', 'U') IS NOT NULL 
DROP TABLE dbo.LW_CustomerSearch2;
select * into [dbo].[LW_CustomerSearch2]
from [dbo].[LW_CustomerSearch]