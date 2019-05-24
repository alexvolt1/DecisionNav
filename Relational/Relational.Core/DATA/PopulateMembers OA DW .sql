-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- exec [dbo].[PopulateMembers OA DW] 
CREATE PROCEDURE [dbo].[PopulateMembers OA DW] 


AS
BEGIN

IF OBJECT_ID('dbo.m_Relational_ActiveUnitQuery', 'U') IS NOT NULL 
  DROP TABLE dbo.m_Relational_ActiveUnitQuery; 


select * into [m_Relational_ActiveUnitQuery] from [dbo].[Relational_ActiveUnitQuery]

END


GO