-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- exec [dbo].[PopulateMembers OA DW] 
CREATE PROCEDURE [dbo].[PopulateMembers PAT] 


AS
BEGIN

IF OBJECT_ID('dbo.m_Vision_Org', 'U') IS NOT NULL 
  DROP TABLE dbo.m_Vision_Org; 


select * into [m_Vision_Org] from [dbo].[Vision_Org]

END


GO
