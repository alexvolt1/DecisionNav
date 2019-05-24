using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DSS.Logs;
namespace Relational.Core.CONVERTERS
{
    class ToJSON
    {
        internal static void BuildColumnsJSON(IRelationalReport reportsession)
        {
            bool showId = true;
            string datetimeformatter = "Slick.Formatters.ShortDate";
            string currencyformatterdollar = "Slick.Formatters.Dollar";
            string percentformatter = "Slick.Formatters.Percent";
            string floatformatter = "Slick.Formatters.Float2";
            string intformatter = "Slick.Formatters.Int";
            string obligorformatter = "Slick.Formatters.Obligor";

            

            Dictionary<string, string> countAndTotals = DATA.ReportData.GetReportCountAndTotals(reportsession);
           
            string[] reportcolumnswidth = reportsession.ReportColWidths.Split(',');
            List<UIColumn> columns = new List<UIColumn>();
            if (showId)
            {
               // var c = new UIColumn("Id");
                var c = new UIColumn();
                c.id = "Id";
                c.name = "Record#";
                c.field = "id";
                c.width = 80;
                columns.Add(c);
            }
            string[] reportcolumns = reportsession.ReportColumns.ToArray(); 
            int index = 0;
            foreach (var s in reportcolumns)
            {
                UIColumn c = new UIColumn(s);
                if (!reportsession.sqlTableSchema.ContainsKey(s))
                {
                   // log.Error("Column [" + s + "] was not found in the table " + reportsession.reportTableName + ". Check spelling or Casing");
                   // continue;
                    c.width = 200;
                    columns.Add(c);
                    continue;
                }
                string sv = reportsession.sqlTableSchema[s];
                /// Formatter
                switch (sv)
                {
                    case "date": c.formatter = datetimeformatter;
                        break;
                    case "datetime": c.formatter = datetimeformatter;
                        break;
                    case "real": c.formatter = percentformatter;
                        c.cssClass = "column-numeric";
                        break;
                    case "float": c.formatter = floatformatter;
                        c.cssClass = "column-numeric";
                        break;
                    case "int": c.formatter = intformatter;
                        c.cssClass = "column-numeric";
                        break;
                    case "decimal":
                        c.formatter = currencyformatterdollar;
                        c.hasTotal = true;
                        decimal t = 0;
                        Decimal.TryParse(countAndTotals["[" + s + "]"],out t);
                        c.Total = t;
                        c.cssClass = "column-numeric";
                        break;
                }
                /// Column Width
                int i = 200;
                if (reportcolumnswidth != null && reportcolumnswidth.Length > index)
                {
                    int.TryParse(reportcolumnswidth[index], out i);
                        //c.width = Convert.ToInt32(reportcolumnswidth[index]);
                }
                c.width = i;
                /// 
                if (s == "Obligor")
                {
                    c.formatter = obligorformatter;
                    c.cssClass = "column-obligor";
                }

                columns.Add(c);
                index++;
            }

 




            SerializedTable jtbl = new SerializedTable();
            System.Runtime.Serialization.Json.DataContractJsonSerializer columnserializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(columns.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                columnserializer.WriteObject(ms, columns);
                jtbl.columns = System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            reportsession.ColumnsJson = jtbl.columns;
        }
    }
}


