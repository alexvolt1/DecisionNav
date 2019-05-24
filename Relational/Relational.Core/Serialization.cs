using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.IO;

namespace Relational.Core
{
    public class Serialization
    {
        public static SerializedTable DataTableToJSONRelational(DataTable table)
        {
            //Options can be passed here later 
           
            string datetimeformatter = "Slick.Formatters.ShortDate";

            List<UIColumn> columns = new List<UIColumn>();
            foreach (DataColumn col in table.Columns)
            {
               // if (col.ColumnName == "id" || showId)
               // {
                    UIColumn c = new UIColumn(col.ColumnName);

                    switch (Convert.ToString(col.DataType))
                    {
                        case "System.DateTime": c.formatter = datetimeformatter;
                            break;
                    }

                    columns.Add(c);
               // }
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

        public static SerializedTable DataTableToJSONRelational_Extended(DataTable table)
        {
            //Options can be passed here later 
           
            string datetimeformatter = "Slick.Formatters.ShortDate";

            List<UIColumn> columns = new List<UIColumn>();
            foreach (DataColumn col in table.Columns)
            {
                // if (col.ColumnName == "id" || showId)
                // {
                UIColumn c = new UIColumn(col.ColumnName);

                switch (Convert.ToString(col.DataType))
                {
                    case "System.DateTime": c.formatter = datetimeformatter;
                        break;
                }

                columns.Add(c);
                // }
            }
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                string colDealType = "";
                string[] colDealTypes = { "Agent", "Member", "Sold"};
                foreach (DataColumn col in table.Columns)
                {
                    colDealType = row[table.Columns["DealType"]].ToString();

                    if (col.ColumnName == "Obligor" && (colDealTypes.Contains(colDealType)))
                    {
                        dict[col.ColumnName] = "<span class = \"indented-15\">" + row[col] + "</span>";
                    }
                    else
                    {
                        dict[col.ColumnName] = row[col];
                    }
                }
                list.Add(dict);
            }
            SerializedTable jtbl = new SerializedTable();
            System.Runtime.Serialization.Json.DataContractJsonSerializer columnserializer = 
               new System.Runtime.Serialization.Json.DataContractJsonSerializer(columns.GetType());
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