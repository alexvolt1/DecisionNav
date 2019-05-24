--  select * into [dbo].[LWStoredProcs_bacup]  from [dbo].[LWStoredProcs]


--3 Delinquent Obligors   The column 'Relationship Number' was specified multiple times for 'result_set'.
update [dbo].[LWStoredProcs2]
 set   [ReportDefaultColumns]='[Obligor Name],[Obligor],[Obligation],[Position 2 of the Process Type],[Performing Status],[Past Due Days],[Past Due Principal Billed],[Past Due Interest Billed],[Past Due Fees Billed],[Loan Product],[Loan Outstanding Balance, End],[Facility Exposure, End],[Maturity Date],[RRR],[LQC/GAAP (Loan)],[Officer],[Assignment Unit],[RA],[Collateral],[Industry],[Past Due Comments],[Relationship Number],[Relationship Name],[IFRS],[AML]'
      ,[ReportAllColumns]=    '[Obligor Name],[Obligor],[Obligation],[Position 2 of the Process Type],[Performing Status],[Past Due Days],[Past Due Principal Billed],[Past Due Interest Billed],[Past Due Fees Billed],[Loan Product],[Loan Outstanding Balance, End],[Facility Exposure, End],[Maturity Date],[RRR],[AML],[IFRS],[RRR Numeric],[LQC/GAAP (Loan)],[LQC/GAAP Numeric],[Officer],[Assignment Unit],[RA],[Relationship Number],[Collateral],[Industry],[Lending Entity],[Territory],[TDR Indicator],[Past Due Comments],[Accrual Date],[Relationship Name]'
where  [QueryID]=3

update [dbo].[LWStoredProcs2]
 set   [ReportDefaultColumns]='[Obligor Name],[Obligor],[Officer],[Assignment Unit],[RA],[Relationship Number],[Facility Exposure, Prior Period End],[Facility Exposure, End],[Change in Facility Exposure],[Unused Balance, End],[LCs, End],[BAs, End],[LCs, Average - YTD],[BAs, Average - YTD],[Loan Outstanding Balance, Average - YTD],[Loan Outstanding Balance, Average - MTD],[Loan Outstanding Balance, End],[RRR],[AML],[IFRS],[Interest Earned - YTD],[Fees Earned - YTD],[Fees Paid - YTD],[Industry],[Lending Entity],[Territory],[Accrual Date],[LQC/GAAP]'
      ,[ReportAllColumns]=    '[Obligor Name],[Obligor],[Officer],[Assignment Unit],[RA],[Relationship Number],[Facility Exposure, Prior Period End],[Facility Exposure, End],[Change in Facility Exposure],[Unused Balance, End],[LCs, End],[BAs, End],[LCs, Average - YTD],[BAs, Average - YTD],[Loan Outstanding Balance, Average - YTD],[Loan Outstanding Balance, Average - MTD],[Loan Outstanding Balance, End],[RRR],[AML],[IFRS],[Interest Earned - YTD],[Fees Earned - YTD],[Fees Paid - YTD],[Industry],[Lending Entity],[Territory],[Accrual Date],[LQC/GAAP]'
where  [QueryID]=4

--6
update [dbo].[LWStoredProcs2]
set   [ReportDefaultColumns]='[Obligor Name],[Obligor],[Officer],[Assignment Unit],[RA],[Relationship Number],[Facility Exposure, End],[LCs, Average - YTD],[BAs, Average - YTD],[Loan Outstanding Balance, Average - YTD],[Loan Outstanding Balance, Average - MTD],[Loan Outstanding Balance, End],[RRR],[Interest Earned - YTD],[Fees Earned - YTD],[Fees Paid - YTD],[Industry],[LQC/GAAP],[IFRS],[AML]'
     ,[ReportAllColumns]=    '[Obligor Name],[Obligor],[Officer],[Assignment Unit],[RA],[Relationship Number],[Loan Outstanding Balance, Average - YTD],[Facility Exposure, End],[Unused Balance, End],[LCs, End],[BAs, End],[LCs, Average - YTD],[BAs, Average - YTD],[Loan Outstanding Balance, Average - MTD],[Loan Outstanding Balance, End],[RRR],[AML],[IFRS],[Interest Earned - YTD],[Fees Earned - YTD],[Fees Paid - YTD],[Industry],[Lending Entity],[Territory],[Accrual Date],[LQC/GAAP]'
where  [QueryID]=6

--7
update [dbo].[LWStoredProcs2]
set   [ReportDefaultColumns]='[Obligor Name],[Obligor],[Officer],[Assignment Unit],[RA],[Relationship Number],[Facility Exposure, End],[LCs, Average - YTD],[BAs, Average - YTD],[Loan Outstanding Balance, Average - YTD],[Loan Outstanding Balance, Average - MTD],[Loan Outstanding Balance, End],[RRR],[Interest Earned - YTD],[Fees Earned - YTD],[Fees Paid - YTD],[Industry],[LQC/GAAP],[IFRS],[AML]'
     ,[ReportAllColumns]=    '[Obligor Name],[Obligor],[Officer],[Assignment Unit],[RA],[Relationship Number],[Facility Exposure, End],[Unused Balance, End],[LCs, End],[BAs, End],[LCs, Average - YTD],[BAs, Average - YTD],[Loan Outstanding Balance, Average - YTD],[Loan Outstanding Balance, Average - MTD],[Loan Outstanding Balance, End],[RRR],[AML],[IFRS],[Interest Earned - YTD],[Fees Earned - YTD],[Fees Paid - YTD],[Industry],[Lending Entity],[Territory],[Accrual Date],[LQC/GAAP]'
where  [QueryID]=7



--8 Industry Groupings
update [dbo].[LWStoredProcs2]
 set   [ReportDefaultColumns]='[Obligor Name],[Obligor],[Officer],[Assignment Unit],[RA],[Relationship Number],[Facility Exposure, End],[LCs, Average - YTD],[BAs, Average - YTD],[Loan Outstanding Balance, Average - YTD],[Loan Outstanding Balance, Average - MTD],[Loan Outstanding Balance, End],[RRR],[Interest Earned - YTD],[Fees Earned - YTD],[Fees Paid - YTD],[Industry],[LQC/GAAP],[IFRS],[AML]'
      ,[ReportAllColumns]=    '[Obligor Name],[Obligor],[Industry],[Officer],[Assignment Unit],[RA],[Relationship Number],[Facility Exposure, End],[Unused Balance, End],[LCs, End],[BAs, End],[LCs, Average - YTD],[BAs, Average - YTD],[Loan Outstanding Balance, Average - YTD],[Loan Outstanding Balance, Average - MTD],[Loan Outstanding Balance, End],[RRR],[AML],[IFRS],[Interest Earned - YTD],[Fees Earned - YTD],[Fees Paid - YTD],[Lending Entity],[Territory],[Accrual Date],[LQC/GAAP]'
where  [QueryID]=8


--12 Risk Rating Upgrades/ Downgrades (Loans)
update [dbo].[LWStoredProcs2]
 set   [ReportAllColumns]=    '[Obligor Name],[Obligor],[Obligation],[Position 2 of the Process Type],[Loan Outstanding Balance, End],[Facility Exposure, End],[Effective Date],[Maturity Date],[Loan Product],[Officer],[Assignment Unit],[RA],[Industry],[Collateral],[Lending Entity],[Territory],[TDR Indicator],[Accrual Date],[IFRS],[AML],[Relationship Number],[Relationship Name]'
where  [QueryID]=12



--11 Risk Rating Upgrades/ Downgrades (Facilities)
update [dbo].[LWStoredProcs2]
 set   [ReportAllColumns]=    '[Obligor Name],[Obligor],[Obligation],[Position 2 of the Process Type],[Position 3 of the Process Type],[Position 6 of the Process Type],[Facility Exposure, End],[Loan Outstanding Balance, End],[Effective Date],[Maturity / Expiration Date],[Facility Product],[Officer],[Assignment Unit],[RA],[Industry],[Collateral],[Lending Entity],[Territory],[TDR Indicator],[Accrual Date],[IFRS],[AML],[Relationship Number],[Relationship Name]'
where  [QueryID]=11




--18   Change in Facility Exposure   Invalid column name 'TermInMonths'.
update [dbo].[LWStoredProcs2]
set   [ReportDefaultColumns]='[Obligor Name],[Obligor],[Obligation],[Loan Product],[TermInMonths],[Officer],[Region],[Current Rate],[RRR],[Facility Exposure, End],[Change in Funds (MTD)],[Change in Funds (MTD)],[Loan Outstanding Balance, End],[Lending Entity],[Territory],[Region],[Office],[RA],[RA ID],[Relationship Number],[Relationship Name],[IFRS],[AML],[LQC/GAAP (Facility)]'
     ,[ReportAllColumns]=    '[Obligor Name],[Obligor],[Obligation],[Facility Product],[TermInMonths],[Officer],[Assignment Unit],[Current Rate],[LQC/GAAP (Facility)],[RRR],[AML],[IFRS],[Facility Exposure, End],[Change in Funds (MTD)],[Change in Funds (YTD)],[Loan Outstanding Balance, End],[Lending Entity],[Territory],[Region],[Office],[RA],[RA ID],[Accrual Date],[Relationship Number],[Relationship Name]'
where  [QueryID]=18



----- Alternate Reports -----
---[Relationship Number],SUM([Facility Exposure, Prior Period End]) AS [Facility Exposure, Prior Period End],SUM([Facility Exposure, End]) AS [Facility Exposure, End],SUM([Change in Facility Exposure]) AS [Change in Facility Exposure],SUM([LCs, Average - YTD]) AS [LCs, Average - YTD],SUM([BAs, Average - YTD]) AS [BAs Average - YTD],SUM([Loan Outstanding Balance, Average - YTD]) AS [Loan Outstanding Balance, Average - YTD],SUM([Loan Outstanding Balance, Average - MTD]) AS [Loan Outstanding Balance, Average - MTD],SUM([Loan Outstanding Balance, End]) AS [Loan Outstanding Balance, End],SUM([Interest Earned - YTD]) AS [Interest Earned - YTD],SUM([Fees Earned - YTD]) AS [Fees Earned - YTD],SUM([Fees Paid - YTD]) AS [Fees Paid - YTD],[Accrual Date]
update [dbo].[LWStoredProcs2]
set [ReportAllColumns]='[Relationship Number],[Facility Exposure, Prior Period End],[Facility Exposure, End],[Change in Facility Exposure],[LCs, Average - YTD],[BAs, Average - YTD],[Loan Outstanding Balance, Average - YTD],[Loan Outstanding Balance, Average - MTD],[Loan Outstanding Balance, End],[Interest Earned - YTD],[Fees Earned - YTD],[Fees Paid - YTD],[Accrual Date]'
where  [QueryID]=104

-----------------------------
--[Relationship Number],SUM([Loan Outstanding Balance, Average - YTD]) AS [Loan Outstanding Balance, Average - YTD],SUM([Loan Outstanding Balance, Average - MTD]) AS [Loan Outstanding Balance, Average - MTD],SUM([Loan Outstanding Balance, End]) AS [Loan Outstanding Balance, End],SUM([Facility Exposure, End]) AS [Facility Exposure, End],SUM([LCs, Average - YTD]) AS [LCs, Average - YTD],SUM([BAs, Average - YTD]) AS [BAs, Average - YTD],SUM([Interest Earned - YTD]) AS [Interest Earned - YTD],SUM([Fees Earned - YTD]) AS [Fees Earned - YTD],SUM([Fees Paid - YTD]) AS [Fees Paid - YTD],[Accrual Date]
update [dbo].[LWStoredProcs2]
set [ReportAllColumns]='[Relationship Number],[Loan Outstanding Balance, Average - YTD],[Loan Outstanding Balance, Average - MTD],[Loan Outstanding Balance, End],[Facility Exposure, End],[LCs, Average - YTD],[BAs, Average - YTD],[Interest Earned - YTD],[Fees Earned - YTD],[Fees Paid - YTD],[Accrual Date]'
where  [QueryID]=106

--[Relationship Number],SUM([Facility Exposure, End]) AS [Facility Exposure, End],SUM([LCs, Average - YTD]) AS [LCs, Average - YTD],SUM([BAs, Average - YTD]) AS [BAs, Average - YTD],SUM([Loan Outstanding Balance, Average - YTD]) AS [Loan Outstanding Balance, Average - YTD],SUM([Loan Outstanding Balance, Average - MTD]) AS [Loan Outstanding Balance, Average - MTD],SUM([Loan Outstanding Balance, End]) AS [Loan Outstanding Balance, End],SUM([Interest Earned - YTD]) AS [Interest Earned - YTD],SUM([Fees Earned - YTD]) AS [Fees Earned - YTD],SUM([Fees Paid - YTD]) AS [Fees Paid - YTD],[Accrual Date]
update [dbo].[LWStoredProcs2]
set [ReportAllColumns]='[Relationship Number],[Facility Exposure, End],[LCs, Average - YTD],[BAs, Average - YTD],[Loan Outstanding Balance, Average - YTD],[Loan Outstanding Balance, Average - MTD],[Loan Outstanding Balance, End],[Interest Earned - YTD],[Fees Earned - YTD],[Fees Paid - YTD],[Accrual Date]'
where  [QueryID]=107

-- View names
update [RAF_Even].[dbo].[LWStoredProcs2] set ReportName='Comparable Deals (Facilities)' where QueryID = 9
update [RAF_Even].[dbo].[LWStoredProcs2] set ReportName='Comparable Deals (Loans)' where QueryID = 10
update [RAF_Even].[dbo].[LWStoredProcs2] set ReportName='Made and Paid (List)' where QueryID = 21
update [RAF_Even].[dbo].[LWStoredProcs2] set ReportName='Change in Funds' where QueryID = 18


--------------
use [RAF_Even]

IF OBJECT_ID('dbo.LW_CustomerSearch2', 'U') IS NOT NULL 
DROP TABLE dbo.LW_CustomerSearch2;
CREATE TABLE [dbo].[LW_CustomerSearch2](
	[SearchID] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [varchar](255) NULL,
	[ColumnNameq] [varchar](255) NULL
) ON [PRIMARY]

-- Here we will build new  keywords search metadata --
insert into [RAF_Even].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('LW_Relational_Obligor','Obligor Name')
insert into [RAF_Even].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('LW_Relational_Obligor','RA')
insert into [RAF_Even].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('LW_Relational_Loan','Obligor Name')
insert into [RAF_Even].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('LW_Relational_Loan','RA')
insert into [RAF_Even].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('LW_Relational_Facility','Obligor Name')
insert into [RAF_Even].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('LW_Relational_Facility','RA')
insert into [RAF_Even].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('LW_Relational_Facility_Guidance','Obligor Name')
insert into [RAF_Even].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('LW_Relational_Facility_Guidance','RA')
insert into [RAF_Even].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('LW_Relational_Delinquency','Obligor Name')
insert into [RAF_Even].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('LW_Relational_Delinquency','RA')
insert into [RAF_Even].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('LW_Relational_CAFE','Obligor Name')
insert into [RAF_Even].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('LW_Relational_CAFE','RA')
insert into [RAF_Even].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('LW_Relational_PartSyndi','Obligor')
insert into [RAF_Even].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('LW_Relational_PartSyndi','RA')
insert into [RAF_Even].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('LW_updown_Facility','Obligor Name')
insert into [RAF_Even].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('LW_updown_Facility','RA')
insert into [RAF_Even].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('LW_updown_Loan','Obligor Name')
insert into [RAF_Even].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('LW_updown_Loan','RA')
go
------------------------------------------------------

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PopulateMembers PAT')
DROP PROCEDURE  [dbo].[PopulateMembers PAT]
go
CREATE PROCEDURE [dbo].[PopulateMembers PAT] 
AS
BEGIN
IF OBJECT_ID('dbo.m_Vision_Org', 'U') IS NOT NULL 
  DROP TABLE dbo.m_Vision_Org; 
select * into [RAF_Even].[dbo].[m_Vision_Org] from [RAF_Even].[dbo].[Vision_Org]
END
GO
exec [dbo].[PopulateMembers PAT]
go
--------------




----------
use [RAF PROD OA DW]
go
IF OBJECT_ID('dbo.LW_CustomerSearch2', 'U') IS NOT NULL 
  DROP TABLE dbo.LW_CustomerSearch2;
CREATE TABLE [dbo].[LW_CustomerSearch2](
	[SearchID] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [varchar](255) NULL,
	[ColumnNameq] [varchar](255) NULL
) ON [PRIMARY]

-- Here we will build new  keywords search metadata --
insert into [RAF PROD OA DW].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('Relational_ActiveUnitQuery','Customer/Borrower Name')
insert into [RAF PROD OA DW].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('Relational_CompletedEvents','Customer/Borrower Name')
insert into [RAF PROD OA DW].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('Relational_InProcessEvents','Customer/Borrower Name')
insert into [RAF PROD OA DW].[dbo].[LW_CustomerSearch2] ([TableName],[ColumnNameq])
values('Relational_LoanOperations','Primary Borrower')
------------------------------------------------------
go
IF OBJECT_ID('dbo.LWStoredProcs2', 'U') IS NOT NULL 
  DROP TABLE dbo.LWStoredProcs2;
 go 
 SELECT * into [RAF PROD OA DW].[dbo].[LWStoredProcs2]  FROM [RAF PROD OA DW].[dbo].[LWStoredProcs] 
 go
update [RAF PROD OA DW].[dbo].[LWStoredProcs2] set ReportName='In Process Events' where QueryID = 103
update [RAF PROD OA DW].[dbo].[LWStoredProcs2] set ReportName='Completed Events' where QueryID = 102

go

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PopulateMembers OA DW')
DROP PROCEDURE  [dbo].[PopulateMembers OA DW]
GO
CREATE PROCEDURE [dbo].[PopulateMembers OA DW] 
AS
BEGIN

IF OBJECT_ID('dbo.m_Relational_ActiveUnitQuery', 'U') IS NOT NULL 
  DROP TABLE dbo.m_Relational_ActiveUnitQuery; 
select * into [m_Relational_ActiveUnitQuery] from [dbo].[Relational_ActiveUnitQuery]
END
go
exec [dbo].[PopulateMembers OA DW] 
-----------














-----Vision Org compiled---
--IF OBJECT_ID('dbo.m_Vision_Org', 'U') IS NOT NULL 
--  DROP TABLE dbo.m_Vision_Org; 

--GO
--select * into [m_Vision_Org] from [dbo].[Vision_Org]

--select * from [m_Vision_Org] 
