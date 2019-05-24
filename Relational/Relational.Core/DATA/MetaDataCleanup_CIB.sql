/****** CIB Changes Documented (NOT AN ACTUAL SCRIPT)  ******/
use [CIB CLO DW PROD]
go

IF OBJECT_ID('dbo.LWStoredProcs2', 'U') IS NOT NULL 
DROP TABLE dbo.LWStoredProcs2;
select * into [dbo].[LW_CustomerSearch2]
from [dbo].[LW_CustomerSearch]
go

IF OBJECT_ID('dbo.LWStoredProcs2', 'U') IS NOT NULL 
DROP TABLE dbo.LWStoredProcs2;
SELECT * into [CIB CLO DW PROD].[dbo].[LWStoredProcs2]
FROM [CIB CLO DW PROD].[dbo].[LWStoredProcs]






go
use [CIB_Even]
go
IF OBJECT_ID('dbo.LWStoredProcs2', 'U') IS NOT NULL 
DROP TABLE dbo.LWStoredProcs2;

SELECT * into [CIB_Even].[dbo].[LWStoredProcs2]
FROM [CIB_Even].[dbo].[LWStoredProcs]
go


IF OBJECT_ID('dbo.LWStoredProcs2', 'U') IS NOT NULL 
DROP TABLE dbo.LWStoredProcs2;
select * into [dbo].[LW_CustomerSearch2]
from [dbo].[LW_CustomerSearch]










  update [CIB_Even].[dbo].[LWStoredProcs2]
  set [ReportAllColumns] =
  '[Relationship Number],[Facility Exposure, End],[LCs, Average - YTD],[BAs, Average - YTD],[Loan Outstanding Balance, Average - YTD],[Loan Outstanding Balance, Average - MTD],[Loan Outstanding Balance, End],[Interest Earned - YTD],[Fees Earned - YTD],[Fees Paid - YTD]'
  where QueryID = 104
go
  update [CIB_Even].[dbo].[LWStoredProcs2]
  set [ReportAllColumns] =
  '[Relationship Number],[Loan Outstanding Balance, Average - YTD],[Loan Outstanding Balance, Average - MTD],[Loan Outstanding Balance, End],[Facility Exposure, End],[LCs, Average - YTD],[BAs, Average - YTD],[Interest Earned - YTD],[Fees Earned - YTD],[Fees Paid - YTD]'
  where QueryID = 106
go
  update [CIB_Even].[dbo].[LWStoredProcs2]
  set [ReportAllColumns] =
  '[Relationship Number],[Facility Exposure, End],[LCs, Average - YTD],[BAs, Average - YTD],[Loan Outstanding Balance, Average - YTD],[Loan Outstanding Balance, Average - MTD],[Loan Outstanding Balance, End],[Interest Earned - YTD],[Fees Earned - YTD],[Fees Paid - YTD]'
  where QueryID = 107


go

update [CIB_Even].[dbo].[LWStoredProcs2]
set [ReportAllColumns] =  
'[Obligor Name],[Obligor],[Obligation],[Obligation Record Key],[Position 2 of the Process Type],[Position 3 of the Process Type],[Position 6 of the Process Type],[Facility Exposure, End],[Loan Outstanding Balance, End],[Effective Date],[Maturity / Expiration Date],[Facility Product],[Officer],[Industry],[Collateral]'
where QueryID = 11

  update [CIB_Even].[dbo].[LWStoredProcs2]
  set ReportAllColumns =
  '[Obligor Name],[Obligor],[Obligation],[Obligation Record Key],[Position 2 of the Process Type],[Loan Outstanding Balance, End],[Facility Exposure, End],[Previous LGD/FR (Loan)],[Effective Date],[Officer],[Loan Product],[Maturity Date],[Industry],[Collateral]'
  where QueryID = 12




