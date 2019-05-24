using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.Xml.Linq;


namespace OpenXmlPowerTools.SpreadsheetWriter02
{
  public class Generator1
  {
      #region Variables

      #endregion


        public byte[] GenerateExcel()
        {
            byte[] file = null;


            Workbook wb = new Workbook
            {
                Worksheets = new Worksheet[]
                {
                    new Worksheet
                    {
                        Name = "MyFirstSheet",
                        TableName = "NamesAndRates",
                       ColumnHeadings = new Cell[]
                        {
                            new Cell
                            {
                                Value = "Name",
                                Bold = true,
                            },
                            new Cell
                            {
                                Value = "Age",
                                Bold = true,
                                HorizontalCellAlignment = HorizontalCellAlignment.Left,
                            },
                            new Cell
                            {
                                Value = "Rate",
                                Bold = true,
                                HorizontalCellAlignment = HorizontalCellAlignment.Left,
                            },
                                                        new Cell
                            {
                                Value = "Age2",
                                Bold = true,
                                HorizontalCellAlignment = HorizontalCellAlignment.Left,
                            },
                            new Cell
                            {
                                Value = "Rate2",
                                Bold = true,
                                HorizontalCellAlignment = HorizontalCellAlignment.Left,
                            }
                        },
                        Rows = new Row[]
                        {
                            new Row
                            {
                                Cells = new Cell[]
                                {
                                    new Cell {
                                        CellDataType = CellDataType.String,
                                        Value = "Boolean",
                                    },
                                    new Cell {
                                        CellDataType = CellDataType.Boolean,
                                        Value = true,
                                    },
                                                                        new Cell {
                                        CellDataType = CellDataType.Boolean,
                                        Value = true,
                                    },
                                                                        new Cell {
                                        CellDataType = CellDataType.Boolean,
                                        Value = true,
                                    },
                                                                        new Cell {
                                        CellDataType = CellDataType.Boolean,
                                        Value = true,
                                    },
                                }
                            },
                            new Row
                            {
                                Cells = new Cell[]
                                {
                                    new Cell {
                                        CellDataType = CellDataType.String,
                                        Value = "Boolean",
                                    },
                                    new Cell {
                                        CellDataType = CellDataType.Boolean,
                                        Value = true,
                                    },
                                                                        new Cell {
                                        CellDataType = CellDataType.Boolean,
                                        Value = true,
                                    },
                                                                        new Cell {
                                        CellDataType = CellDataType.Boolean,
                                        Value = true,
                                    },
                                                                        new Cell {
                                        CellDataType = CellDataType.Boolean,
                                        Value = true,
                                    },
                                }
                            },
                                                        new Row
                            {
                                Cells = new Cell[]
                                {
                                    new Cell {
                                        CellDataType = CellDataType.String,
                                        Value = "Boolean",
                                    },
                                    new Cell {
                                        CellDataType = CellDataType.Boolean,
                                        Value = true,
                                    },
                                                                        new Cell {
                                        CellDataType = CellDataType.Boolean,
                                        Value = true,
                                    },
                                                                        new Cell {
                                        CellDataType = CellDataType.Boolean,
                                        Value = true,
                                    },
                                                                        new Cell {
                                        CellDataType = CellDataType.Boolean,
                                        Value = true,
                                    },
                                }
                            },
                                                        new Row
                            {
                                Cells = new Cell[]
                                {
                                    new Cell {
                                        CellDataType = CellDataType.String,
                                        Value = "Boolean",
                                    },
                                    new Cell {
                                        CellDataType = CellDataType.Boolean,
                                        Value = true,
                                    },
                                                                        new Cell {
                                        CellDataType = CellDataType.Boolean,
                                        Value = true,
                                    },
                                                                        new Cell {
                                        CellDataType = CellDataType.Boolean,
                                        Value = true,
                                    },
                                                                        new Cell {
                                        CellDataType = CellDataType.Boolean,
                                        Value = true,
                                    },
                                }
                            },
                        }
                    }
                }
            };
           SpreadsheetWriter.Write(Path.Combine(@"D:\DEV\Relational\RelationalInternal\Tempfiles", RelationalInternal.UniqueID.RNGCharacterMask() + ".xlsx"), wb);

            return file;
        }
  }
}
