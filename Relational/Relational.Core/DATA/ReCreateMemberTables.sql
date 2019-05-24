IF OBJECT_ID('dbo.m_Vision_Org', 'U') IS NOT NULL 
  DROP TABLE dbo.m_Vision_Org; 

GO
select * into [m_Vision_Org] from [dbo].[Vision_Org]


