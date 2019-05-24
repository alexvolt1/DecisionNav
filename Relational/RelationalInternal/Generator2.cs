using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Web;
using System.Diagnostics;
using DSS.Logs;

namespace OpenXmlPowerTools.SpreadsheetWriter
{
    public class Generator2
    {
        #region Variables
        RelationalInternal.DWCredentials _dwc;
        List<Column> _listCols;
        string _path;
        Type thisType = null;
        #endregion

        public Generator2()
        {
            Log.AppendToFile = true;
            Log.Console = true;
            Log.LogPath = @"D:\Logs";
            Log.FileName = "Relational Excel Generator 2";
            thisType = typeof(Generator2);
        }
        public Generator2(RelationalInternal.DWCredentials dwc)
        {

            Log.AppendToFile = true;
            Log.Console = true;
            Log.LogPath = @"D:\Logs";
            Log.FileName = "Relational Excel Generator 2";
            thisType = typeof(Generator2);

            _dwc = dwc;
            _path = string.Empty;
            try
            {
                _path = System.Web.Hosting.HostingEnvironment.MapPath("~/Tempfiles");
            }
            catch 
            {
                Log.Write("Relational Service doesn't have enough permissions to access a file system", thisType);
                throw new Exception("Relational Service doesn't have enough permissions to access a file system");
            }

            _listCols = new List<Column>();
            Directory.CreateDirectory(_path);
        }
        public byte[] GenerateExcel()
        {
            return GenerateExcel(Path.Combine(_path, RelationalInternal.UniqueID.RNGCharacterMask() + ".xlsx"));
        }
        public byte[] GenerateExcel(string name)
        {

            Stopwatch sw = new Stopwatch();
            // sw.Start();
            Log.Write("Excel generator started for User:[" + _dwc.UserName + "] Report ID:[" + _dwc.ReportId + "] Topic:[" + _dwc.TopicID + "]");
            // sw.Stop();
            //Cleanup();

            byte[] file = null;
            string fileName = name;
            if (name == null || name == "")
                throw new Exception("No report name provided ");

            var listColHeads = new List<Cell>();
            var rows = new List<Row>();

            sw.Start();
            Log.Write("Getting Data for User:[" + _dwc.UserName + "] Report ID:[" + _dwc.ReportId + "] Topic:[" + _dwc.TopicID + "]");


            using (SqlConnection conn = RelationalInternal.SecureConnection.GetSecureConnection(_dwc.DATAConnectionString, _dwc.GroupName, _dwc.UserName, _dwc.Password))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(_dwc.sql, conn))
                {
                    cmd.CommandTimeout = 240;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        DataTable dtSchema = dr.GetSchemaTable();
                        if (dtSchema != null)
                        {

                            for (int i = 0; i < dtSchema.Rows.Count; i++)
                            {
                                string columnName = Convert.ToString(dtSchema.Rows[i]["ColumnName"]);
                                var value = Convert.ToString(dtSchema.Rows[i]["ColumnName"]);
                                var column = new Column
                                {
                                    Min = (UInt32)i,
                                    Max = (UInt32)i,
                                    Length = value.Length,
                                    Style = 0,
                                    CustomWidth = true,
                                };
                                _listCols.Add(column);
                                var columnheader = new Cell
                                {
                                    Value = value,
                                    Bold = true,
                                    CellDataType = CellDataType.String,
                                    HorizontalCellAlignment = HorizontalCellAlignment.Center,
                                };
                                listColHeads.Add(columnheader);
                            }
                        }
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var newRow = new Row();
                                var cells = new List<Cell>();
                                for (int i = 0; i < listColHeads.Count; i++)
                                {

                                    string d = Convert.ToString(dtSchema.Rows[i]["DataType"]);



                                    switch (d)
                                    {
                                        case "System.Int32":
                                            var int32cellv = (Convert.IsDBNull(dr[i])) ? 0 : dr.GetInt32(i);
                                            var int32cell = new Cell
                                            {
                                                CellDataType = CellDataType.Number,
                                                Value = int32cellv,
                                                //FormatCode = "0",
                                                FormatCode = "0;[Red] -0",
                                                HorizontalCellAlignment = HorizontalCellAlignment.Right,
                                            };
                                            SetColumnWidth(i, int32cellv.ToString().Length);
                                            cells.Add(int32cell);
                                            break;

                                        case "System.Int16":
                                            var int16cellv = (Convert.IsDBNull(dr[i])) ? 0 : dr.GetInt16(i);
                                            var int16cell = new Cell
                                            {
                                                CellDataType = CellDataType.Number,
                                                Value = int16cellv,
                                                FormatCode = "0",
                                                //FormatCode = @"#,##0.00_);[Red]\(#,##0.00\)",
                                                HorizontalCellAlignment = HorizontalCellAlignment.Left,
                                            };
                                            SetColumnWidth(i, int16cellv.ToString().Length);
                                            cells.Add(int16cell);
                                            break;


                                        case "System.Decimal":
                                            var numbercellv = (Convert.IsDBNull(dr[i])) ? 0 : dr.GetDecimal(i);

                                            //hack to provide custom formatting for colName "Amount"
                                            //string formatCode = listColHeads[i].Value.ToString().ToLower()=="amount" ? @" #,##0.00_);[Red]\( #,##0.00\)": @"$ #,##0.00_);[Red]\($ #,##0.00\)";
                                            string formatCode = listColHeads[i].Value.ToString().ToLower() == "amount" ? @" #,##0.00_);[Red] -#,##0.00" : @"$ #,##0.00_);[Red] -$ #,##0.00";

                                            var numbercell = new Cell
                                            {
                                                CellDataType = CellDataType.Number,
                                                Value = numbercellv,
                                                // FormatCode = "0.00",
                                                //FormatCode = @"#,##0.00_);[Red]\(#,##0.00\)",

                                                FormatCode = formatCode,
                                                // FormatCode = StringValue.FromString("\"$\"#,##0_);[Red]\\(\"$\"#,##0\\)")

                                                HorizontalCellAlignment = HorizontalCellAlignment.Right,
                                            };
                                            SetColumnWidth(i, numbercellv.ToString().Length + 4);
                                            cells.Add(numbercell);
                                            break;

                                        case "System.Double":
                                            var doublecellv = (Convert.IsDBNull(dr[i])) ? 0 : dr.GetDouble(i);
                                            var doublecell = new Cell
                                            {
                                                CellDataType = CellDataType.Number,
                                                Value = doublecellv,
                                                // FormatCode = "0.00",
                                                FormatCode = @"#,##0.00_);[Red]\(#,##0.00\)",

                                                HorizontalCellAlignment = HorizontalCellAlignment.Right,
                                            };
                                            SetColumnWidth(i, doublecellv.ToString().Length);
                                            cells.Add(doublecell);
                                            break;

                                        case "System.Single":
                                            var singlecellv = (Convert.IsDBNull(dr[i])) ? 0 : dr.GetFloat(i);
                                            var singlecell = new Cell
                                            {
                                                CellDataType = CellDataType.Number,
                                                Value = singlecellv,
                                                // FormatCode = "0.00",
                                                //FormatCode = "0.00%",
                                                //FormatCode = "0.000%",

                                                FormatCode = "0.000%;[Red] -0.000%",

                                                HorizontalCellAlignment = HorizontalCellAlignment.Right,
                                            };
                                            SetColumnWidth(i, singlecellv.ToString().Length);
                                            cells.Add(singlecell);
                                            break;

                                        case "System.DateTime":


                                            if (Convert.IsDBNull(dr[i]))
                                            {
                                                var datecell = new Cell
                                                {
                                                    CellDataType = CellDataType.Date,
                                                    FormatCode = "mm-dd-yy",
                                                    HorizontalCellAlignment = HorizontalCellAlignment.Right,
                                                };
                                                cells.Add(datecell);
                                            }
                                            else
                                            {
                                                var datecellv = dr.GetDateTime(i);
                                                var datecell = new Cell
                                                {
                                                    CellDataType = CellDataType.Date,
                                                    Value = datecellv,
                                                    FormatCode = "mm-dd-yy",
                                                    HorizontalCellAlignment = HorizontalCellAlignment.Right,
                                                };
                                                SetColumnWidth(i, datecellv.ToString().Length);
                                                cells.Add(datecell);
                                            }

                                            //var datecellv = (Convert.IsDBNull(dr[i])) ? DateTime.MaxValue : dr.GetDateTime(i);
                                            //var datecell = new Cell
                                            //  {
                                            //      CellDataType = CellDataType.Date,
                                            //     // Value = datecellv,
                                            //      FormatCode = "mm-dd-yy",
                                            //      HorizontalCellAlignment = HorizontalCellAlignment.Left,
                                            //  };
                                            //SetColumnWidth(i, datecellv.ToString().Length);
                                            //cells.Add(datecell);


                                            break;

                                        default:
                                            var defaultcellv = dr[i].ToString();

                                            var defaultcell = new Cell
                                            {
                                                CellDataType = CellDataType.String,
                                                FormatCode = "0",
                                                Value = defaultcellv,
                                                HorizontalCellAlignment = HorizontalCellAlignment.Left,
                                            };
                                            SetColumnWidth(i, defaultcellv.Length);
                                            cells.Add(defaultcell);

                                            break;
                                    }
                                }
                                newRow.Cells = cells;
                                rows.Add(newRow);
                            }
                        }

                    }//reader
                }// sqlcommand
            }// connection


            sw.Stop();
            Log.Write("Got Data for User:[" + _dwc.UserName + "] Report ID:[" + _dwc.ReportId + "] Topic:[" + _dwc.TopicID + "]. Elapsed Time [" + sw.Elapsed.Hours + ":" + sw.Elapsed.Minutes + ":" + sw.Elapsed.Seconds + "]");


            sw.Restart();
            Log.Write("Building Excel file for User:[" + _dwc.UserName + "] Report ID:[" + _dwc.ReportId + "] Topic:[" + _dwc.TopicID + "]");

            Workbook wb = new Workbook
            {
                Worksheets = new Worksheet[]
                            {
                               new Worksheet
                               {
                                  Name = "Report",
                                  //TableName = "Report1",
                                  Columns=_listCols,
                                  ColumnHeadings = listColHeads,
                                  Rows = rows,
                               }
                            }
            };


            SpreadsheetWriter.Write(fileName, wb);
            listColHeads = null;
            _listCols = null;
            rows = null;
            wb = null;
            /////////


            sw.Stop();
            Log.Write("Excel file generated for User:[" + _dwc.UserName + "] Report ID:[" + _dwc.ReportId + "] Topic:[" + _dwc.TopicID + "]. Elapsed Time [" + sw.Elapsed.Hours + ":" + sw.Elapsed.Minutes + ":" + sw.Elapsed.Seconds + "]");



            sw.Restart();
            Log.Write("Writing Excel file to the temp directory as [" + name + "] for User:[" + _dwc.UserName + "] Report ID:[" + _dwc.ReportId + "] Topic:[" + _dwc.TopicID + "]");


            using (FileStream fs = File.Open(name, FileMode.Open))
            {
                file = new byte[fs.Length];
                fs.Read(file, 0, Convert.ToInt32(fs.Length));
                fs.Close();
            }
            sw.Stop();
            Log.Write("Saved Excel file to the temp directory as [" + name + "] for User:[" + _dwc.UserName + "] Report ID:[" + _dwc.ReportId + "] Topic:[" + _dwc.TopicID + "]. Elapsed Time [" + sw.Elapsed.Hours + ":" + sw.Elapsed.Minutes + ":" + sw.Elapsed.Seconds + "]");
            //System.IO.File.Delete(name);
            Cleanup();
            return file;
        }

        private void Cleanup()
        {
            Cleanup(_path, 2);
        }
        private void Cleanup(string path, int keep)
        {
            try
            {
                foreach (var fi in new DirectoryInfo(path).GetFiles().Where(x => x.LastWriteTime < DateTime.Now.AddMinutes(-10)).OrderByDescending(x => x.LastWriteTime).Skip(keep))
                    fi.Delete();
            }
            catch (Exception ex)
            {
                Log.Write("Cleanup didn't work properly :" + ex.ToString(), thisType);
            }


            if (new DirectoryInfo(path).GetFiles().Length > 45)
            {
                throw new Exception("Server is busy");
            }


        }
        private void SetColumnWidth(int index, int length)
        {
            var t = _listCols[index].Length + 5;
            if (t < 70 && length > t)
            {
                t = length;
            }
            _listCols[index].Width = (((t * 7 + 9) / 7 * 256) / 256);
        }
    }
}




