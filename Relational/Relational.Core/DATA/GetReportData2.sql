USE [BBT_PROD_Even]
GO

/****** Object:  StoredProcedure [dbo].[spGetReportData2]    Script Date: 10/27/2017 1:09:50 PM ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[spGetReportData2]
 @Start INT = 0,
 @Count INT = 150,
 @OrderBy varchar(1500)  = ' ',
 @TableName varchar(200),
 @Columns varchar(5000),
 @Where varchar(1000)=' ',
 @GroupBy varchar(1000) = ' '

AS
declare @s varchar(8000);
set @s =
'WITH result_set AS (
  SELECT
    ROW_NUMBER() OVER (' + @OrderBy + ') AS [id],
    ' + @Columns + '
  FROM
    ' + @TableName +'
    ' + @Where + '
	' + @GroupBy + '
) SELECT
  *
FROM
  result_set
WHERE
  [id] BETWEEN ' + Convert(varchar(50),@Start) + ' AND ' + Convert(varchar(50),@Start + @Count-1)


exec(@s);

GO


