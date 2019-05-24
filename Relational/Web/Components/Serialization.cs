using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.IO;

namespace Relational.Components
{
    public class Serialization
    {


        internal static SerializedTable DataTableToJSONRelational(RelationalReport rr,DataTable table)
        {
            //Options can be passed here later
           
            string[] reportcolumnswidth = rr.ReportColWidths.Split(',');



            int index = 0;
            bool hideId = true;
            string datetimeformatter = "Slick.Formatters.ShortDate";
            string currencyformatterdollar = "Slick.Formatters.Dollar";


            List<UIColumn> columns = new List<UIColumn>();
            foreach (DataColumn col in table.Columns)
            {
                if (col.ColumnName != "id" || !hideId)
                {
                    UIColumn c = new UIColumn(col.ColumnName);
                    
                    /// Formatter
                    switch (Convert.ToString(col.DataType))
                    {


                        case "System.DateTime": c.formatter = datetimeformatter;
                            break;



                        case "System.Decimal":

                            c.formatter = currencyformatterdollar;
                            c.hasTotal = true;
                            c.Total = rr.ReportTotals[col.ColumnName];
                            break;

                    }
                    ///

                    /// Column Width
                    if (reportcolumnswidth != null && reportcolumnswidth.Length > index)
                    {
                        c.width = Convert.ToInt32(reportcolumnswidth[index]);
                    }
                    /// 


 


                    columns.Add(c);
                }
                index++;
            }
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }
            SerializedTable jtbl = new SerializedTable();
            System.Runtime.Serialization.Json.DataContractJsonSerializer columnserializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(columns.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                columnserializer.WriteObject(ms, columns);
                jtbl.columns = System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            jtbl.data = serializer.Serialize(list);

            return jtbl;
        }

        public static SerializedTable DataTableToJSONTableExtended(DataTable table)
        {
            List<Column> columns = new List<Column>();
            foreach (DataColumn col in table.Columns)
            {
                columns.Add(new Column(col.ColumnName));
            }

            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = row[col];

                }
                list.Add(dict);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            SerializedTable jtbl = new SerializedTable();
            jtbl.columns = serializer.Serialize(columns);
            jtbl.data = serializer.Serialize(list);

            return jtbl;
        }

        public static SerializedTable DataTableToJSONRelational(DataTable table)
        {
            //Options can be passed here later 
            bool hideId = true;
            string datetimeformatter = "Slick.Formatters.ShortDate";

            List<UIColumn> columns = new List<UIColumn>();
            foreach (DataColumn col in table.Columns)
            {
                if (col.ColumnName != "id" || !hideId)
                {
                    UIColumn c = new UIColumn(col.ColumnName);

                    switch (Convert.ToString(col.DataType))
                    {
                        case "System.DateTime": c.formatter = datetimeformatter;
                            break;

                    }

                    columns.Add(c);
                }
            }
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }
            SerializedTable jtbl = new SerializedTable();
            System.Runtime.Serialization.Json.DataContractJsonSerializer columnserializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(columns.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                columnserializer.WriteObject(ms, columns);
                jtbl.columns = System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            jtbl.data = serializer.Serialize(list);

            return jtbl;
        }

    }
}